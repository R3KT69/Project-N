using PurrNet;
using UnityEngine;

public class Player_information : NetworkBehaviour
{
    public Player_hud player_hud;
    public string player_name;
    public string player_id;
    public int player_health = 100;


    
    void Start()
    {
        if (!isOwner) 
        {
            return; 
        }


        initPlayer();

        player_hud = GetComponentInChildren<Player_hud>();

    }

    void initPlayer()
    {
        if (player_name == null || string.IsNullOrWhiteSpace(player_name))
        {
            player_name = $"{Network_connection.player_name} [{localPlayer}]";
        }

        if (player_id == null || string.IsNullOrWhiteSpace(player_id))
        {
            player_id = $"{localPlayer}";
        }

        player_health = 100;
    }

    void Update()
    {
        if (!isOwner)
        {
            
            return;
        }

        player_hud.idText.text = $"player id: {player_id}";
        player_hud.playerhp.text = $"player hp: {player_health}";
        player_hud.playername.text = $"player name: {player_name}";
        UpdateNametag(player_hud, player_name);

    }

    [ObserversRpc]
    public void UpdateNametag(Player_hud player_Hud_other, string name)
    {
        player_Hud_other.player_nameplate.text = name;
    }

}
