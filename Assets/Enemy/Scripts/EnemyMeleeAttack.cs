using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private float meleeDamage = 10f;

    [SerializeField]
    private float meleeCooldown = 1f;

    [SerializeField]
    private float meleeRange = 2f;
    [SerializeField]
    private LayerMask barrierMask;

    private TimeMachineHealth timeMachine;

    private Collider timeMachineCollider;
    private float currentMeleeCooldown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        timeMachine = FindFirstObjectByType<TimeMachineHealth>();
        timeMachineCollider = timeMachine.GetComponent<Collider>();
    }

    public void MultiplyDamage(float amount)
    {
        meleeDamage = meleeDamage * amount;
    }

    private void Update()
    {
        if (Time.time >= currentMeleeCooldown)
        {
            var barrier = FindBarrierInRange();
            if (barrier != null)
            {
                Debug.Log("Attacking Barrier");
                currentMeleeCooldown = Time.time + meleeCooldown;
                barrier.TakeDamage(meleeDamage);
                return;
            }

            Vector3 toTarget = timeMachineCollider.ClosestPoint(transform.position);
            float dist = (toTarget - transform.position).magnitude;

            if (dist <= meleeRange)
            {
                currentMeleeCooldown = Time.time + meleeCooldown;
                //apply dmg to time machine
                timeMachine.TakeDamage(meleeDamage);

            }
        }
    }
    WoodenBarrier FindBarrierInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, meleeRange, barrierMask, QueryTriggerInteraction.Collide);
        if (hits.Length == 0)
        {
            return null;
        }

        Collider closest = hits[0];
        float closestDist = (hits[0].transform.position - transform.position).magnitude;

        for (int i = 1; i < hits.Length; i++)
        {
            float currentDist = (hits[i].transform.position - transform.position).magnitude;
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closest = hits[i];
            }
        }

        return closest.GetComponentInParent<WoodenBarrier>();
    }
}
