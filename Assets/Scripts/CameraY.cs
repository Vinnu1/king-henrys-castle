using UnityEngine;

public class CameraY : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    private Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cameraPos = transform.position;
        cameraPos.y = (player.transform.position + offset).y;
        transform.position = cameraPos;
    }
}
