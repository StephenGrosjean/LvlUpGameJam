using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public void TeleportPlayer(Vector3 position, Teleporter.Gravity gravityDirection)
    {
        transform.position = position;

        if (gravityDirection == Teleporter.Gravity.UP)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            Physics2D.gravity = new Vector3(0, 9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.DOWN)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            Physics2D.gravity = new Vector3(0, -9.81f, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.LEFT)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            Physics2D.gravity = new Vector3(-9.81f, 0, 0);
        }
        else if (gravityDirection == Teleporter.Gravity.RIGHT)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            Physics2D.gravity = new Vector3(9.81f, 0, 0);
        }
    }
}
