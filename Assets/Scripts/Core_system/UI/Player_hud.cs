using System.Collections.Generic;
using PurrNet;
using TMPro;
using UnityEngine;

public class Player_hud : NetworkBehaviour
{
    public TextMeshProUGUI idText, lookAtText, playerhp, playername;
    public TextMeshPro player_nameplate;

    public List<TextMeshProUGUI> hotbarslots;

    void Start()
    {
        if (!isOwner)
        {
            gameObject.SetActive(false);
            return;
        }
    }
}