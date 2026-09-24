using PurrNet;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rig_shifting : NetworkBehaviour
{
    public Rig rig;

    [SerializeField] private float smoothTime = 0.12f;
    private float targetWeight;
    private float currentVelocity;
    private bool rigEnabled;

    private void Start()
    {
        if (rig == null)
        {
            rig = GetComponent<Rig>();
        }

        targetWeight = 0f;
        rigEnabled = false;

        if (rig != null)
        {
            rig.weight = 0f;
        }
    }

    private void Update()
    {
        if (rig == null)
        {
            return;
        }

        rig.weight = Mathf.SmoothDamp(rig.weight, targetWeight, ref currentVelocity, smoothTime);
    }

    public void DisableRig()
    {
        SetRigEnabled(false);
    }

    public void EnableRig()
    {
        SetRigEnabled(true);
    }

    private void SetRigEnabled(bool enabled)
    {
        if (rigEnabled == enabled)
        {
            return;
        }

        rigEnabled = enabled;
        targetWeight = enabled ? 1f : 0f;

        if (isOwner)
        {
            RpcSetRigEnabled(enabled);
        }
    }

    [ObserversRpc]
    private void RpcSetRigEnabled(bool enabled)
    {
        rigEnabled = enabled;
        targetWeight = enabled ? 1f : 0f;
    }
}
