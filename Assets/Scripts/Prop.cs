using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Rigidbody2D body;
    public float rb_mass;
    public float rb_drag;
    public float rb_angularDrag;

    public void DisableBody()
    {
        rb_mass = body.mass;
        rb_drag = body.drag;
        rb_angularDrag = body.angularDrag;
        gameObject.layer = 10;
        Destroy(body);
    }

    public void EnableBody()
    {
        body = gameObject.AddComponent<Rigidbody2D>();
        body.mass = rb_mass;
        body.drag = rb_drag;
        body.angularDrag = rb_angularDrag;
        gameObject.layer = 9;
    }
}
