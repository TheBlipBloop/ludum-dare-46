using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;

    public Camera self;
    Vector2 curPosition;

    public float overrideZoomAmount = -1;

    public static PlayerCamera instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screnMid = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mouseOffset = Vector2.zero;

        if (overrideZoomAmount < 0)
        {
            mouseOffset = (Mice.screenMousePosition() - screnMid) / 400 * Player.Scale();
        }

        curPosition = Vector2.Lerp(curPosition, player.transform.position, Time.deltaTime * 4);

        transform.position = (Vector3)curPosition + (Vector3)mouseOffset + new Vector3(0, 0, -10);
        if (overrideZoomAmount < 0)
        {
            self.orthographicSize = Mathf.Lerp(self.orthographicSize, Mathf.Clamp(Player.Scale() * 5f, 0.35f, 100f), Time.deltaTime * 0.4f);
        }
        else
        {
            self.orthographicSize = Mathf.Lerp(self.orthographicSize, overrideZoomAmount, Time.deltaTime * 0.4f);
        }
    }
}
