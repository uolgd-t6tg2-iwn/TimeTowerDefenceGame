using UnityEngine;

public class CrossbowTower : TowerBase
{
    [Header("Arrow Settings")]
    [SerializeField]
    private float damage = 12f;
    [SerializeField]
    public float buildCost = 20f;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject impactVFX;

    protected override void FireAt(Transform target)
    {
        Vector3 impactPos = target.position + Vector3.up * 0.5f;

        var enemyHealth = target.GetComponentInParent<EnemyHealth>();
        enemyHealth.TakeDamage(damage);

        GameObject projectileInstance = Instantiate(projectile, firePoint.position, Quaternion.identity);
        var p = projectileInstance.GetComponent<Projectile>();
        p.projectileSetup(firePoint.position, impactPos, impactVFX);


    }
}
