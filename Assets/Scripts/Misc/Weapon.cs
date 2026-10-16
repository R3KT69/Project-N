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
    [Header("Weapon Attributes")]
    public int Ammo;
    public float firerate;
    public float pellet_count;
    public float recoil_force;


    [Header("Weapon Requirements")]
    public ParticleSystem muzzleFlash;
    public GameObject bullet_obj;
    public Transform shootpoint;
    public AudioSource audioSource;
    public WeaponClass weaponType;
    public Player_camera player_Camera;


    public Light pointLight;
    public float duration = 0.05f;
    public float intensity = 5f;

    void Start()
    {
        pointLight = gameObject.GetComponentInChildren<Light>();
        player_Camera = transform.root.GetComponent<Player_camera>();
    }


    public void PlayRecoil(float amount)
    {
        if (player_Camera == null)
        {
            player_Camera = transform.root.GetComponent<Player_camera>();
        }

        player_Camera.Recoil(amount);
    }
 
    public void PlayVFX()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (pointLight != null)
        {
            toggle_light();
        }

    }

    public void PlaySFX()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void FireProjectile(float force = 25f)
    {
        GameObject bullet = Instantiate(
            bullet_obj,
            shootpoint.position,
            shootpoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(shootpoint.forward * force, ForceMode.Impulse);
        }

        
    }

    // RIFLE
    public void RifleFire()
    {
        FireProjectile(150);
        PlaySFX_VFX_Auto();
        PlayRecoil(recoil_force);
    }

    // PISTOL
    public void PistolFire()
    {
        FireProjectile(50);
        PlaySFX_VFX_Auto();
        PlayRecoil(recoil_force);
    }

    // SHOTGUN
    public void ShotgunFire(int pellets, float spread, float force = 25f)
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
                rb.AddForce(direction * force, ForceMode.Impulse);
            }
        }

        PlaySFX_VFX_Auto();
        PlayRecoil(recoil_force);
    }

    // AUTOMATIC
    private Coroutine automaticCoroutine;

    public void AutomaticFire(float speed = 5)
    {
        if (automaticCoroutine != null) return;

        automaticCoroutine = StartCoroutine(AutomaticFireRoutine(speed));
    }

    private IEnumerator AutomaticFireRoutine(float speed)
    {
        float interval = 1f / Mathf.Max(speed, 0.01f);

        while (true)
        {
            Debug.Log($"AUTO FIRE: {Time.time}");
            FireProjectile(100);
            PlaySFX_VFX_Auto();
            PlayRecoil(recoil_force);

            yield return new WaitForSeconds(interval);
        }
    }

    public void StopAutomaticFire()
    {
        if (automaticCoroutine == null) return;

        StopCoroutine(automaticCoroutine);
        automaticCoroutine = null;
    }

    private Coroutine lightCoroutine;

    public void toggle_light()
    {
        if (lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }

        lightCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        pointLight.enabled = true;
        pointLight.intensity = intensity;

        yield return new WaitForSeconds(duration);

        pointLight.enabled = false;
        lightCoroutine = null;
    }

    [ObserversRpc]
    public void PlaySFX_VFX_Auto()
    {
        PlaySFX();
        PlayVFX();
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


