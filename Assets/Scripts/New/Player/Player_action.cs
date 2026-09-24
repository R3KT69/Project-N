using PurrNet;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle
}

public class Player_action : NetworkBehaviour
{
    public Animator animator;
    public WeaponType weaponType;
    private bool isHolding = false;
    private bool isAiming = false;


    private void Update()
    {
        if (!isOwner) return;
        if (animator == null) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponType = WeaponType.Rifle;
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                animator.SetTrigger("Rifle");
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
    }
}
