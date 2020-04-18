using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Rigidbody2D body;
    public float rb_mass;
    public float rb_drag;
    public float rb_angularDrag;

    void OnEnable()
    {
        Beast.AddProp(this);
    }
    void OnDisable()
    {
        Beast.RemoveProp(this);
    }

    void Update()
    {
        if (CanGrab())
        {
            if (body)
            {
                body.bodyType = RigidbodyType2D.Dynamic;
            }
            spriteRenderer.color = Color.LerpUnclamped(spriteRenderer.color, Color.white, Time.deltaTime * 4);

            if (body == null)
            {
                gameObject.layer = 11;
            }
            else
            {
                gameObject.layer = 9;
            }
        }
        else
        {
            if (body)
            {
                body.bodyType = RigidbodyType2D.Static;
            }
            spriteRenderer.color = Color.LerpUnclamped(spriteRenderer.color, Color.black, Time.deltaTime * 3);
            gameObject.layer = 8;
        }
    }

    public bool CanGrab() { return Mass() < Player.Scale() * 0.65f; }

    public void DisableBody()
    {
        rb_mass = body.mass;
        rb_drag = body.drag;
        rb_angularDrag = body.angularDrag;
        gameObject.layer = 11;
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

    public float Mass()
    {
        return body == null ? rb_mass : body.mass;
    }
}
