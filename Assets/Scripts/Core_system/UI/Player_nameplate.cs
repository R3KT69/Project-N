using PurrNet;
using TMPro;
using UnityEngine;

public class Player_nameplate : NetworkBehaviour
{
    private TextMeshPro textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();

        // Hide our own nameplate
        if (isOwner)
        {
            if (textMesh != null)
                textMesh.enabled = false;
            
            enabled = false; 
            return;
        }
    }

    void LateUpdate()
    {
        // Find the active camera
        Camera activeCam = GetActiveCamera();
        if (activeCam == null) return;

        // face the camera
        transform.LookAt(activeCam.transform);
        transform.Rotate(0, 180, 0);
    }

    private Camera GetActiveCamera()
    {
        // Scan all cameras in the scene
        Camera[] cams = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (var cam in cams)
        {
            if (cam.isActiveAndEnabled)
            {
                return cam;
            }
        }
        return null;
    }
}