using System.Collections.Generic;
using UnityEngine;

public class Player_audio : MonoBehaviour
{
    public AudioSource playerAudioSource;

    [Header("Player Audios")]
    public AudioClip drop_item;
    public AudioClip pickup_item;

    public void Drop_Item_Sfx()
    {
        playerAudioSource.clip = drop_item;
        playerAudioSource.PlayOneShot(drop_item);
    }

    public void Pickup_Item_Sfx()
    {
        playerAudioSource.clip = pickup_item;
        playerAudioSource.PlayOneShot(pickup_item);
    }
}
