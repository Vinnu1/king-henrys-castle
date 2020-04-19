using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Inhit
{
    public Transform startPoint, endPoint;
    private Vector3 currentTarget;
    public float speed;
    private Animator enemyAnimator;
    private BoxCollider2D enemyCollider;
    private CircleCollider2D enemyCollider2;
    protected Player player;
    private bool fighting = false;
    public int health;
    private bool dead = false;
    //public AudioClip hitSound;
    //private AudioSource Audio;

    virtual public void Init()
    {
        currentTarget = endPoint.position;
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyCollider2 = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Wait())
        {
            return;
        }
        if (!dead)
        {
            Move(4.0f);
        }
    }

    virtual public bool Wait()
    {
        if ((enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) && (enemyAnimator.GetBool("Attack") == false))
        {
            return true;
        }
        else
            return false;
    }

    //Move function with dis(how close to enemy for attack range) to set attack and enemyType 
    virtual public void Move(float dis, string enemyType = "general")
    {
        
        if(transform.position == startPoint.position)
        {
            Flip(false);
            currentTarget = endPoint.position;
            enemyAnimator.SetTrigger("Idle");
        }
        else if(transform.position == endPoint.position)
        {
            Flip(true);
            currentTarget = startPoint.position;
            enemyAnimator.SetTrigger("Idle");
        }

        //distance btw enemy and player
        float distance = Vector2.Distance(player.transform.localPosition, transform.localPosition);
        //Debug.Log("distance:" + distance);
        if (distance > dis && dis > 0)
        {
            fighting = false;
            enemyAnimator.SetBool("Attack", false);
        }
        else
        {
            fighting = true;
            enemyAnimator.SetTrigger("Idle");
            enemyAnimator.SetBool("Attack", true);
            //enemyAnimator.SetTrigger("Idle");
        }

        Vector3 direction = player.transform.position - transform.position;
        if (direction.x > 0 && enemyAnimator.GetBool("Attack") == true)
        {
            Flip(false);
            currentTarget = endPoint.position;
        }
        else if (direction.x < 0 && enemyAnimator.GetBool("Attack") == true)
        {
            Flip(true);
            currentTarget = startPoint.position;
        }

        if (fighting == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed*Time.deltaTime);
        }
        if(enemyType != "Archer")
            enemyAnimator.SetBool("Walk", true);
    }

    public void Damage()
    {
        if (health > 0)
        {
            health--;
            if(health == 0)
            {
                enemyCollider.enabled = false;
                if (enemyCollider2)
                {
                    enemyCollider2.enabled = false;
                }
                enemyAnimator.SetTrigger("Death");
                dead = true;
                //Push this to death function
                //StartCoroutine(Erase());
            }
            else
            {
                enemyAnimator.SetTrigger("Hit");
                
            }
        }
    }

    public void Death()
    {
        //enemies won't die in one hit, so nothing to do
    }

    private void Flip(bool state)
    {
        //transform.Rotate(0f, 180f, 0f);
        //if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        
            if (state)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator Erase()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy();
    }
}
