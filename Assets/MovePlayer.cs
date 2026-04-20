using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public InputActionReference move;
    Rigidbody2D rb;
    public InputActionReference jumpAction;
    public float jumpSpeed = 10f;

    public InputActionReference mouse1;
    public InputActionReference point;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        Vector3 dir = move.action.ReadValue<Vector2>();

        if(Mathf.Abs(rb.linearVelocity.x) < 8)
        {
            rb.linearVelocity += new Vector2(dir.x, 0) * Time.deltaTime * moveSpeed;
        }

        if (jumpAction.action.WasPressedThisFrame() && Mathf.Abs(rb.linearVelocityY) < .1f)
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }
}
