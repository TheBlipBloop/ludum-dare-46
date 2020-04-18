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

    [Header("Movement")]

    public AnimationCurve XAnimationCurve;
    /// <summary>
    /// 0 == no air control, 1 == full control
    /// </summary>
    public float airControl = 0.6f;
    public float moveSpeed = 6;
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
    }

    bool canJump()
    {
        return curCoyoteTime <= maxCoyoteTime;
    }

    void FixedUpdate()
    {
        Move(xMove, ref jumpMove);
        onGround = Physics2D.Raycast(feetPoint.position, feetPoint.up * -1, 0.5f, floorLayer.value);

        if (!onGround)
        {
            curCoyoteTime += Time.fixedDeltaTime;
        }
        else
        {
            curCoyoteTime = 0;
        }
    }

    private void Move(float x, ref bool jump)
    {
        body.velocity = new Vector2(x * moveSpeed, body.velocity.y);

        if (jump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            jump = false;
        }

    }
}
