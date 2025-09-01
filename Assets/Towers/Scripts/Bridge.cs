using UnityEngine;

public class Bridge : MonoBehaviour
{
    [Header("Bridge Settings")]
    [SerializeField]
    private GameObject brokenBridgePrefab;

    [SerializeField]
     private LayerMask enemyLayer;
    [SerializeField]
    private float damage = 999999f;
    [SerializeField]
    private float damageRadius = 2.5f;


    public void TakeDamage()
    {
        Die();
    }

    void Die()
    {   
       Vector3 pos = transform.position;

        Collider[] hits = Physics.OverlapSphere(pos, damageRadius, enemyLayer, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyhealth = hits[i].GetComponentInParent<EnemyHealth>();
            enemyhealth.TakeDamage(damage);
        }

        Instantiate(brokenBridgePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
