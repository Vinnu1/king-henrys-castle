using System.Collections;
using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    void AfterDeathAnim()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
    }

}
