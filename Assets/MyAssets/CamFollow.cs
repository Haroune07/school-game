using UnityEngine;

public class CamFollow : MonoBehaviour
{
    private Vector3 offset;
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        cam.transform.position = transform.position + offset;
    }
}
