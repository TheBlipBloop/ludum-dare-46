using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float score;
    public int collectedPeices;

    public PlayerMovement movement;

    float startingSpeed;
    float startingSpeedMax;
    float startingJump;
    float startingFeetSize;
    float startingSpeedDecay;

    public bool grow;

    public static Player instance;

    float scale;

    float lastScale;

    void OnEnable()
    {
        instance = this;
        startingSpeedMax = movement.maxMoveSpeed;
        startingSpeed = movement.moveSpeed;
        startingJump = movement.jumpPower;
        startingFeetSize = movement.feetSize;
        startingSpeedDecay = movement.moveSpeedDecayAmount;
    }

    // Update is called once per frame
    void Update()
    {
        scale = Mathf.Clamp(score * 1, 0.15f, 0.5f + (collectedPeices / 10f));

        if (lastScale != scale)
        {
            lastScale = scale;
            transform.localScale = new Vector3(scale * Mathf.Sign(transform.localScale.x), scale, 1);
            transform.position += transform.up * scale / 2f;
        }

        movement.jumpPower = startingJump * scale + 0.3f;
        movement.maxMoveSpeed = startingSpeedMax * scale;
        movement.moveSpeed = startingSpeed * scale;
        movement.feetSize = startingFeetSize * scale;
        movement.moveSpeedDecayAmount = startingSpeedDecay * scale;

        Physics2D.gravity = new Vector2(0, -9.81f * scale);
    }

    public static float Score() { return instance.score; }
    public static float Scale() { return instance.scale; }
}
