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

    public CircleCollider2D feetCollider;
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
    public float xMoveInput;
    bool jumpMove;

    public KeyCode moveXPositive = KeyCode.D;
    public KeyCode moveXNegitive = KeyCode.A;

    public KeyCode jump = KeyCode.W;

    // [Header("Effects")]
    // public ParticleSystem movementDust;
    // public ParticleSystem.EmissionModule dustEmissionModule;

    // Start is called before the first frame update
    void Start()
    {
        // dustEmissionModule = movementDust.emission;
    }

    void Update()
    {
        xMoveInput = (Input.GetKey(moveXPositive) ? 1 : (Input.GetKey(moveXNegitive) ? -1 : 0)) * (canJump() ? 1 : airControl);

        if (Input.GetKey(moveXNegitive) && Input.GetKey(moveXPositive))
        {

        }
        else
        {
            if (Input.GetKey(moveXNegitive))
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }

            if (Input.GetKey(moveXPositive))
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            }

        }

        if (Input.GetKeyDown(jump) && canJump())
        {

            Jump();
        }

        // dustEmissionModule.rateOverDistanceMultiplier = Mathf.Lerp(dustEmissionModule.rateOverDistanceMultiplier, Mathf.Abs(xMoveInput) > 0.5f ? 1 : 0, Time.deltaTime);

    }

    bool canJump()
    {
        return curCoyoteTime <= maxCoyoteTime;
    }

    void FixedUpdate()
    {
        feetSize = feetCollider.radius * Player.Scale() + 0.05f;

        Move(xMoveInput, Time.fixedDeltaTime);

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
        curCoyoteTime = maxCoyoteTime + 1;
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
