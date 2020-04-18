using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mice : MonoBehaviour
{
    public Camera self;

    public static Mice instance;

    void Awake()
    {
        instance = this;
    }

    public static Vector3 worldMousePosition() { return instance.self.ScreenToWorldPoint(screenMousePosition()); }

    public static Vector3 screenMousePosition() { return Input.mousePosition; }
}
