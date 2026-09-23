using UnityEngine;
using UnityEngine.UI;

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
    public Image image;

    void Start()
    {
        if (item == null)
        {
            item = gameObject;
        }
    }

}
