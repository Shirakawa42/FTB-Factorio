using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    void Update()
    {
        if (Globals.Player != null)
            transform.position = new Vector3(Globals.Player.transform.position.x, Globals.Player.transform.position.y, transform.position.z);
    }
}
