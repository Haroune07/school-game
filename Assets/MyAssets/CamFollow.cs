using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Vector3 offset;
    public Camera cam;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = cam.transform.position - transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.transform.position = transform.position + offset;

        if (Mathf.Abs(rb.linearVelocityY) > 15)
        {
            cam.orthographicSize = 20;
        }
        else
        {
            cam.orthographicSize = 10;
        }
    }
}
