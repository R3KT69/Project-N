using PurrNet;
using UnityEngine;

public class CubeSpawner : NetworkBehaviour
{
    public GameObject cube;

    void Update()
    {
        if (!isOwner)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(cube, transform.position, transform.rotation);
        }
    }
}
