using UnityEngine;

public class CannonTower : TowerBase
{
    [Header("Cannon Settings")]
    [SerializeField]
    private float damage = 50f;
    [SerializeField]
    private float damageRadius = 3.0f;
    [SerializeField]
    public float buildCost = 30f;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject impactVFX;

    protected override void FireAt(Transform target)
    {

        //aoe dmg
        Vector3 impactPos = target.position + Vector3.up * 0.5f;
        Collider[] hits = Physics.OverlapSphere(impactPos, damageRadius, enemyLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyhealth = hits[i].GetComponentInParent<EnemyHealth>();
            enemyhealth.TakeDamage(damage);
        }

        GameObject projectileInstance = Instantiate(projectile, firePoint.position, Quaternion.identity);
        Projectile p = projectileInstance.GetComponent<Projectile>();
        p.projectileSetup(firePoint.position, impactPos, impactVFX);
    }

}
