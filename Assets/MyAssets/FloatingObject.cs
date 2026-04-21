using UnityEngine;
using UnityEngine.UI;

public class FloatingObject : MonoBehaviour
{

    public float amplitude = 10;
    public float freq = 4;
    public float startingPoint = 0;

    float initialYPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialYPos = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, initialYPos + Mathf.Sin(Time.time * freq + startingPoint) * amplitude, transform.position.z);
    }
}
