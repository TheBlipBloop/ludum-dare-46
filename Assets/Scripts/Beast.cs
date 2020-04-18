using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{
    public float eatRange;

    public Transform upperMouth;

    static HashSet<Prop> allProps = new HashSet<Prop>();


    public static void AddProp(Prop prop)
    {
        allProps.Add(prop);
    }

    public static void RemoveProp(Prop prop)
    {
        allProps.Remove(prop);
    }

    // Update is called once per frame
    void Update()
    {
        List<Prop> p = new List<Prop>(allProps);

        foreach (var curProp in p)
        {
            if (Mathf.Sqrt(Meth.distanceSq(curProp.transform.position, transform.position)) < transform.localScale.x * eatRange)
            {
                ConsumeProp(curProp);
            }
        }

        upperMouth.localEulerAngles = Vector3.LerpUnclamped(upperMouth.localEulerAngles, new Vector3(0, 0, 0), Time.deltaTime * 3);
    }

    public void ConsumeProp(Prop prop)
    {
        Player.instance.score += 1.35f * (prop.body ? prop.body.mass : prop.rb_mass);
        Player.instance.collectedPeices++;
        upperMouth.localEulerAngles = new Vector3(0, 0, Random.Range(50f, 70f));
        Destroy(prop.gameObject);
    }
}
