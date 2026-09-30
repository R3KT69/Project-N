using PurrNet;
using UnityEditor;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle
}

public class Player_action : NetworkBehaviour
{
    public Player_movement player_Movement;
    public Player_inventory player_Inventory;
    public Player_camera playerCamera;
    public Animator animator;
    public WeaponType weaponType;
    public Rig_shifting rig_Shifting;
    public bool isHolding = false;
    public bool isAiming = false;
    public GameObject rifle_slot, pistol_slot, rifle_holster, pistol_holster;
    private Color selected_color = Color.red;
    private string lookAtObj;

    void Start()
    {
        if (rig_Shifting == null)
        {
            rig_Shifting = gameObject.GetComponent<Rig_shifting>();
        }
       
    }

    void OnGUI()
    {
        if (!isOwner) return;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 9;
        style.normal.textColor = selected_color;
        

        GUI.Label(
            new Rect(0, Screen.height / 2f - 15f + 15f, Screen.width, 30f),
            $"looking_at: {lookAtObj}",
            style
        );

    }


    private void Update()
    {
        if (!player_Movement.isInEditor)
        {
            if (!isOwner) return;
        }

        if (animator == null) return;
        

        handle_equip_twohanded();
        handle_equip_onehanded();
        handle_unequip();
        drop_item();


        
        Debug.DrawRay(playerCamera.p_camera.transform.position, playerCamera.p_camera.transform.forward * 10f, selected_color);
        selected_color = Color.red;
        lookAtObj = "None";
        if (Physics.Raycast(playerCamera.p_camera.transform.position, playerCamera.p_camera.transform.forward , out RaycastHit hit, 10f))
        {
            if (hit.collider.CompareTag("Weapon"))
            {
                Debug.Log("Hit: " + hit.collider.name);
                lookAtObj = hit.collider.name;
                selected_color = Color.green;
            }
        }


        if (!isAiming) rig_Shifting.DisableRig();
        
    }


    public void drop_item()
    {
        if (player_Inventory.selectedItem.itemCategory == ItemCategory.None) return;
        if (isHolding || isAiming) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject selected_item_old = player_Inventory.selectedItem.gameObject;
            

            Destroy(player_Inventory.selectedItem.gameObject);
            player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex] = player_Inventory.empty_hand.GetComponent<Item>();
            player_Inventory.selectedItem = player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex];

            GameObject dropped_item = Instantiate(selected_item_old, player_Inventory.throw_item.position, player_Inventory.throw_item.rotation);
            
            dropped_item.AddComponent<Rigidbody>();
            Rigidbody rb = dropped_item.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            
            //dropped_item.GetComponent<Rigidbody>().AddForce(Vector3.forward * 15f);
            
            
        }
        
    }

    

    public void handle_unequip()
    {
        if (isHolding)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isAiming = !isAiming;
                animator.SetBool("isAiming", isAiming);

                if (isAiming)
                {
                    if (weaponType == WeaponType.Rifle)
                    {
                        RpcTriggerAim_RifleAim();
                        rig_Shifting.EnableRig();
                    } else if (weaponType == WeaponType.Pistol)
                    {
                        RpcTriggerAim_PistolAim();
                        rig_Shifting.EnableRig();
                    }
                    
                }
            }
        }
    }

    public void handle_equip_twohanded()
    {
        if (player_Inventory.selectedItem.itemCategory == ItemCategory.None) return;
        
        if (Input.GetKeyDown(KeyCode.R) && player_Inventory.selectedItem.itemCategory == ItemCategory.TwoHanded)
        {
            if (isAiming) return;

            
            RpcInvTransferTransform(rifle_slot.transform);
            weaponType = WeaponType.Rifle;
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                RpcTriggerAim_Rifle();
            } else
            {
                RpcInvTransferTransform(rifle_holster.transform);
            }
        }
    }

    public void handle_equip_onehanded()
    {
        if (player_Inventory.selectedItem.itemCategory == ItemCategory.None) return;
        
        if (Input.GetKeyDown(KeyCode.R) && player_Inventory.selectedItem.itemCategory == ItemCategory.OneHanded)
        {
            if (isAiming) return;

            RpcInvTransferTransform(pistol_slot.transform);
            weaponType = WeaponType.Pistol;
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                RpcTriggerAim_Pistol();
            } else
            {
                RpcInvTransferTransform(pistol_holster.transform);
            }
        }
    }

    [ObserversRpc]
    private void RpcInvTransferTransform(Transform transform)
    {
        
        player_Inventory.selectedItem?.transform.SetParent(transform, false);
    }

    [ObserversRpc]
    private void RpcTriggerAim_Rifle()
    {
        animator.SetTrigger("Rifle");
    }

    [ObserversRpc]
    private void RpcTriggerAim_Pistol()
    {
        animator.SetTrigger("Pistol");
    }

    [ObserversRpc]
    private void RpcTriggerAim_RifleAim()
    {
        animator.SetTrigger("RifleAim");
    }

    [ObserversRpc]
    private void RpcTriggerAim_PistolAim()
    {
        animator.SetTrigger("PistolAim");
    }


}



