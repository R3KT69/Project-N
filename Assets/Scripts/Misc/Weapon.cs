using PurrNet;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Ammo;

    public void TransferData(Weapon previous)
    {
        Ammo = previous.Ammo;
    }
}


