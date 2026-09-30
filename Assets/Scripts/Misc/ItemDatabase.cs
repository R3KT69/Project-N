using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("!!! CLICK UPDATE AFTER RE-ARRANGE/ADDING NEW ITEM !!!")]
    public List<GameObject> weapons;
    public List<GameObject> consumables;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.GetComponent<Item>().item_order = i;
        }
    }
}
