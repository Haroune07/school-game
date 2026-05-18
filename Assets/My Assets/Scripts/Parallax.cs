using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    [Header("Parallax")]
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    private Transform cam;
    private Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;

        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);

        lastCamPos = cam.position;
    }
}