using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmoCharacterController : MonoBehaviour {

    public Transform playerCam, character, centerPoint;

    private float mouseX, mouseY;
    private bool mouseDown = false;
    public float mouseSensitivity = 2f;
    public float mouseYPosition = 1f;

    private float moveFB, rotateLR;
    public float moveSpeed = 2f;
    public float moveBackMultiplier = 0.5f;

    private float zoom;
    public float zoomSpeed = 2;

    public float zoomMin = -2f;
    public float zoomMax = -10f;

    public float rotationSpeed = 5f;

	// Use this for initialization
	void Start () {
        zoom = -5;
	}
	
	// Update is called once per frame
	void Update () {
        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom > zoomMin)
            zoom = zoomMin;

        if (zoom < zoomMax)
            zoom = zoomMax;

        //playerCam.transform.localPosition = new Vector3(0, 0, zoom);

        if (Input.GetMouseButton (0))
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY -= Input.GetAxis("Mouse Y");
            mouseDown = true;
        }
        else
        {
            mouseDown = false;
        }

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        //playerCam.LookAt(centerPoint);
        centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);


        if (Input.GetKey(KeyCode.A))
        {
            character.localRotation = Quaternion.Euler(0, character.eulerAngles.y - rotationSpeed, 0);
            //centerPoint.rotation = Quaternion.Euler(0, centerPoint.eulerAngles.y - rotationSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            character.localRotation = Quaternion.Euler(0, character.eulerAngles.y + rotationSpeed, 0);
            //centerPoint.rotation = Quaternion.Euler(0, character.eulerAngles.y, 0);
        }


        if (Input.GetKey(KeyCode.W))
        {
            Vector3 movement = new Vector3(0, 0, moveSpeed);
            Vector3 charMovement = character.rotation * movement;
            Vector3 centerPMovement = centerPoint.rotation * movement;

            character.GetComponent<CharacterController>().Move(charMovement * Time.deltaTime);
            centerPoint.localPosition = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

            //centerPoint.rotation = Quaternion.Euler(0, centerPoint.eulerAngles.y - rotationSpeed, 0);
            Quaternion turnAngle = Quaternion.Euler(0, character.eulerAngles.y, 0);
            if (!mouseDown)
            {
                //centerPoint.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);
                centerPoint.rotation = Quaternion.Slerp(centerPoint.rotation, character.rotation, Time.deltaTime * rotationSpeed);
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 movement = new Vector3(0, 0, -moveSpeed * moveBackMultiplier);
            movement = character.rotation * movement;

            character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
            centerPoint.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

            if (!mouseDown)
            {
                centerPoint.rotation = Quaternion.Slerp(centerPoint.rotation, character.rotation, /*Time.deltaTime **/ rotationSpeed);
            }
        }
    }

    private void LateUpdate()
    {
        playerCam.transform.localPosition = new Vector3(0, 0, zoom);
        playerCam.LookAt(centerPoint);
    }
}
