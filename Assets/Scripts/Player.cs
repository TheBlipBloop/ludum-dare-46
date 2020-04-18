using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float score;

    public bool grow;

    public static Player instance;

    void OnEnable()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (grow)
        {
            float scale = Mathf.Clamp(score / 2f, 1f, 50f);
            transform.localScale = new Vector3(scale * Mathf.Sign(transform.localScale.x), scale, 1);
        }
    }
}
