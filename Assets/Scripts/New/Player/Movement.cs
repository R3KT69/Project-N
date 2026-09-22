using PurrNet;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    private CharacterController controller;
    private Camera player_camera;
    public Transform character_model;
    public Animator player_anim;
    public float movement_speed = 10;
    public float rotation_speed = 10f;
    public float jump_force = 10f;
    public float gravity = -25f;
    public bool toggle_gravity = false;
    public bool isGrounded = false;
    private float vertical_velocity;

    float acceleration, horizontal;


    [Header("Ground Check Settings")]
    public Transform feetTransform;
    public float sphereRadius = 0.3f;
    public LayerMask groundMask;
    private static Collider[] groundHits = new Collider[10];
    

    public float raydist = 0.1f;
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

        // --- CALCULATE ANIMATOR PARAMETERS ---
        // 1. Acceleration should ONLY trigger for Forward/Backward (W and S)
        float targetAcceleration = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) 
        { 
            targetAcceleration = 1f; 
        }
        acceleration = Mathf.MoveTowards(acceleration, targetAcceleration, 4f * Time.deltaTime);
        // 2. Horizontal should ONLY trigger for Left/Right (A and D)
        float targetHorizontal = 0f;
        if (Input.GetKey(KeyCode.D)) targetHorizontal = 1f;
        if (Input.GetKey(KeyCode.A)) targetHorizontal = -1f;
        horizontal = Mathf.MoveTowards(horizontal, targetHorizontal, 4f * Time.deltaTime);

        // --- Pass values to Animator ---
        player_anim.SetFloat("Acceleration", acceleration);
        player_anim.SetFloat("Horizontal", horizontal);

       

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            vertical_velocity = jump_force;
            RpcTriggerJump();
        }

        if (input_vector.sqrMagnitude > 0f)
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

    
}
