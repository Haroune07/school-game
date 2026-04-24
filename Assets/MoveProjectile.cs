using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    public float flySpeed = 4;
    private float startX;
    public float startDeath = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(1 * flySpeed,0,0) * Time.deltaTime;

        if(Mathf.Abs(transform.position.x - startX) >= startDeath)
        {
            Destroy(gameObject);
        }
    }
}
