using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
NOTE: this applies for all scripts

I worked on this game as a side project on breaks, throughout which my knowledge of Unity/C# increased.
So some scripts might have better optimized code and some might not. In this script only, I can see repeatable blocks for which I
should make a separate function, but I can't due to shortage of time. Having said that, this code works smoothly and I believe
is easy to understand. Obviously, you can fork and make it better!

*/

public class Player : MonoBehaviour, Inhit
{
    private Rigidbody2D playerRgd;
    public float speed;
    public float jump;
    public Transform groundTrans;
    public LayerMask theGround;
    public LayerMask theEnemy;
    private float checkRadius = 0.8f;
    private BoxCollider2D playerCollider;
    private CircleCollider2D circleCollider;
    public bool ground;
    private Animator anim;
    public Animator loadSceneAnimator;
    //to stop jump animation
    private bool stopJump = false;
    public int health;
    public bool dead = false;
    private float airTime;
    public Slider slider;
    public float stamina = 1f;
    public Image sliderBG;
    public Image healthScreen;
    public Color redColor = new Color(255, 0, 0);
    public bool healthProcessing = false;
    private AnimationActions animActions;
    private bool isPaused = false;
    public bool flip = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Cursor.visible = false;
        playerRgd = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        redColor.a = 0;
        animActions = FindObjectOfType<AnimationActions>();
        Physics2D.IgnoreLayerCollision(9, 10, false);
        if (flip)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((!dead && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) && !isPaused)
        {
            Move("player");
            Sprint();
            Attack();
            if (health < 4 && healthProcessing == false)
            {
                healthProcessing = true;
                StartCoroutine("RefillHealth");
            }
        }
    }

    void Sprint()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        //Debug.Log(horizontalMove);
        //removed jump from stamina - || Input.GetButton("Jump")
        if ((Input.GetButton("Fire3") && (horizontalMove != 0) && stamina > 0.011f))
        {
            anim.SetFloat("AnimSpeedMultiplier", 1.5f);
            speed = 20;
            jump = 25;
            stamina -= Time.deltaTime / 2f;
            //StartCoroutine(Stamina("deplete"));

        }
        else
        {
            anim.SetFloat("AnimSpeedMultiplier", 1f);
            speed = 10;
            jump = 22;
            stamina += Time.deltaTime / 3f;
            //StartCoroutine(Stamina("regenerate"));
        }
        stamina = Mathf.Clamp01(stamina);
        //Debug.Log("Stamina:" + stamina);
        if (stamina > 0.02f)
            slider.value = stamina;
    }

    public virtual void Move(string characterType)
    {
        ground = Physics2D.OverlapCircle(groundTrans.position, checkRadius, theGround)
            || Physics2D.OverlapCircle(groundTrans.position, checkRadius, theEnemy);

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (characterType == "enemy")
        {
            horizontalMove = -horizontalMove;
        }
        if (horizontalMove < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalMove > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown("space") && ground)
        {

            animJump(true);
            playerRgd.velocity = new Vector2(playerRgd.velocity.x, jump);
            StartCoroutine(StopJumping());
        }
        if (stopJump && ground)
        {
            animJump(false);
            stopJump = false;
        }

        //while in air without jump
        if (!ground && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            airTime += Time.deltaTime;
            if (airTime > 0.5)
            {
                anim.SetBool("InAir", true);
            }
        }
        else
        {
            anim.SetBool("InAir", false);
            airTime = 0;
        }
        playerRgd.velocity = new Vector2(horizontalMove * speed, playerRgd.velocity.y);
        animMove(horizontalMove);
    }

    public void Attack()
    {
        if (Input.GetKeyDown("e"))
        {
            if (stamina > 0.3f)
            {
                animAttack();
            }
            else
            {
                StartCoroutine(SliderBGChange());
            }

        }
    }

    public void Damage()
    {
        if (health > 0)
        {
            health--;
            redColor.a += 0.1f; //1 - health/4;
            healthScreen.color = redColor;
            //Debug.Log("Health:" + health);
            if (health == 0)
            {
                StopCoroutine("RefillHealth");
                Death();
            }
            else
            {
                animHit();
                //force, no need to add
                //playerRgd.AddForce(transform.right * 200f);
                StopCoroutine("RefillHealth");
                healthProcessing = false;
            }
        }
    }

    IEnumerator RefillHealth()
    {
        if (health == 1)
        {
            yield return new WaitForSeconds(10f);
        }
        else
        {
            yield return new WaitForSeconds(5f);
        }
        //Debug.Log("refill called");
        //if (redColor.a > 0)
        //    {
        redColor.a -= 0.1f;
        healthScreen.color = redColor;
        //    }
        health++;
        //Debug.Log("Health:" + health);
        healthProcessing = false;
    }

    IEnumerator SliderBGChange()
    {
        sliderBG.color = Color.red;
        animActions.StopSound();
        yield return new WaitForSeconds(0.1f);
        sliderBG.color = Color.white;
    }

    IEnumerator StopJumping()
    {
        yield return new WaitForSeconds(0.1f);
        stopJump = true;
    }

    public virtual IEnumerator RestartWait()
    {
        yield return new WaitForSeconds(2f);
        Restart("player");

    }

    public virtual void Restart(string characterType)
    {
        if (characterType == "player")
        {
            Scene scene = SceneManager.GetActiveScene();
            //Debug.Log(scene);
            SceneManager.LoadScene(scene.name);
        }
        else if(characterType == "enemy")
        {
            bool playerDead = FindObjectOfType<Player1>().dead;
            if (playerDead == false)
            {
                loadSceneAnimator.SetTrigger("FadeIn");
            }
        }
    }


    //animator functions
    public void animMove(float move)
    {
        anim.SetFloat("Run", Mathf.Abs(move));
    }

    public void animJump(bool state)
    {
        anim.SetBool("Jump", state);
    }

    public void animHit()
    {
        anim.SetTrigger("Hit");
    }

    public void animDead()
    {
        anim.SetTrigger("Death");
        playerRgd.velocity = new Vector2(0, 0);
    }

    virtual public void Death()
    {
        if (!dead)
        {
            //death screen color
            redColor.a = 0.4f;
            healthScreen.color = redColor;

            //remove stamina bar
            slider.gameObject.SetActive(false);

            //modify colliders to cover the dead body sprite
            playerCollider.size = new Vector2(5.285851f, 1.644344f);
            playerCollider.offset = new Vector2(0.6475415f, -1.447018f);

            //play death animation
            animDead();

            //remove collision so player can die touching ground
            Physics2D.IgnoreLayerCollision(9, 10);

            circleCollider.enabled = false;

            //dont play in air animation
            anim.SetBool("InAir", false);

            dead = true;
            //restart level
            StartCoroutine(RestartWait());
        }
    }

    public void animAttack()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anim.SetTrigger("Attack");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spikes")
        {
            if (!dead)
            {
                Death();
            }
        }
        else if(collision.tag == "EndlessFall")
        {
            Debug.Log("endless");
            Restart("player");
        }
    }

    //pause while menu is open
    public void TogglePause()
    {
        if (isPaused)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }
}
