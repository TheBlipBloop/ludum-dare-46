﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsetParent : MonoBehaviour
{
    void Awake() { transform.SetParent(null); }
}
