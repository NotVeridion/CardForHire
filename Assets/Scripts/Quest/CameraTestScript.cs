using UnityEngine;

public class CameraMovement2D : MonoBehaviour
{
    public float speed = 5f; // Adjust the speed in the Unity Inspector

    // Update is called once per frame
    void Update()
    {
        // Get input from the Horizontal and Vertical axes (WASD and Arrow Keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a movement vector in the X and Y plane (2D movement)
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

        // Normalize the movement vector to ensure consistent speed when moving diagonally
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Move the camera's position
        transform.Translate(movement * speed * Time.deltaTime);
    }
}