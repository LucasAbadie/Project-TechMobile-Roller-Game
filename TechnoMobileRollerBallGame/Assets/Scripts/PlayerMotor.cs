using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    private float movementSpeed = 8.0f;
    private float drag = 0.8f;
    private float terminalRotationSpeed = 25.0f;

    private float boostSpeed = 30.0f;
    private float boostCooldown = 4.0f;
    private float lastBoost;

    public VirtualJoystick moveJoystick;

    private Rigidbody controller;
    private Transform cameraTransform;

    private float startTime = 0;
    private const float timeBeforeStart = 2.5f;

    // Use this for initialization
    void Start () {
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;

        cameraTransform = Camera.main.transform;

        startTime = Time.time;

        lastBoost = Time.time - boostCooldown;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time - startTime < timeBeforeStart)
            return;

        if (GameManager.Instance.IsPlayed != 0)
        {
            Vector3 dir = Vector3.zero;

            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");

            if (dir.magnitude > 1)
                dir.Normalize();

            if (moveJoystick.InputDirection != Vector3.zero)
            {
                dir = moveJoystick.InputDirection;
            }

            // Rotate our direction vector with camera
            Vector3 rotateDir = cameraTransform.TransformDirection(dir);
            rotateDir = new Vector3(rotateDir.x, 0, rotateDir.z);
            rotateDir = rotateDir.normalized * dir.magnitude;

            controller.AddForce(rotateDir * movementSpeed);
        }
	}

    public void boostSpeedPlayer()
    {
        if (Time.time - startTime < timeBeforeStart)
            return;

        if (Time.time - lastBoost > boostCooldown)
        {
            lastBoost = Time.time;
            controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.Impulse);
        }
    }
}
