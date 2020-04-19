using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool _damageable = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //has inhit interface
        Inhit hit = collision.GetComponent<Inhit>();
        if(hit != null)
        {

            //Debug.Log("Hit " + collision.name);
            if (_damageable)
            {
                if (transform.GetComponentInParent<Slime>())
                {
                    hit.Death();
                }
                else
                {
                    hit.Damage();
                }
                _damageable = false;
                StartCoroutine(ResetDamageable());
            }
        }
    }

    IEnumerator ResetDamageable()
    {
        yield return new WaitForSeconds(0.3f);
        _damageable = true;
    }
}
