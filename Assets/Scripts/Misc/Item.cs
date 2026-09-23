using UnityEngine;

public enum ItemType
{
    Consumable,
    Weapon
}

public enum ItemCategory
{
    OneHanded,
    TwoHanded
}

public class Item : MonoBehaviour
{
    public string item_name;
    public ItemType itemType;
    public ItemCategory itemCategory;
    public int item_count;
    public GameObject item;
    

    
}
