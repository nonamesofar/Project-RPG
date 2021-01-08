using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHelper : MonoBehaviour
{
    public float targetAngle = 20;
    
    public float turnSmoothTime = 0f;

    public void RotateLeft()
    {
        float turnSmoothVelocity = 5;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.RotateAround(transform.position, Vector3.up, angle);
    }
    public void RotateRight()
    {
        float turnSmoothVelocity = 5;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, -targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.RotateAround(transform.position, Vector3.up, angle);
    }
}
