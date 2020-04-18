﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScaleToTransform : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = target.localScale;
    }
}
