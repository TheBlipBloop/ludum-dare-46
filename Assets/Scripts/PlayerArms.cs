using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArms : MonoBehaviour
{
    public Prop currentProp;

    public Transform armParent;

    public float offset = 0;

    public Transform lockPoint;

    public LayerMask propLayerMask;

    // Update is called once per frame
    void Update()
    {
        armParent.eulerAngles = new Vector3(0, 0, Meth.pointToDegree(Mice.worldMousePosition(), armParent.position) + offset);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Grab();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            UnGrab();
        }

    }

    public void Grab()
    {
        Prop targetProp = TryGrab();

        if (!targetProp)
        {
            return;
        }

        targetProp.DisableBody();

        targetProp.transform.parent = lockPoint;
        currentProp = targetProp;
    }

    public void UnGrab()
    {
        if (!currentProp) { return; }
        currentProp.transform.parent = null;
        currentProp.EnableBody();
        currentProp.body.AddForce(lockPoint.up * 8, ForceMode2D.Impulse);
        currentProp = null;
    }

    public Prop TryGrab()
    {
        RaycastHit2D hit = Physics2D.CircleCast(lockPoint.position, 0.5f, lockPoint.up, 0.3f, propLayerMask.value);
        GameObject hitObj = hit.collider ? hit.collider.gameObject : null;

        if (hitObj)
        {
            return hitObj.GetComponent<Prop>();
        }

        return null;
    }
}
