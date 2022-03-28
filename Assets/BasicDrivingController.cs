using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDrivingController : MonoBehaviour

// Script Inspiration from <https://www.youtube.com/watch?v=Z4HA8zJhGEk>
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private bool isBreaking;
    private bool hasNitroOn;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;


    public float resetDistance = 10f;
    public float nitroSpeed = 100f;

    private float _originalRotX;
    private float _originalRotZ;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleNitro();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        hasNitroOn = Input.GetKey(KeyCode.LeftShift);

        // Reset the car
        if (Input.GetKey(KeyCode.R))
        {
            // Move the rigidbody up by 3 metres
            transform.Translate(0, resetDistance, 0, Space.World);
            // Reset the rotation to what it was when the car was initialized
            transform.rotation = (Quaternion.Euler(new Vector3(_originalRotX, transform.rotation.y, _originalRotZ)));
        }
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? brakeForce : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    private void HandleNitro() {
        if (hasNitroOn) {
            // check if at least one wheel collider touches the ground
            transform.position += transform.forward * nitroSpeed;
        }
    }
}
