using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8f;

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        if (movement.magnitude > 1.0f)
        {
            movement.Normalize();
        }
        transform.position += Speed * Time.deltaTime * movement;
    }
}
