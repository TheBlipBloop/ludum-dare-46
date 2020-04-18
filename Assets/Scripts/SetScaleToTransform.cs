using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScaleToTransform : MonoBehaviour
{
    public Transform target;

    public float scale = 1;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(Mathf.Abs(target.localScale.x), target.localScale.y, 1) * scale;
    }
}
