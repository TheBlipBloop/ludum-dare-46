using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D body;
    public LayerMask floorLayer;


    [Header("Transforms")]
    public Transform arm;
    public Transform feetPoint;
    public float feetSize = 0.6f;

    [Header("Movement")]

    /// <summary>
    /// 0 == no air control, 1 == full control
    /// </summary>
    public float airControl = 0.6f;
    public float moveSpeed = 12;
    public float maxMoveSpeed = 6;
    public float moveSpeedDecayAmount = 36;
    public float jumpPower = 3;
    public float maxCoyoteTime = 0.2f;

    float curCoyoteTime = 0;
    bool onGround;
    float xMove;
    bool jumpMove;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        xMove = (Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0)) * (canJump() ? 1 : airControl);


        jumpMove = Input.GetKeyDown(KeyCode.W) && canJump();

        if (jumpMove)
        {
            Jump();
        }
    }

    bool canJump()
    {
        return curCoyoteTime <= maxCoyoteTime;
    }

    void FixedUpdate()
    {

        Move(xMove, Time.fixedDeltaTime);
        onGround = Physics2D.CircleCast(feetPoint.position, feetSize, feetPoint.up * -1, 0, floorLayer.value);

        if (!onGround)
        {
            curCoyoteTime += Time.fixedDeltaTime;
        }
        else
        {
            curCoyoteTime = 0;
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }

    private void Move(float x, float dt)
    {
        body.velocity += new Vector2(x * moveSpeed * dt, 0);

        if (Mathf.Abs(x) < 0.1f)
        {
            body.velocity = new Vector2(Mathf.MoveTowards(body.velocity.x, 0, moveSpeedDecayAmount * Time.fixedDeltaTime), body.velocity.y);
        }

        UpdateBodyClamp();
    }

    private void UpdateBodyClamp()
    {
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxMoveSpeed, maxMoveSpeed), body.velocity.y);
    }
}
