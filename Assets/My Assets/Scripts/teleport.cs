using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform teleportPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.CompareTag("Player") || collision.gameObject.GetComponent<MovePlayer>() != null)
        {
            collision.gameObject.transform.position = teleportPoint.position;
        }
    }
}
