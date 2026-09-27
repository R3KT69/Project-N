using System.Collections.Generic;
using PurrNet;
using UnityEngine;

public class Player_inventory : NetworkBehaviour
{
    public List<Item> Hotbar;
    public Item selectedItem;
    public Player_action player_Action;
    
    private bool isSelected = false;

    void Awake()
    {
        selectedItem = Hotbar[0];
    }

    void Update()
    {
        if (!isOwner) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RpcSelectedItem(0);
            SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RpcSelectedItem(1);
            SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RpcSelectedItem(2);
            SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RpcSelectedItem(3);
            SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RpcSelectedItem(4);
            SelectItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RpcSelectedItem(5);
            SelectItem();
        }
    }

    private void SelectItem()
    {
        if (selectedItem.itemCategory == ItemCategory.OneHanded)
        {
            player_Action.trigger_equip_onehanded();
        } else if (selectedItem.itemCategory == ItemCategory.TwoHanded)
        {
            player_Action.trigger_equip_twohanded();
        }
    }

    [ObserversRpc]
    private void RpcSelectedItem(int slot)
    {
        selectedItem = Hotbar[slot];
    }


}
