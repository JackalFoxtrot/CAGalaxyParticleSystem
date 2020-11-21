using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public CharacterController controller;

    public float speed = 12f;

    float xRotation = 0f;
    float yRotation = 0f;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InvertPausedBool();
            if(paused)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if(!paused)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation -= mouseX;
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
            playerBody.Rotate(Vector3.right * mouseY);

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            Vector3 velocity = new Vector3();

            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = speed;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocity.y = -speed;
            }

            controller.Move(velocity * Time.deltaTime);
        }
    }
    public void InvertPausedBool()
    {
        paused = !paused;
    }
}
