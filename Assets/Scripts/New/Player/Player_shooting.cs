using PurrNet;
using UnityEngine;

public class Player_shooting : NetworkBehaviour
{
    public Player_action player_Action;
    public Player_inventory player_Inventory;


    void Start()
    {
        player_Action = gameObject.GetComponent<Player_action>();
        player_Inventory = gameObject.GetComponent<Player_inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isOwner) return;

        if (player_Action.isAiming)
        {
            Weapon selected_weapon = player_Inventory.selectedItem.GetComponent<Weapon>();
            
            if (selected_weapon.weaponType == WeaponClass.Automatic)
            {
                if (Input.GetMouseButton(0))
                {
                    InitiateShootingAuto(selected_weapon);
                } else
                {
                    selected_weapon.StopAutomaticFire();
                }

                return;
                //Instantiate()
            }

            if (Input.GetMouseButtonDown(0))
            {
                InitiateShooting(selected_weapon);
                
                //Instantiate()
            }
        }
    }

    [ObserversRpc]
    public void InitiateShooting(Weapon selected_weapon)
    {
        Debug.Log("Shooting.");
        Debug.Log($"got weapon: {selected_weapon.name}");

        selected_weapon.PlaySFX();
        selected_weapon.PlayVFX();

        if (isOwner)
        {
            if (selected_weapon.weaponType == WeaponClass.SemiAutomatic)
            {
                selected_weapon.RifleFire();
            }
            
            else if (selected_weapon.weaponType == WeaponClass.Shotgun)
            {
                selected_weapon.ShotgunFire(6, 15, 50f);
            }
            
        }
        
    }

    [ObserversRpc]
    public void InitiateShootingAuto(Weapon selected_weapon)
    {
        Debug.Log("Shooting.");
        Debug.Log($"got weapon: {selected_weapon.name}");

        selected_weapon.PlaySFX();
        selected_weapon.PlayVFX();

        if (isOwner)
        {
            selected_weapon.AutomaticFire(5);
            
            
        }
        
    }
}
