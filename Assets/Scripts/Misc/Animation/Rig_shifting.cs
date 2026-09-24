using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rig_shifting : MonoBehaviour
{
    public Rig rig;

    void Start()
    {
        DisableRig();
    }

    public void DisableRig()
    {
        rig.weight = 0;
    }

    public void EnableRig()
    {
        rig.weight = 1;
    }
}
