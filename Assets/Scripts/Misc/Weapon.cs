using PurrNet;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    public int Ammo;

    [ObserversRpc]
    public void TransferData(int old_Ammo)
    {
        Ammo = old_Ammo;
    }

    [ObserversRpc]
    public void ReduceAmmo()
    {
        Ammo -= 1;
    }
}


