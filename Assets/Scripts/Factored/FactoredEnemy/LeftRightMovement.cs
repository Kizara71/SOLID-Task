using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    
    [Header("Room Boundaries")]
    [SerializeField] private float leftBound = -8f;
    [SerializeField] private float rightBound = 8f;

    private int direction = 1;

    private void Update()
    {
        // Move the object
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Check if we hit the right wall
        if (transform.position.x >= rightBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
            direction = -1; // Flip left
        }
        // Check if we hit the left wall
        else if (transform.position.x <= leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
            direction = 1; // Flip right
        }
    }
}
