using PurrNet;
using Unity.Mathematics;
using UnityEngine;

public class Player_camera : NetworkBehaviour
{
    public Player_movement player_Movement;
    public Player_action player_action;
    public GameObject playerCamRotation;
    public Camera p_camera;
    public Transform target;
    public GameObject aim_sphere;

    [Header("Normal Camera")]
    public float distance = 6f;
    public float height = 2f;
    public float offset = 0.5f;
    public Vector3 p_camera_default;

    [Header("Mouse")]
    public float mouse_sensitivity = 3f;
    public float minimum_pitch = -30f;
    public float maximum_pitch = 70f;

    [Header("Aim Camera")]
    public Vector3 aim_offset = new Vector3(0.6f, 1.6f, -0.2f);
    

    

    private bool p_camera_cached = false;
    private float yaw;
    private float pitch = 15f;
    [SerializeField] private float aim_transition_speed = 1f;
    private Vector3 aim_position;

    void OnGUI()
    {
        if (!isOwner) return;
        
        float fps = 1f / Time.deltaTime;
        GUI.Label(new Rect(20, 20, 200, 30), "FPS: " + fps.ToString("F0"));
    }

    void Start()
    {
        if (target == null)
        {
            target = transform.root;
        }

        if (p_camera == null)
        {
            p_camera = GetComponentInChildren<Camera>(true);
        }

        yaw = target.eulerAngles.y;
    }

    [ObserversRpc]
    private void SetCamRotationRPC(Quaternion rot)
    {
        if (isOwner) return;
        

        p_camera.transform.rotation = rot;
    }

    void Update()
    {
        // Debug.Log($"p_camera transform:{p_camera.transform.position}\np_camera_default transform:{p_camera_default}");
    }

    void LateUpdate()
    {
        

        if (!player_Movement.isInEditor)
        {
            if (!isOwner || p_camera == null || target == null)
            {
                if (p_camera != null)
                {
                    p_camera.gameObject.GetComponent<Camera>().enabled = false;
                    p_camera.gameObject.GetComponent<AudioListener>().enabled = false;
                }

                return;
            }
        }


        // =========================
        // MOUSE INPUT
        // =========================

        yaw += Input.GetAxis("Mouse X") * mouse_sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouse_sensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minimum_pitch,
            maximum_pitch
        );


        // =========================
        // CURSOR
        // =========================

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        // =========================
        // AIMING CAMERA
        // =========================

        if (player_action.isAiming)
        {
            // Shoulder position
            /*Vector3 aim_position =
                target.position +
                yaw_rotation * aim_offset;*/

            //Vector3 aim_position = aim_sphere.transform.position;


            if (p_camera_cached == false)
            {
                p_camera_default = p_camera.transform.position;
                p_camera_cached = true;
            }
            
           

            aim_position = Vector3.Lerp(
                p_camera.transform.position,
                aim_sphere.transform.position,
                Time.deltaTime * aim_transition_speed
            );
            
            // Camera rotation
            Quaternion camera_rotation = Quaternion.Euler(pitch, yaw, 0f);

            // Set both at once
            p_camera.transform.SetPositionAndRotation(aim_position, camera_rotation);

            SetCamRotationRPC(camera_rotation);

            // =========================
            // ROTATE CHARACTER
            // =========================

            Vector3 lookDirection = p_camera.transform.forward;

            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion target_rotation =
                    Quaternion.LookRotation(
                        lookDirection,
                        Vector3.up
                    );

                player_Movement.character_model.rotation =
                    Quaternion.Lerp(
                        player_Movement.character_model.rotation,
                        target_rotation,
                        player_Movement.rotation_speed *
                        Time.deltaTime
                    );
            }


            return;
        }
        

        // =========================
        // NORMAL ORBIT CAMERA
        // =========================

        
        Vector3 target_position = target.position + Vector3.up * height;

        Quaternion orbit_rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 camera_position = target_position
            + orbit_rotation * Vector3.right * offset
            + orbit_rotation * Vector3.back * distance;

        Quaternion normal_camera_rotation = orbit_rotation;

        camera_position = Vector3.Lerp(
            p_camera.transform.position,
            camera_position,
            Time.deltaTime * aim_transition_speed * 0.5f
        );

        p_camera.transform.SetPositionAndRotation(
            camera_position,
            normal_camera_rotation
        );
    }
}