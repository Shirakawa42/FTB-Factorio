using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8f;
    public float PlayerSize;

    void Start()
    {
        Transform playerSkin = transform.Find("PlayerSkin");
        PlayerSize = playerSkin.localScale.x;
    }

    void Update()
    {
        Vector3 movement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if (movement.magnitude > 1.0f)
        {
            movement.Normalize();
        }

        Vector3 futurePositionH = transform.position + new Vector3(Speed * Time.deltaTime * movement.x, 0f, 0f);
        Vector3 futurePositionV = transform.position + new Vector3(0f, Speed * Time.deltaTime * movement.y, 0f);

        Vector3 moveH = new(Speed * Time.deltaTime * movement.x, 0f, 0f);
        Vector3 moveV = new(0f, Speed * Time.deltaTime * movement.y, 0f);
        Vector3 move = new(0f, 0f, 0f);

        // Collision check for horizontal movement
        Vector3 topLeftH = new(futurePositionH.x - PlayerSize / 2, futurePositionH.y + PlayerSize / 2, futurePositionH.z);
        Vector3 topRightH = new(futurePositionH.x + PlayerSize / 2, futurePositionH.y + PlayerSize / 2, futurePositionH.z);
        Vector3 bottomLeftH = new(futurePositionH.x - PlayerSize / 2, futurePositionH.y - PlayerSize / 2, futurePositionH.z);
        Vector3 bottomRightH = new(futurePositionH.x + PlayerSize / 2, futurePositionH.y - PlayerSize / 2, futurePositionH.z);

        if (Globals.GetSolidBlockStatsFromWorldPosition(topLeftH).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(topRightH).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(bottomLeftH).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(bottomRightH).IsSolid == false)
        {
            move += moveH;
        }

        // Collision check for vertical movement
        Vector3 topLeftV = new(futurePositionV.x - PlayerSize / 2, futurePositionV.y + PlayerSize / 2, futurePositionV.z);
        Vector3 topRightV = new(futurePositionV.x + PlayerSize / 2, futurePositionV.y + PlayerSize / 2, futurePositionV.z);
        Vector3 bottomLeftV = new(futurePositionV.x - PlayerSize / 2, futurePositionV.y - PlayerSize / 2, futurePositionV.z);
        Vector3 bottomRightV = new(futurePositionV.x + PlayerSize / 2, futurePositionV.y - PlayerSize / 2, futurePositionV.z);

        if (Globals.GetSolidBlockStatsFromWorldPosition(topLeftV).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(topRightV).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(bottomLeftV).IsSolid == false &&
            Globals.GetSolidBlockStatsFromWorldPosition(bottomRightV).IsSolid == false)
        {
            move += moveV;
        }

        // Apply the effective move
        transform.position += move;
    }
}
