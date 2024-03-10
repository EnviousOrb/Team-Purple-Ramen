using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int topClamp, botClamp;
    float xrot;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float mousey = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mousex = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        xrot -= mousey;
        xrot = Mathf.Clamp(xrot, botClamp, topClamp);
        transform.localRotation = Quaternion.Euler(xrot, 0, 0);
        transform.parent.Rotate(Vector3.up * mousex);
    }
}
