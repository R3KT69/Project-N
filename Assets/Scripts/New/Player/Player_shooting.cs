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
            if (Input.GetMouseButtonDown(0))
            {
                InitiateShooting(selected_weapon);

            }
        }
    }

    [ObserversRpc]
    public void InitiateShooting(Weapon selected_weapon)
    {
        Debug.Log("Shooting.");
        Debug.Log($"got weapon: {selected_weapon.name}");
        selected_weapon.muzzleFlash.Play();
    }
}
