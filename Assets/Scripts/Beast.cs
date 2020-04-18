﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{

    public Mouth[] mouths = new Mouth[1];

    [System.Serializable]
    public class Mouth
    {
        public Transform point;

        public float range;
    }

    Mouth currentMouth;

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
        for (int currentMouthID = 0; currentMouthID < mouths.Length; currentMouthID++)
        {
            currentMouth = mouths[currentMouthID];

            List<Prop> p = new List<Prop>(allProps);

            foreach (var curProp in p)
            {
                if (Mathf.Sqrt(Meth.distanceSq(curProp.transform.position, currentMouth.point.position)) < transform.lossyScale.x * 1.28f)
                {
                    ConsumeProp(curProp);
                }
            }
        }
    }

    public void ConsumeProp(Prop prop)
    {
        Player.instance.score += 1.35f * (prop.body ? prop.body.mass : prop.rb_mass);
        Player.instance.collectedPeices++;
        Destroy(prop.gameObject);
    }
}
