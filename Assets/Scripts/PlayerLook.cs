using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    [Header("Look Limits")]
    public float minXAngle = -85f; // Up limit
    public float maxXAngle = 85f;  // Down limit
    public float minYAngle = -60f; // Left limit
    public float maxYAngle = 60f;  // Right limit

    void Start()
    {
        // Lock the cursor to the center of the screen so it doesn't fly off the game window
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get raw mouse input data
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate rotations and clamp them so the player doesn't exceed the bounds
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXAngle, maxXAngle);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, minYAngle, maxYAngle);

        // Apply rotations directly to the camera transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
