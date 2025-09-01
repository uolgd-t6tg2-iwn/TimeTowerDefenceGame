using System;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField]
    protected float range = 12f;
    [SerializeField]
    protected LayerMask enemyLayer;
    [SerializeField]
    protected Transform turretRotatePoint;
    [SerializeField]
    protected Transform firePoint;

    [Header("Rate of Fire")]
    [SerializeField]
    protected float fireRate = 2f;
    protected float towerCooldown;
    protected Transform currentTarget;

    protected AudioSource adiosrc;
    protected void Start()
    {
        adiosrc = GetComponentInChildren<AudioSource>();
        Debug.Log("TowerBase AudioSource is: " + adiosrc.name);
        // if(adiosrc!=null)
        // {
            // Debug.Log("TowerBase AudioSource is: " + adiosrc.name);
        // }
    }

    protected virtual void Update()
    {
        //check for valid target and try to find one if none
        if (currentTarget == null || !InRange(currentTarget.position))
        {
            currentTarget = Targeting.FindClosestEnemy(transform.position, range, enemyLayer);
           
           //dont continue if no targets
            if (currentTarget == null)
            {
                return;
            }
        }

        AimRotate(currentTarget.position);

        if (Time.time >= towerCooldown)
        {
            towerCooldown = Time.time + 1f / fireRate; //rate of fire per second
            FireAt(currentTarget);
            adiosrc.Play();
        }
    }

    //check if target is close enough to shoot at
    protected virtual bool InRange(Vector3 pos)
    {
        return (pos - transform.position).magnitude <= range;
    }

    protected virtual void AimRotate(Vector3 towerPos)
    {
        Vector3 dir = towerPos - turretRotatePoint.position;
        dir.y = 0f;
        var targetRotate = Quaternion.LookRotation(dir.normalized, Vector3.up);
        turretRotatePoint.rotation = Quaternion.Slerp(turretRotatePoint.rotation, targetRotate, 10f * Time.deltaTime);
    }

    protected abstract void FireAt(Transform target);

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
