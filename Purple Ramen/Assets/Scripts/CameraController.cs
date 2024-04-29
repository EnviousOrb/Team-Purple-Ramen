using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the camera movement based on mouse input.
public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity; // How sensitive the camera movement is to mouse movement.
    [SerializeField] int topClamp, botClamp; // Limits for the camera's vertical rotation to prevent it from going too far up or down.
    int originalSens;
    float xrot; // Current vertical rotation of the camera.

    // Start is called before the first frame update
    void Start()
    {
        // Locks the cursor to the center of the screen and hides it for an immersive FPS experience.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalSens = sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.playerDead)
            sensitivity = 0;
        else
            sensitivity = originalSens;
        // Gets vertical mouse movement, adjusts it by time and sensitivity.
        float mousey = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        // Gets horizontal mouse movement, adjusts it by time and sensitivity.
        float mousex = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        // Adjusts the camera's vertical rotation based on mouse input.
        xrot -= mousey;
        // Clamps the vertical rotation to stay within specified limits.
        xrot = Mathf.Clamp(xrot, botClamp, topClamp);

        // Applies the calculated vertical rotation to the camera.
        transform.localRotation = Quaternion.Euler(xrot, 0, 0);
        // Rotates the parent object (likely the player) based on horizontal mouse movement, allowing for looking around.
        transform.parent.Rotate(Vector3.up * mousex);
    }
}

