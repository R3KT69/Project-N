using PurrNet;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    private CharacterController controller;
    private Camera player_camera;
    public Transform character_model;
    public Animator player_anim;

    [Header("Player Misc")]
    [Range(1f, 10.0f)] public float movement_speed = 10;
    [Range(1f, 10.0f)] public float default_speed = 9;
    [Range(1f, 10.0f)] public float crouched_speed = 5;
    [Range(1f, 10.0f)] public float sprint_speed = 12;
    [Range(1f, 10.0f)] public float rotation_speed = 10f;
    [Range(1f, 10.0f)] public float jump_force = 10f;
    public bool isGrounded = false;
    public bool isCrouched = false;

    [Header("Physics setting")]
    public float gravity = -25f;
    public bool toggle_gravity = false;
    private float vertical_velocity;
    public float acceleration, horizontal;


    [Header("Ground Check Settings")]
    public Transform feetTransform;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;
    private static Collider[] groundHits = new Collider[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        player_camera = GetComponentInChildren<Camera>(true);

        if (character_model == null)
        {
            character_model = transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!isOwner) return;

        Vector3 input_vector = Vector3.zero;

        // --- input checks ---
        if (Input.GetKey(KeyCode.W)) input_vector += Vector3.forward; 
        if (Input.GetKey(KeyCode.A)) input_vector += Vector3.left; 
        if (Input.GetKey(KeyCode.S)) input_vector += Vector3.back; 
        if (Input.GetKey(KeyCode.D)) input_vector += Vector3.right; 

        bool isMoving = input_vector.sqrMagnitude > 0.01f;
        movement_speed = isMoving ? GetTargetMoveSpeed() : 0f;

        // --- CALCULATE ANIMATOR PARAMETERS ---

        // Forward/Backward movement (acceleration)
        float targetAcceleration = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) 
        { 
            float target = 1f;
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
            {
                target = 1.5f;
            }
            else if (isCrouched)
            {
                target = 1f;
            }

            targetAcceleration = target; 
        }
        acceleration = Mathf.MoveTowards(acceleration, targetAcceleration, 4f * Time.deltaTime);


        // Left/Right movement (horizontal)
        float targetHorizontal = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            float target = 1f;
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
            {
                target = 1.5f;
            }
            else if (isCrouched)
            {
                target = 1f;
            }

            
            targetHorizontal = target;
        }
        if (Input.GetKey(KeyCode.A))
        {
            float target = -1f;
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouched)
            {
                target = -1.5f;
            }
            else if (isCrouched)
            {
                target = -1f;
            }

            
            targetHorizontal = target;
        }




        horizontal = Mathf.MoveTowards(horizontal, targetHorizontal, 4f * Time.deltaTime);


        // --- Pass values to Animator ---
        player_anim.SetFloat("Acceleration", acceleration);
        player_anim.SetFloat("Horizontal", horizontal);

       

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouched)
        {
            vertical_velocity = jump_force;
            RpcTriggerJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouched = !isCrouched;
            player_anim.SetBool("isCrouched", isCrouched);
            RpcTriggerCrouch();
        }

        if (input_vector.sqrMagnitude > 0.01f)
        {
            input_vector.Normalize();

            Vector3 forward = player_camera != null ? player_camera.transform.forward : Vector3.forward;
            Vector3 right = player_camera != null ? player_camera.transform.right : Vector3.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            

            Vector3 movement_direction = right * input_vector.x + forward * input_vector.z;
            movement_direction.Normalize();

            
            Quaternion target_rotation = Quaternion.LookRotation(movement_direction, Vector3.up);
            character_model.rotation = Quaternion.Lerp(
                character_model.rotation,
                target_rotation,
                rotation_speed * Time.deltaTime);

            controller.Move(movement_direction * movement_speed * Time.deltaTime);
        }

        CheckGroundStatus();
        //Debug.DrawRay(origin, Vector3.down * raydist, Color.white);
        

        if (isGrounded && vertical_velocity < 0f)
        {
            vertical_velocity = -9.8f; // simulating gravity constantly pushing the player towards the ground
        }

        if (!isGrounded && toggle_gravity)
        {
            vertical_velocity += gravity * Time.deltaTime;
        }

        controller.Move(new Vector3(0f, vertical_velocity * Time.deltaTime, 0f));



        
    }

    [ObserversRpc]
    private void RpcTriggerJump()
    {
        player_anim.SetTrigger("Jump");
    }

    [ObserversRpc]
    private void RpcTriggerCrouch()
    {
        player_anim.SetTrigger("Crouch");
    }

    bool GroundCheck(Transform feetTransform, float sphereRadius, LayerMask groundMask)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(feetTransform.position, sphereRadius, groundHits, groundMask);
        return hitCount > 0;
    }

    void CheckGroundStatus()
    {
        isGrounded = GroundCheck(feetTransform, sphereRadius, groundMask);
        player_anim.SetBool("isGrounded", isGrounded);
    }
    
    void OnDrawGizmos()
    {
        if (feetTransform == null) return;
        bool grounded = GroundCheck(feetTransform, sphereRadius, groundMask);
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(feetTransform.position, sphereRadius);
    }

    private float GetTargetMoveSpeed()
    {
        if (isCrouched)
        {
            return crouched_speed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            return sprint_speed;
        }

        return default_speed;
    }

    
}
