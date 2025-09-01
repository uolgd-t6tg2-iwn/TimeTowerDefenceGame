using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Mine Settings")]
    [SerializeField]
    private float damage = 75f;
    [SerializeField]
    private float damageRadius = 3f;
    [SerializeField]
    private float triggerRadius = 1.2f;
    [SerializeField]
    public float buildCost = 10f;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private LayerMask bridgeLayer;
    [SerializeField]
    private GameObject explosionVFX;

    private bool detonated = false;
    
    protected AudioSource adiosrc;
    protected void Start()
    {
        adiosrc = GetComponentInChildren<AudioSource>();
        Debug.Log("Mine AudioSource is: " + adiosrc.name);
    }
    private void Update()
    {
        //prevent multiple detonations
        if (detonated == true)
        {
            return;
        }

        Transform currentTarget = Targeting.FindClosestEnemy(transform.position, triggerRadius, enemyLayer);
        if (currentTarget != null)
        {
            detonated = true;
            Detonate();
        }
    }

    private void Detonate()
    {
        Vector3 pos = transform.position + Vector3.up * 0.5f;

        //AoE damage
        Collider[] hits = Physics.OverlapSphere(pos, damageRadius, enemyLayer, QueryTriggerInteraction.Collide);
        
        adiosrc.Play();
        
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyhealth = hits[i].GetComponentInParent<EnemyHealth>();
            enemyhealth.TakeDamage(damage);
        }

        Collider[] bridges = Physics.OverlapSphere(pos, damageRadius, bridgeLayer, QueryTriggerInteraction.Collide);
        for (int i = 0; i < bridges.Length; i++)
        {
            var bridge = bridges[i].GetComponentInParent<Bridge>();
            if (bridge != null)
            {
                bridge.TakeDamage(); 
                break;                     
            }
        }

        //vfx
        Instantiate(explosionVFX, pos, Quaternion.identity);

        //set spot as unoccupied
        TowerPlacement spot = GetComponentInParent<TowerPlacement>();
        spot.SetOccupied(false);

        adiosrc.Stop();
        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        //trigger radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);

        //dmg radius
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}