using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8f;
    private float PlayerSize;

    private const int _cameraBottomLimit = 2;
    private const int _cameraTopLimit = 15;

    void Start()
    {
        Transform playerSkin = transform.Find("PlayerSkin");
        PlayerSize = playerSkin.localScale.x;
    }

    void Update()
    {
        HandleMovements();
        RotatePlayer();
        ZoomPlayer();
    }

    private void ZoomPlayer()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f && Camera.main.orthographicSize > _cameraBottomLimit)
            Camera.main.orthographicSize -= 0.5f;
        else if (scroll < 0f && Camera.main.orthographicSize < _cameraTopLimit)
            Camera.main.orthographicSize += 0.5f;
    }

    private void RotatePlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandleMovements()
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

        if (WorldsHelper.GetBlockStats(topLeftH, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(topRightH, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(bottomLeftH, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(bottomRightH, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false)
        {
            move += moveH;
        }

        // Collision check for vertical movement
        Vector3 topLeftV = new(futurePositionV.x - PlayerSize / 2, futurePositionV.y + PlayerSize / 2, futurePositionV.z);
        Vector3 topRightV = new(futurePositionV.x + PlayerSize / 2, futurePositionV.y + PlayerSize / 2, futurePositionV.z);
        Vector3 bottomLeftV = new(futurePositionV.x - PlayerSize / 2, futurePositionV.y - PlayerSize / 2, futurePositionV.z);
        Vector3 bottomRightV = new(futurePositionV.x + PlayerSize / 2, futurePositionV.y - PlayerSize / 2, futurePositionV.z);

        if (WorldsHelper.GetBlockStats(topLeftV, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(topRightV, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(bottomLeftV, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false &&
            WorldsHelper.GetBlockStats(bottomRightV, Globals.CurrentWorldId, ChunkTypes.Solid).IsSolid == false)
        {
            move += moveV;
        }

        // Apply the effective move
        transform.position += move;
    }
}
