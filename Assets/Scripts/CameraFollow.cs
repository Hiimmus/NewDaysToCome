using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform playerTransform;
    public Vector3 offset;
    // public bool onPlayer = true;
 void LateUpdate()
    {
        transform.position = playerTransform.position + offset;

      
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(-90);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(90);
        }
    }

    private void RotateCamera(float angle)
    {
      
        transform.RotateAround(playerTransform.position, Vector3.up, angle);

     
        offset = transform.position - playerTransform.position;
    }

}
