using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float walkSpeed = 5f;
    public InputActionReference move;
    public InputActionReference sprint;
    public float sprintSpeed = 10;
    float currentSpeed;
    Rigidbody2D rb;
    public InputActionReference jumpAction;
    public float jumpSpeed = 10f;
    Animator anim;
    Vector3 initalScale;

    public string yVelFloat = "YVelocity";
    string isRunningBool = "IsRunning";
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initalScale = transform.localScale;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        Vector2 input = move.action.ReadValue<Vector2>();

        if(sprint.action.IsPressed()){
            currentSpeed = sprintSpeed;
        }else{
            currentSpeed = walkSpeed;
        }

        if(input.x >= 0){
            transform.localScale = initalScale;
        }else{
            transform.localScale = new Vector3(initalScale.x * -1, initalScale.y, initalScale.z);
        }

        transform.position += new Vector3(input.x, 0, 0) * Time.deltaTime * currentSpeed;

        anim.SetBool(isRunningBool, input.magnitude > 0);

        if (jumpAction.action.WasPressedThisFrame() && Mathf.Abs(rb.linearVelocityY) < .1f)
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }

        anim.SetFloat(yVelFloat, rb.linearVelocityY);
    }
}