using System.Collections;
using PurrNet;
using UnityEngine;

public enum WeaponClass
{
    Automatic,
    Shotgun,
    SemiAutomatic
}

public class Weapon : NetworkBehaviour
{
    public int Ammo;
    public ParticleSystem muzzleFlash;
    public GameObject bullet_obj;
    public Transform shootpoint;
    public AudioSource audioSource;
    public WeaponClass weaponType;

    public void PlayVFX()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    public void PlaySFX()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void FireProjectile()
    {
        GameObject bullet = Instantiate(
            bullet_obj,
            shootpoint.position,
            shootpoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(shootpoint.forward * 25f, ForceMode.Impulse);
        }
    }

    // RIFLE
    public void RifleFire()
    {
        FireProjectile();
    }

    // PISTOL
    public void PistolFire()
    {
        FireProjectile();
    }

    // SHOTGUN
    public void ShotgunFire(int pellets, float spread)
    {
        float halfAngle = spread * 0.5f;
        float cosMaxAngle = Mathf.Cos(halfAngle * Mathf.Deg2Rad);

        for (int i = 0; i < pellets; i++)
        {
            float cosTheta = Random.Range(cosMaxAngle, 1f);
            float sinTheta = Mathf.Sqrt(1f - cosTheta * cosTheta);
            float phi = Random.Range(0f, 2f * Mathf.PI);

            Vector3 localDirection = new Vector3(
                sinTheta * Mathf.Cos(phi),
                sinTheta * Mathf.Sin(phi),
                cosTheta
            );

            Vector3 direction = shootpoint.TransformDirection(localDirection);

            GameObject bullet = Instantiate(
                bullet_obj,
                shootpoint.position,
                Quaternion.LookRotation(direction)
            );

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(direction * 25f, ForceMode.Impulse);
            }
        }
    }

    // AUTOMATIC
    public void AutomaticFire(float speed)
    {
        if (automaticCoroutine != null) return;

        automaticCoroutine = StartCoroutine(AutomaticFireRoutine(speed));
    }

    private Coroutine automaticCoroutine;

    private IEnumerator AutomaticFireRoutine(float speed)
    {
        float interval = 1f / Mathf.Max(speed, 0.01f);

        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(interval);
        }
    }

    public void StopAutomaticFire()
    {
        if (automaticCoroutine != null)
        {
            StopCoroutine(automaticCoroutine);
            automaticCoroutine = null;
        }
    }

    [ObserversRpc]
    public void TransferData(int old_Ammo)
    {
        Ammo = old_Ammo;
    }

    [ObserversRpc]
    public void ReduceAmmo()
    {
        Debug.Log(
            "REDUCE AMMO\n" +
            "Object: " + gameObject.name +
            "\nOwner: " + isOwner +
            "\nServer: " + isServer +
            "\nSpawned: " + isSpawned
        );

        Ammo -= 1;
    }
}


