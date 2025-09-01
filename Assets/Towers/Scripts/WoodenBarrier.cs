using UnityEngine;

public class WoodenBarrier : MonoBehaviour
{
    [Header("Wooden Barrier Settings")]

    [SerializeField]
    public float barrierHealth = 100f;

    [SerializeField]
    public float buildCost = 10f;
    [SerializeField]
    private GameObject brokenBarrierPrefab;
    [SerializeField]
    private float spawnInvuln = 1f;

    private float currentHealth;
    private float invulnerableUntil;

    void OnEnable()
    {
        invulnerableUntil = Time.time + spawnInvuln;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = barrierHealth;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time < invulnerableUntil)
        {
            return;
        }

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        //set spot as unoccupied
        TowerPlacement spot = GetComponentInParent<TowerPlacement>();
        spot.SetOccupied(false);
        Instantiate(brokenBarrierPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
