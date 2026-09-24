using PurrNet;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rig_shifting : NetworkBehaviour
{
    public Rig rig;

    [SerializeField] private float smoothTime = 0.12f;
    private float targetWeight;
    private float currentVelocity;

    private void Start()
    {
        if (rig == null)
        {
            rig = GetComponent<Rig>();
        }

        targetWeight = 0f;
        if (rig != null)
        {
            rig.weight = 0f;
        }
    }

    private void Update()
    {
        if (!isOwner) return;
        
        if (rig == null)
        {
            return;
        }

        rig.weight = Mathf.SmoothDamp(rig.weight, targetWeight, ref currentVelocity, smoothTime);
    }

    public void DisableRig()
    {
        targetWeight = 0f;
    }

    public void EnableRig()
    {
        targetWeight = 1f;
    }
}
