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

    public void DrawHotbar()
    {
        for (int i = 0; i < 6; i++)
        {
            player_Action.player_canvas.hotbarslots[i].text = $"{Hotbar[i].item_name}";
            
        }
    }

    public bool isHotbarFull()
    {
        int inv_count = 0;

        for (int i = 0; i < Hotbar.Count; i++)
        {
            if (Hotbar[i] != empty_hand.GetComponent<Item>())
            {
                inv_count += 1;
            }
        }

        Debug.Log($"current inv size: {inv_count}");

        if (inv_count >= 6) return true;

        else return false;
    }

    public bool isPrimaryEmpty()
    {
        for (int i = 0; i < Hotbar.Count; i++)
        {
            if (Hotbar[i].gameObject.GetComponent<Item>().itemCategory == ItemCategory.TwoHanded)
            {
                return false;
            }
        }

        return true;
    }

    public bool isSecondaryEmpty()
    {
        for (int i = 0; i < Hotbar.Count; i++)
        {
            if (Hotbar[i].gameObject.GetComponent<Item>().itemCategory == ItemCategory.OneHanded)
            {
                return false;
            }
        }

        return true;
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

        DrawHotbar();

        Debug.Log(isHotbarFull());

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
            /*
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
                
            }*/
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
