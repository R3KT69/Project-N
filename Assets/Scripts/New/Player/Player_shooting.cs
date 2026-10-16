using PurrNet;
using UnityEngine;

public class Player_shooting : NetworkBehaviour
{
    public Player_action player_Action;
    public Player_camera player_Camera;
    public Player_inventory player_Inventory;


    void Start()
    {
        player_Action = gameObject.GetComponent<Player_action>();
        player_Inventory = gameObject.GetComponent<Player_inventory>();
        player_Camera = gameObject.GetComponent<Player_camera>();
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
                if (Input.GetMouseButtonDown(0))
                {
                    InitiateShootingAuto(selected_weapon);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    selected_weapon.StopAutomaticFire();
                }

                return;
            }

            if (selected_weapon.weaponType != WeaponClass.Automatic)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    InitiateShooting(selected_weapon);
                    
                    //Instantiate()
                }
                //Instantiate()
            }

            
        }
    }

    [ObserversRpc]
    public void InitiateShooting(Weapon selected_weapon)
    {
        Debug.Log("Shooting.");
        Debug.Log($"got weapon: {selected_weapon.name}");

        //selected_weapon.PlaySFX();
        //selected_weapon.PlayVFX();

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

        //selected_weapon.PlaySFX();
        //selected_weapon.PlayVFX();

        if (isOwner)
        {
            selected_weapon.AutomaticFire(selected_weapon.firerate);
        }
        
    }
}
