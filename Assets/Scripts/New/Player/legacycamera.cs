using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Orbit Settings")]
    public float distance = 5f;
    public float mouseSensitivity = 2f;
    public float minYAngle = -30f;
    public float maxYAngle = 75f;

    [Header("Camera Offsets")]
    public Vector3 baseOffset = new Vector3(0f, 1.5f, 0f); // Camera position 
    public Vector3 orbitOffset = new Vector3(0f, 0f, 0f);  // Free-look offset
    public Vector3 aimingOffset = new Vector3(0.5f, 0f, 1f);   // Aiming offset
    private bool isOffsetFlipped = false;

    [Header("Transition Settings")]
    public float bodyRotationSpeed = 10f;
    public float aimingShoulderSnapSpeed = 50f;
    public float offsetTransitionSpeed = 5f;

    private float xAngle = 0f;
    private float yAngle = 15f;

    private Transform playerTransform;
    
    private Vector3 currentShoulderOffset = Vector3.zero;
    private float aimBlend = 0f; 

    void Start()
    {
        playerTransform = transform.parent;
        

        if (playerTransform == null)
        {
            enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xAngle += mouseX;
        yAngle -= mouseY;
        yAngle = Mathf.Clamp(yAngle, minYAngle, maxYAngle);

        // Shoulder Snapping
        

        Quaternion camRotation = Quaternion.Euler(yAngle, xAngle, 0f);

    

        // Smooth blend factor for aim transition
        

        // Compute world-space offsets
        

        

        Vector3 camPosition = playerTransform.position + baseOffset + currentShoulderOffset - camRotation * Vector3.forward * distance;

        transform.position = camPosition;
        transform.rotation = camRotation;
    }
}
