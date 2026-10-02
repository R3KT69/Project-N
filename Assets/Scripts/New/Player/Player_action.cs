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
    private bool isLooking = false;
    private string lookAt;
    public GameObject lookAtObj;

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
            new Rect(0, Screen.height / 2f - 15f + 10f, Screen.width, 30f),
            $"[{lookAt}]",
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
        //drop_item();
        drop_item_exp();
        pickup_item_exp();
        //pickup_item();


        
        Debug.DrawRay(playerCamera.p_camera.transform.position, playerCamera.p_camera.transform.forward * 5f, selected_color);
        selected_color = Color.red;
        lookAt = "";
        isLooking = false;
        if (Physics.Raycast(playerCamera.p_camera.transform.position, playerCamera.p_camera.transform.forward , out RaycastHit hit, 5f))
        {
            if (hit.collider.CompareTag("Weapon") && !hit.collider.gameObject.GetComponent<Item>().isEquipped)
            {
                isLooking = true;
                Debug.Log("Hit: " + hit.collider.name);
                lookAtObj = hit.collider.gameObject;
                lookAt = lookAtObj.GetComponent<Item>().item_name;
                selected_color = Color.green;
            }
        }


        if (!isAiming) rig_Shifting.DisableRig();
        
    }

    public void drop_item_exp()
    {
        if (player_Inventory.selectedItem.itemCategory == ItemCategory.None) return;
        if (isHolding || isAiming) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject selected_item_old = player_Inventory.selectedItem.gameObject;

            //int item_order = selected_item_old.GetComponent<Item>().item_order;
            //int ammo = selected_item_old.GetComponent<Weapon>().Ammo;

            player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex] =
                player_Inventory.empty_hand.GetComponent<Item>();

            player_Inventory.selectedItem =
                player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex];


            SetEmptyHotbar(player_Inventory.selectedHotbarIndex);
            DetatchWeapon(selected_item_old);

            selected_item_old.AddComponent<Rigidbody>();

            Rigidbody rb = selected_item_old.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            //rb.centerOfMass = Vector3.zero;
            //rb.AddForce(transform.forward * 2f, ForceMode.Impulse);

            //selected_item_old.GetComponent<Item>().isEquipped = false;
            //AdjustAmmoAndEquipState(ammo, false, dropped_item);

            
        }
    }


    // need to remove rigidbody when its picked up..

    public void pickup_item_exp()
    {
        if (isHolding || isAiming || !isLooking) return;

        Debug.Log("Trying to pickup");

        

        if (Input.GetKeyDown(KeyCode.F))
        {
            int free_index = player_Inventory.GetEmptyHandIndex();
            string flag = "none";

            if (lookAtObj.GetComponent<Item>().itemCategory == ItemCategory.OneHanded)
            {
                flag = "Pistol";
            } else if (lookAtObj.GetComponent<Item>().itemCategory == ItemCategory.TwoHanded)
            {
                flag = "Rifle";
            }

            AttachWeapon(lookAtObj, flag);

            SetItemHotbar(lookAtObj.GetComponent<Item>(), free_index);
            
            lookAtObj.GetComponent<NetworkIdentity>().GiveOwnership(localPlayer);
            lookAtObj.GetComponent<Item>().isEquipped = true;

            Debug.Log($"Success. Given item {lookAtObj.name} to playerid: {localPlayer}");
        }
    }

    [ObserversRpc]
    public void SetEmptyHotbar(int index)
    {
        player_Inventory.selectedItem = player_Inventory.empty_hand.GetComponent<Item>();
        player_Inventory.selectedHotbarIndex = index;
        player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex] = player_Inventory.selectedItem;
    }

    [ObserversRpc]
    private void DetatchWeapon(GameObject selected_item)
    {
        selected_item.transform.SetParent(null);
        selected_item.transform.SetPositionAndRotation(
            player_Inventory.throw_item.position,
            player_Inventory.throw_item.rotation
        );

        selected_item.GetComponent<Item>().isEquipped = false;
    }

    [ObserversRpc]
    private void AttachWeapon(GameObject selected_item, string flag)
    {
        if (flag == "Pistol")
        {
            selected_item.transform.SetParent(pistol_holster.transform);
            selected_item.transform.SetPositionAndRotation(
            pistol_holster.transform.position,
            pistol_holster.transform.rotation
            );
        } else if (flag == "Rifle")
        {
            selected_item.transform.SetParent(rifle_holster.transform);
            selected_item.transform.SetPositionAndRotation(
            rifle_holster.transform.position,
            rifle_holster.transform.rotation
            );
        }

        Destroy(selected_item.GetComponent<Rigidbody>());
         
    
    }

    [ObserversRpc]
    public void SetItemHotbar(Item item, int index)
    {
        player_Inventory.selectedItem = item;
        player_Inventory.selectedHotbarIndex = index;
        player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex] = player_Inventory.selectedItem;
    }

    // drop_item and pickup_item below only works for offline instances. Dont use them

    /*
    public void drop_item()
    {
        if (player_Inventory.selectedItem.itemCategory == ItemCategory.None) return;
        if (isHolding || isAiming) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject selected_item_old = player_Inventory.selectedItem.gameObject;

            int item_order = selected_item_old.GetComponent<Item>().item_order;
            int ammo = selected_item_old.GetComponent<Weapon>().Ammo;


            player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex] =
                player_Inventory.empty_hand.GetComponent<Item>();

            player_Inventory.selectedItem =
                player_Inventory.Hotbar[player_Inventory.selectedHotbarIndex];

            GameObject dropped_item = Instantiate(
                ItemDatabase.instance.weapons[item_order],
                player_Inventory.throw_item.position,
                player_Inventory.throw_item.rotation
            );

            dropped_item.AddComponent<Rigidbody>();

            Rigidbody rb = dropped_item.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.centerOfMass = Vector3.zero;
            rb.AddForce(transform.forward * 2f, ForceMode.Impulse);


            
            //AdjustAmmoAndEquipState(ammo, false, dropped_item);

            Destroy(selected_item_old);
        }
    }*/

    /*
    [ObserversRpc]
    public void AdjustAmmoAndEquipState(int ammo, bool state, GameObject dropped_item)
    {
        Debug.Log($"Ammo to transfer: {ammo}");
        dropped_item.GetComponent<Weapon>().Ammo = ammo;
        dropped_item.GetComponent<Item>().isEquipped = state;
    }*/

    /*
    public void pickup_item()
    {
        if (isHolding || isAiming || !isLooking) return;

        Debug.Log("Trying to pickup");

        

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Success");
            Transform holster_position = null;

            if (lookAtObj.GetComponent<Item>().itemCategory == ItemCategory.OneHanded)
            {
                holster_position = pistol_holster.transform;
            } else if (lookAtObj.GetComponent<Item>().itemCategory == ItemCategory.TwoHanded)
            {
                holster_position = rifle_holster.transform;
            }

            GameObject new_item = Instantiate(
            ItemDatabase.instance.weapons[lookAtObj.GetComponent<Item>().item_order],
            holster_position.transform.position,
            holster_position.transform.rotation,
            holster_position.transform
            );

            int old_ammo = lookAtObj.GetComponent<Weapon>().Ammo;

            new_item.GetComponent<Weapon>().TransferData(old_ammo);
            
           int emptyHandIndex = player_Inventory.GetEmptyHandIndex();


            //RpcPickupItem(new_item, emptyHandIndex);
            Destroy(lookAtObj);
        }
    }*/

    /*
    [ObserversRpc]
    private void RpcPickupItem(GameObject new_item, int slot)
    {
        Debug.Log(
            $"PICKUP RPC | player={gameObject.name} | " +
            $"new_item={new_item} | slot={slot}"
        );

        if (new_item == null)
        {
            Debug.LogError("new_item is NULL on " + gameObject.name);
            return;
        }

        if (player_Inventory == null)
        {
            Debug.LogError("player_Inventory is NULL on " + gameObject.name);
            return;
        }

        Item item = new_item.GetComponent<Item>();

        if (item == null)
        {
            Debug.LogError("Item component is NULL on " + gameObject.name);
            return;
        }

        player_Inventory.Hotbar[slot] = item;
        player_Inventory.selectedItem = item;
        player_Inventory.selectedHotbarIndex = slot;
        item.isEquipped = true;
    }
    */

    // drop_item and pickup_item above only works for offline instances. Dont use them

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
        if (player_Inventory.selectedItem == null)
            return;

        player_Inventory.selectedItem.transform.SetParent(transform, false);
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



