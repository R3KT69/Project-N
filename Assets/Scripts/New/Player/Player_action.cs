using PurrNet;
using UnityEngine;

public class Player_action : NetworkBehaviour
{
    public Animator animator;
    private bool isAiming = false;

    private void Update()
    {
        if (!isOwner) return;
        if (animator == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            animator.SetBool("isAiming", isAiming);

            if (isAiming)
            {
                animator.SetTrigger("Rifle");
            }
        }
    }
}
