using PurrNet;
using UnityEngine;

public class Player_action : NetworkBehaviour
{
    public Animator animator;
    private bool isHolding = false;
    private bool isAiming = false;


    private void Update()
    {
        if (!isOwner) return;
        if (animator == null) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            isHolding = !isHolding;
            animator.SetBool("isHolding", isHolding);

            if (isHolding)
            {
                animator.SetTrigger("Rifle");
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
                    animator.SetTrigger("RifleAim");
                }
            }
        }
    }
}
