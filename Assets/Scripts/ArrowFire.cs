using System.Collections;
using UnityEngine;

public class ArrowFire : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    private float fireY;
    //not using angle errors in arrows
    public float errorDistance = 0.5f;
    private AudioSource Audio;
    public AudioClip AttackClip;

    private void Start()
    {
        fireY = firePoint.position.y;
        Audio = GetComponent<AudioSource>();
    }

    void Fire()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        Audio.PlayOneShot(AttackClip, 0.5f);
        if (hitInfo)
        {
            Player1 player = hitInfo.transform.GetComponent<Player1>();
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (player != null)
            {
                player.Damage();
            }
            else if(enemy != null)
            {
                enemy.Damage();
            }
            //Random.Range(fireY - errorDistance, fireY + errorDistance)
            firePoint.position = new Vector3(firePoint.position.x, fireY, firePoint.position.z);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);

        }

        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }
}
