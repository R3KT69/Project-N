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
    public Animator animator;
    public WeaponType weaponType;
    public Rig_shifting rig_Shifting;
    private bool isHolding = false;
    private bool isAiming = false;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponType = WeaponType.Rifle;
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                animator.SetTrigger("Rifle");
                rig_Shifting.EnableRig();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            weaponType = WeaponType.Pistol;
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                animator.SetTrigger("Pistol");
                rig_Shifting.EnableRig();
            }
        }

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
                        animator.SetTrigger("RifleAim");
                    } else if (weaponType == WeaponType.Pistol)
                    {
                        animator.SetTrigger("PistolAim");
                    }
                    
                }
            }
        }

        if (!isHolding)
        {
            rig_Shifting.DisableRig();
        }
    }
}
