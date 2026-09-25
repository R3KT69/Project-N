using System.Collections.Generic;
using PurrNet;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rig_shifting : NetworkBehaviour
{
    public Rig rifle_rig;
    public Rig spine_rig;
    public Rig head_rig;
    

    [SerializeField] private float smoothTime = 0.12f;
    private float targetWeight;
    private float currentVelocity;
    private bool rigEnabled;

    private void Start()
    {
        targetWeight = 0f;
        rigEnabled = false;

        if (rifle_rig != null)
        {
            rifle_rig.weight = 0f;
        }
    }

    private void Update()
    {
        if (rifle_rig == null || spine_rig == null || head_rig == null) return;
        

        toggle_rig(rifle_rig);
        toggle_rig(spine_rig);
        toggle_rig(head_rig);
    }

    public void toggle_rig(Rig selected_rig)
    {
        selected_rig.weight = Mathf.SmoothDamp(selected_rig.weight, targetWeight, ref currentVelocity, smoothTime);
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
