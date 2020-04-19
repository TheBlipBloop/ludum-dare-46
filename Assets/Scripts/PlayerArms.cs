using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public PlayerMovement player;

    public Prop currentProp;

    public Transform armParent;

    public float offset = 0;

    public Transform lockPoint;

    public LayerMask grabbedPropLayerMask;
    public LayerMask propLayerMask;

    Vector2 draggedArmPosition;

    Vector2 grabbedOffset;

    public AudioSource grabEffect;
    public AudioSource ungrabEffect;

    public AudioSource moveArmEffect;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Grab();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            UnGrab();
        }

        if (currentProp)
        {
            // currentProp.transform.localPosition = Vector3.MoveTowards(currentProp.transform.localPosition, Vector2.zero, Time.deltaTime);
            // currentProp.transform.localPosition = Vector3.MoveTowards(currentProp.transform.localPosition, lockPoint.InverseTransformPoint(currentProp.transform.TransformPoint(grabbedOffset)), Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.Mouse1) || currentProp)
        {
            // armParent.eulerAngles = new Vector3(0, 0, Meth.pointToDegree(Mice.worldMousePosition(), armParent.position) + offset);
            armParent.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(armParent.eulerAngles.z, Meth.pointToDegree(Mice.worldMousePosition(), armParent.position) + offset, Time.deltaTime * 6f));
        }
        else
        {
            float targetAngle = (((player.xMoveInput * -40) - 90 + offset) % 360);
            armParent.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(armParent.eulerAngles.z, targetAngle, Time.deltaTime * 3));
        }

        // Time.timeScale = Mathf.MoveTowards(Time.time, currentProp && !player.onGround ? 0.8f : 1, Time.deltaTime * 3);
    }

    public void Grab()
    {
        Prop targetProp = TryGrab();

        if (!targetProp)
        {
            return;
        }

        if (!targetProp.CanGrab()) { return; }



        targetProp.DisableBody();

        targetProp.transform.parent = lockPoint;

        grabEffect.transform.position = targetProp.transform.position;
        grabEffect.PlayOneShot(grabEffect.clip);

        currentProp = targetProp;

        // RaycastHit2D hit = Physics2D.Raycast(armParent.position, lockPoint.up, 0.5f, grabbedPropLayerMask.value);
        // if (hit.collider)
        {
            // lockPoint.position = hit.point;
            // Debug.Log("HIT " + hit.collider.gameObject.name);
            // Debug.DrawRay(hit.point, Vector2.up, Color.red, 15);
            // grabbedOffset = currentProp.transform.InverseTransformPoint(hit.point);
        }
    }

    public void UnGrab()
    {
        if (!currentProp) { return; }
        currentProp.transform.parent = null;
        currentProp.EnableBody();
        currentProp.body.velocity = lockPoint.up * 8 * Player.Scale();
        // currentProp.body.AddForce(lockPoint.up * 4 * Player.Scale(), ForceMode2D.Impulse);

        ungrabEffect.PlayOneShot(ungrabEffect.clip);
        ungrabEffect.transform.position = currentProp.transform.position;


        currentProp = null;
        player.body.AddForce(new Vector2(lockPoint.up.x * -3, lockPoint.up.y * -5) * Player.Scale(), ForceMode2D.Impulse);
        // player.body.AddForce(new Vector2(0, 5f) * Player.Scale(), ForceMode2D.Impulse);
    }

    public Prop TryGrab()
    {
        RaycastHit2D hit = Physics2D.CircleCast(lockPoint.position, Player.Scale() * 0.35f, lockPoint.up, Player.Scale() * 0.3f, propLayerMask.value);
        GameObject hitObj = hit.collider ? hit.collider.gameObject : null;

        if (hitObj)
        {
            return hitObj.GetComponent<Prop>();
        }

        return null;
    }
}
