using PurrNet;
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
    public Animator animator;
    public WeaponType weaponType;
    public Rig_shifting rig_Shifting;
    public bool isHolding = false;
    public bool isAiming = false;
    public GameObject rifle_slot, pistol_slot, rifle_holster, pistol_holster;

    void Start()
    {
        if (rig_Shifting == null)
        {
            rig_Shifting = gameObject.GetComponent<Rig_shifting>();
        }
       
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
        if (!isAiming) rig_Shifting.DisableRig();
        
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



