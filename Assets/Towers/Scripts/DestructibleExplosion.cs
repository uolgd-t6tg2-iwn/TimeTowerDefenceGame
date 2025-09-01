using UnityEngine;

public class DestructibleExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionForce = 100f;
    [SerializeField]
    private float explosionRadius = 3f;
    [SerializeField]
    private float upwardsModifier = 0.2f;
    [SerializeField]
    private float despawnDelay = 2f;

    void Start()
    {
        Vector3 center = transform.position;

        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < bodies.Length; i++)
        {
            var rb = bodies[i];
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, center, explosionRadius, upwardsModifier, ForceMode.Impulse);

            Destroy(rb.gameObject, despawnDelay);
        }
        Destroy(gameObject, despawnDelay + 1f);
    }
}
