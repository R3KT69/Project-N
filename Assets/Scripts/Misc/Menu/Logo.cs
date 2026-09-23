using UnityEngine;

public class Logo : MonoBehaviour
{
    [SerializeField] private float minSpinSpeed = 30f;
    [SerializeField] private float maxSpinSpeed = 90f;
    [SerializeField] private float changeDirectionInterval = 2.5f;
    [SerializeField] private float directionSmoothing = 1.5f;

    private Vector3 currentSpinVelocity;
    private Vector3 targetSpinVelocity;
    private float timeSinceDirectionChange;

    private void Start()
    {
        targetSpinVelocity = GetRandomSpinVelocity();
        currentSpinVelocity = targetSpinVelocity;
    }

    private void Update()
    {
        timeSinceDirectionChange += Time.deltaTime;

        if (timeSinceDirectionChange >= changeDirectionInterval)
        {
            timeSinceDirectionChange = 0f;
            targetSpinVelocity = GetRandomSpinVelocity();
        }

        currentSpinVelocity = Vector3.Lerp(
            currentSpinVelocity,
            targetSpinVelocity,
            Time.deltaTime / Mathf.Max(directionSmoothing, 0.01f));

        transform.Rotate(currentSpinVelocity * Time.deltaTime, Space.World);
    }

    private Vector3 GetRandomSpinVelocity()
    {
        Vector3 randomAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        float randomSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
        return randomAxis * randomSpeed;
    }
}
