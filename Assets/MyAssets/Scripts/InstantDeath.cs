using UnityEngine;

public class InstantDeath : MonoBehaviour
{
    public Transform respawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision!");
        if (respawnPos != null)
        {
            collider.gameObject.transform.position = respawnPos.position;
        }
    }
}
