using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float score;

    public PlayerMovement movement;

    float startingSpeed;
    float startingSpeedMax;
    float startingJump;
    float startingFeetSize;
    float startingSpeedDecay;

    public bool grow;

    public static Player instance;

    float scale;

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
        scale = Mathf.Clamp(score / 2f, 0.15f, 50f);
        transform.localScale = new Vector3(scale * Mathf.Sign(transform.localScale.x), scale, 1);

        movement.jumpPower = startingJump * scale + 0.3f;
        movement.maxMoveSpeed = startingSpeedMax * scale;
        movement.moveSpeed = startingSpeed * scale;
        movement.feetSize = startingFeetSize * scale;
        movement.moveSpeedDecayAmount = startingSpeedDecay * scale;
    }

    public static float Score() { return instance.score; }
    public static float Scale() { return instance.scale; }
}
