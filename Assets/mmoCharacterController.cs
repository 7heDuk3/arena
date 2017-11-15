using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmoCharacterController : MonoBehaviour {

    public Transform playerCam, character, centerPoint;

    private float mouseX, mouseY;
    private bool mouseDownL = false;
    private bool mouseDownR = false;
    public float mouseSensitivity = 5f;
    public float mouseYPosition = 1f;

    private float moveFB, rotateLR;
    public float moveSpeed = 2f;
    public float moveBackMultiplier = 0.5f;

    private float zoom;
    public float zoomSpeed = 2;

    public float zoomMin = -2f;
    public float zoomMax = -10f;

    public float rotationSpeed = 5f;
    public float slerpSpeed = 2f;

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

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            mouseY = Mathf.Clamp(mouseY, -60f, 60f);

            centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

            Vector3 charMovement = new Vector3(0, 0, moveSpeed);
            charMovement = character.rotation * charMovement;
            character.GetComponent<CharacterController>().Move(charMovement * Time.deltaTime);

            centerPoint.localPosition = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

            character.localRotation = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
                mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                mouseDownL = true;
                mouseDownR = false;
            }
            else if (Input.GetMouseButton(1))
            {
                mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
                mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                mouseDownL = false;
                mouseDownR = true;
            }
            else
            {
                mouseX = centerPoint.eulerAngles.y;
                mouseY = centerPoint.eulerAngles.x;
                mouseDownL = false;
                mouseDownR = false;
            }

            mouseY = Mathf.Clamp(mouseY, -60f, 60f);
            
            if (mouseDownL || mouseDownR)
            {
                centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);
            }

            if (!mouseDownR)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    character.localRotation = Quaternion.Euler(0, character.eulerAngles.y - rotationSpeed, 0);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    character.localRotation = Quaternion.Euler(0, character.eulerAngles.y + rotationSpeed, 0);
                }
            }
            else
            {
                character.localRotation = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);
            }


            if (Input.GetKey(KeyCode.W))
            {
                Vector3 movement = new Vector3(0, 0, moveSpeed);
                Vector3 charMovement = character.rotation * movement;
                Vector3 centerPMovement = centerPoint.rotation * movement;

                character.GetComponent<CharacterController>().Move(charMovement * Time.deltaTime);
                centerPoint.localPosition = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

                
                if (!mouseDownL && !mouseDownR)
                {
                    centerPoint.rotation = Quaternion.Slerp(centerPoint.rotation, character.rotation, Time.deltaTime * rotationSpeed);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 movement = new Vector3(0, 0, -moveSpeed * moveBackMultiplier);
                movement = character.rotation * movement;

                character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
                centerPoint.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

                if (!mouseDownL && !mouseDownR)
                {
                    centerPoint.rotation = Quaternion.Slerp(centerPoint.rotation, character.rotation, Time.deltaTime * rotationSpeed);
                }
            }
        }
    }

    private void LateUpdate()
    {
        playerCam.transform.localPosition = new Vector3(0, 0, zoom);
        playerCam.LookAt(centerPoint);
    }
}
