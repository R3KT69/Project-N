using System;
using System.Collections.Generic;
using PurrNet;
using UnityEngine;

public class Player_inventory : NetworkBehaviour
{
    public List<Item> Hotbar;
    public Item selectedItem;
    public int selectedHotbarIndex;
    public Player_action player_Action;
    public GameObject empty_hand;
    public Transform throw_item;
    

    void OnGUI()
    {
        if (!isOwner) return;
        
        GUI.Label(new Rect(20, 40, 300, 30), $"selected_Item: {selectedItem?.item_name} | Ammo: {selectedItem?.GetComponent<Weapon>()?.Ammo}");

        for (int i = 0; i < 6; i++)
        {
            GUI.Label(new Rect(20, 40 + i*20 + 275, 300, 30), $"hotbar[{i}]: {Hotbar[i].item_name}");
        }
        
    }

    void Awake()
    {
        selectedItem = Hotbar[0];
        //selectedItem.isEquipped = true;
    }

    void Update()
    {
        if (selectedItem == null)
        {
            Debug.Log($"Selected item got null on {gameObject.name}");
            selectedItem = empty_hand.GetComponent<Item>();
        }

        if (!isOwner) return;

        

        if (!player_Action.isHolding)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RpcSelectedItem(0);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                RpcSelectedItem(1);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                RpcSelectedItem(2);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                RpcSelectedItem(3);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                RpcSelectedItem(4);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                RpcSelectedItem(5);
                //SelectItem();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (selectedItem != null)
                {
                    if (selectedItem.GetComponent<Weapon>() != null)
                    {
                        Weapon weapon = selectedItem.GetComponent<Weapon>();
                        weapon.TransferData(weapon.Ammo-1);
                    }
                    
                }
                
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log($"Empty hotbar: {GetEmptyHandIndex()}");
            }

            
        }

        

        
    }

    public int GetEmptyHandIndex()
    {
        for (int i = 0; i < Hotbar.Count; i++)
        {
            if (Hotbar[i].itemCategory == ItemCategory.None)
            {
                return i;
            }
        }

        return -1;
    }

    [ObserversRpc]
    public void SelectEmptyHand()
    {
        selectedItem = empty_hand.GetComponent<Item>();
    }

    /*
    [ObserversRpc]
    public void SetItemHotbar(Item item, int index)
    {
        selectedItem = item;
        selectedHotbarIndex = index;
        Hotbar[selectedHotbarIndex] = selectedItem;
    }*/

    


    [ObserversRpc]
    private void RpcSelectedItem(int slot)
    {
        selectedItem = Hotbar[slot];
        selectedHotbarIndex = slot;
        selectedItem.isEquipped = true;
        
    }


}
