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
    public bool onGround;

    bool lastOnGround;

    public float xMoveInput;
    bool jumpMove;

    public KeyCode moveXPositive = KeyCode.D;
    public KeyCode moveXNegitive = KeyCode.A;

    public KeyCode jump = KeyCode.W;

    [Header("Effects")]
    public ParticleSystem movementDust;

    ParticleSystem.EmissionModule movementDustEmision;
    public ParticleSystem jumpDust;

    [Header("Janky Animation")]

    public float frameInterval = 1f / 32f;

    float nextAnimTime;
    Sprite nextFrame;
    public SpriteRenderer spriteRenderer;

    public Sprite idle;
    public Sprite jumpFrame;
    public Sprite jumpLandFrame;

    int currentRun;

    public Sprite[] run;

    float accelerationX;
    float lastVX;

    float timeOnGround;

    public AudioSource jumpSound;
    public AudioSource accSound;

    public float jumpOffsetP;
    public float jumpOffsetN;

    // Start is called before the first frame update
    void Start()
    {
        movementDustEmision = movementDust.emission;
        // dustEmissionModule = movementDust.emission;
        nextAnimTime = Time.time + frameInterval;
    }

    void Update()
    {
        accelerationX = (lastVX - body.velocity.x) / Time.deltaTime;

        lastVX = body.velocity.x;

        xMoveInput = (Input.GetKey(moveXPositive) ? 1 : (Input.GetKey(moveXNegitive) ? -1 : 0)) * (canJump() ? 1 : airControl);

        // movementDust.enableEmission = xMoveInput != 0;

        if ((Mathf.Abs(accelerationX) > 1f && onGround) || (onGround && timeOnGround < 0.5f))
        // if (Mathf.Abs(body.velocity.x) < maxMoveSpeed && xMoveInput != 0)
        {
            movementDustEmision.rateOverTime = 75;
            // accSound.volume = Mathf.Lerp(accSound.volume, 0.08f, Time.deltaTime * 12);
            // accSound.pitch = Mathf.Lerp(accSound.volume, 1.2f, Time.deltaTime * 16f);

            // movementDustEmision.rateOverTimeMultiplier = Mathf.MoveTowards(movementDustEmision.rateOverTimeMultiplier, 1, Time.deltaTime * 3);

        }
        else
        {
            if (xMoveInput != 0 && onGround)
            {
                // accSound.volume = Mathf.Lerp(accSound.volume, 0.05f, Time.deltaTime * 8);
                // accSound.pitch = Mathf.Lerp(accSound.volume, 1.2f, Time.deltaTime * 10);
            }
            else
            {
                // accSound.volume = Mathf.Lerp(accSound.volume, 0f, Time.deltaTime * 12);
                // accSound.pitch = Mathf.Lerp(accSound.volume, 0.8f, Time.deltaTime * 10);
            }
            movementDustEmision.rateOverTime = 0;
        }

        accSound.volume = onGround && xMoveInput != 0 ? 0.085f : Mathf.MoveTowards(accSound.volume, 0, Time.deltaTime * 4);

        if (Time.time > nextAnimTime)
        {
            spriteRenderer.sprite = nextFrame;

            if (onGround)
            {
                if (xMoveInput == 0)
                {
                    nextFrame = idle;
                    nextAnimTime = Time.time + frameInterval;
                    currentRun = 0;
                }
                else
                {
                    currentRun++;

                    currentRun = currentRun % run.Length;

                    nextFrame = run[currentRun];
                    nextAnimTime = Time.time + frameInterval;
                }
            }
            else
            {
                if (body.velocity.y > 0)
                {
                    nextFrame = jumpFrame;
                    nextAnimTime = Time.time + frameInterval;
                    currentRun = 0;
                }
                else
                {
                    nextFrame = jumpLandFrame;
                    nextAnimTime = Time.time + frameInterval;
                    currentRun = 0;

                }
            }
        }


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
        Time.fixedDeltaTime = Time.smoothDeltaTime;

        // if (onGround != lastOnGround)
        // {
        //     lastOnGround = onGround;

        //     if (onGround && timeOnGround > 1)
        //     {
        //         // jumpDust.Play();
        //     }
        // }

        if (!onGround) { timeOnGround = 0; } else { timeOnGround += Time.deltaTime; }

    }

    public bool canJump()
    {
        return curCoyoteTime <= maxCoyoteTime;
    }

    void FixedUpdate()
    {
        feetSize = feetCollider.radius * Player.Scale() + 0.05f;

        Move(xMoveInput, Time.fixedDeltaTime);



        // onGround = Physics2D.Raycast(feetPoint.position + (feetPoint.right * 0.05f), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);
        // onGround = Physics2D.Raycast(feetPoint.position + (feetPoint.right * -0.05f), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);

        onGround = Physics2D.Raycast(feetPoint.position + (feetPoint.right * jumpOffsetP), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value) || Physics2D.Raycast(feetPoint.position + (feetPoint.right * -jumpOffsetN), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);

        Debug.DrawRay(feetPoint.position + (feetPoint.right * jumpOffsetP), feetPoint.up * -1, Color.red);
        Debug.DrawRay(feetPoint.position + (feetPoint.right * -jumpOffsetN), feetPoint.up * -1, Color.magenta);

        // onGround = Physics2D.Raycast(feetPoint.position + (feetPoint.right * 0.02f), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value) || Physics2D.Raycast(feetPoint.position + (feetPoint.right * -0.02f), feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);

        // onGround = Physics2D.CircleCast(feetPoint.position, 0.06f, feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);
        // onGround = Physics2D.Raycast(feetPoint.position, feetPoint.up * -1, Player.Scale() / 3, floorLayer.value);
        // onGround = Physics2D.CircleCast(feetPoint.position, feetSize, feetPoint.up * -1, 0, floorLayer.value);

        if (!onGround)
        {
            curCoyoteTime += Time.fixedDeltaTime;
        }
        else
        {
            curCoyoteTime = Mathf.MoveTowards(curCoyoteTime, 0, Time.fixedDeltaTime * 16);
        }
    }

    private void Jump()
    {
        jumpSound.PlayOneShot(jumpSound.clip);
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
