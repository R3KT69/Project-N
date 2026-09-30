using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabase database = (ItemDatabase)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Update Item Orders"))
        {
            UpdateItemOrders(database);
        }
    }

    void UpdateItemOrders(ItemDatabase database)
    {
        for (int i = 0; i < database.weapons.Count; i++)
        {
            SetItemOrder(database.weapons[i], i);
        }

        for (int i = 0; i < database.consumables.Count; i++)
        {
            SetItemOrder(database.consumables[i], i);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Item orders updated.");
    }

    void SetItemOrder(GameObject prefab, int index)
    {
        if (prefab == null)
            return;

        string path = AssetDatabase.GetAssetPath(prefab);

        if (string.IsNullOrEmpty(path))
            return;

        GameObject prefabContents = PrefabUtility.LoadPrefabContents(path);

        Item item = prefabContents.GetComponent<Item>();

        if (item != null)
        {
            item.item_order = index;
            EditorUtility.SetDirty(item);
        }

        PrefabUtility.SaveAsPrefabAsset(prefabContents, path);
        PrefabUtility.UnloadPrefabContents(prefabContents);
    }
}