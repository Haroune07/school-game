using UnityEngine;

public class CemeraMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform player;
    public float smoothSpeed = 5f;
    public float yOffset = 1.5f;

    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
