//////////////////////////////////////////////////////
// Assignment/Lab/Project: Roll A Ball Game
//Name: John Hair
//Section: 2023SP.SGD.113.0001
//Instructor: George Cox
// Date: 04/23/2023
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float rotationSpeed = 5.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Confine the cursor within the game window
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Update the current camera rotation based on mouse movement
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // Clamp the vertical rotation to prevent the camera from flipping upside down
            currentY = Mathf.Clamp(currentY, -45, 45);
        }
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        // Calculate the direction and distance of the camera relative to the player
        Vector3 direction = new Vector3(0, height, -distance);

        // Create a rotation based on the current X and Y values
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Calculate the camera's position based on the player's position and the rotation
        transform.position = player.transform.position + rotation * direction;

        // Make the camera look at the player's position
        transform.LookAt(player.transform.position);
    }
}
