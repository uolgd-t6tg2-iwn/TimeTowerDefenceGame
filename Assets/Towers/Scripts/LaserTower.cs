using UnityEngine;


public class LaserTower : TowerBase
{
    [Header("Laser Settings")] [SerializeField]
    private float dps = 20f;

    [SerializeField] public float buildCost = 30f;
    [SerializeField] private ParticleSystem impactVFX;
    private LineRenderer laser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private new void Start()
    {
        base.Start();

        laser = GetComponent<LineRenderer>();
        laser.positionCount = 2;
        laser.enabled = false;

        impactVFX = Instantiate(impactVFX);
        impactVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }


    //fires a continous laser beam
    protected override void Update()
    {
        //check for valid target and try to find one if none
        if (currentTarget == null || !InRange(currentTarget.position))
        {
            currentTarget = Targeting.FindClosestEnemy(transform.position, range, enemyLayer);
            if (currentTarget == null)
            {
                HideBeam(); //hide beam if no target found
                return;
            }
        }

        //rotate turret
        AimRotate(currentTarget.position);

        //beam start and end point
        Vector3 start = firePoint.position;
        Vector3 end = currentTarget.position + Vector3.up * 0.5f;

        laser.enabled = true;


        //update start and end point
        laser.SetPosition(0, start);
        laser.SetPosition(1, end);

        //vfx
        impactVFX.transform.position = end;
        impactVFX.transform.rotation = Quaternion.LookRotation((start - end).normalized, Vector3.up);
        impactVFX.Play();
        if (laser.enabled)
        {
            adiosrc.loop=true;
            adiosrc.Play();
        }
        else
        {
            adiosrc.Stop();
        }


        //apply damage 
        var enemyHealth = currentTarget.GetComponentInParent<EnemyHealth>();
        enemyHealth.TakeDamage(dps * Time.deltaTime);
    }

    //unused
    protected override void FireAt(Transform target)
    {
    }

    private void HideBeam()
    {
        laser.enabled = false;
        impactVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}