using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmoCharacterController : MonoBehaviour {

    public Transform playerCam, character, centerPoint;

    private float mouseX, mouseY;
    public float mouseSensitivity = 2f;
    public float mouseYPosition = 1f;

    private float moveFB, rotateLR;
    public float moveSpeed = 2f;

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

        playerCam.transform.localPosition = new Vector3(0, 0, zoom);

        if (Input.GetMouseButton (0))
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY -= Input.GetAxis("Mouse Y");
        }

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        playerCam.LookAt(centerPoint);
        centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

        moveFB = Input.GetAxis("Vertical") * moveSpeed;
        rotateLR = Input.GetAxis("Horizontal") * moveSpeed;

        

        Vector3 movement = new Vector3(0, 0, moveFB);
        movement = character.rotation * movement;

        character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
        centerPoint.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);

        if (Input.GetKey(KeyCode.A))
        {
            character.rotation = Quaternion.Euler(0, character.eulerAngles.y - rotationSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            character.rotation = Quaternion.Euler(0, character.eulerAngles.y + rotationSpeed, 0);
        }

        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
        {
            //Quaternion turnAngle = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);

            //character.rotation = Quaternion.Slerp(centerPoint.localRotation, character.rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
