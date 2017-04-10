using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;
    private Transform lookAtStart;
    public GameObject player;

    public RectTransform VirtualJoystickSpace;
    public RectTransform buttonLetsStartSpace;

    private Vector3 offset;
    private Vector3 desiredPosition;

    private Vector2 touchPosition;
    private float swipeResistance = 300.0f;

    private float distance = 4.5f;
    private float yOffset = 2.0f;
    private float smoothSpeed = 7.5f;

    private float startTime = 0;
    private const float timeBeforeStart = 2.0f;

    private bool playerIsDead = false;
    private bool isInsideSpace = false;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, yOffset, -1 * distance);

        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time - startTime < timeBeforeStart)
        {
            lookAtStart = lookAt;
            return;
        }

        if (GameManager.Instance.IsPlayed != 0)
        {
            if (player.transform.position.y < -5.0f)
                playerIsDead = true;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                slideCamera(true);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                slideCamera(false);

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(VirtualJoystickSpace, Input.mousePosition) || RectTransformUtility.RectangleContainsScreenPoint(buttonLetsStartSpace, Input.mousePosition))
                    isInsideSpace = true;
                else
                    touchPosition = Input.mousePosition;
                
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {

                if (isInsideSpace)
                {
                    isInsideSpace = false;
                    return;
                }

                float swipeForce = touchPosition.x - Input.mousePosition.x;
                if (Mathf.Abs(swipeForce) > swipeResistance)
                {
                    if (swipeForce < 0)
                        slideCamera(true);
                    else
                        slideCamera(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - startTime < timeBeforeStart)
        {
            //lookAtStart = lookAt;
            return;
        }

        if (playerIsDead)
        {
            offset = new Vector3(0, yOffset, -1 * distance);

            desiredPosition = lookAtStart.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(lookAt.position + Vector3.up);

            playerIsDead = false;
        }
        else
        {
            desiredPosition = lookAt.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.LookAt(lookAt.position + Vector3.up);
        }
    }

    public void slideCamera(bool left)
    {
            if (left)
                offset = Quaternion.Euler(0, 90, 0) * offset;
            else
                offset = Quaternion.Euler(0, -90, 0) * offset;
    }
}
