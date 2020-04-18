using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{
    public string nextLevel = "";

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
        transform.localScale = new Vector3(Player.Scale() * (transform.position.x - Player.instance.transform.position.x < 0 ? 1 : -1), Player.Scale(), 1);

        List<Prop> p = new List<Prop>(allProps);

        foreach (var curProp in p)
        {
            if (Mathf.Sqrt(Meth.distance(curProp.transform.position, transform.position)) < Player.Scale() * eatRange)
            {
                ConsumeProp(curProp);
            }
        }

        if (allProps.Count < 1 && Meth.distance(Player.instance.transform.position, transform.position) < Player.Scale() * eatRange)
        {
            // Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, transform.position, Time.deltaTime * 18);
            // PlayerCamera.instance.overrideZoomAmount = 0.01f;
        }
        else
        {
            PlayerCamera.instance.overrideZoomAmount = -1f;
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
