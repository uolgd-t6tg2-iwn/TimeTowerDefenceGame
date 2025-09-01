using UnityEngine;
using UnityEngine.AI;

public class BossBuff : MonoBehaviour
{
    [SerializeField]
    private float damageMultiplier = 2f;

    [SerializeField]
    private float speedMultiplier = 1.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float buffHealthActivationPercent = 0.5f;

    [SerializeField]
    private GameObject buffEffect; 

    [SerializeField]
    private EnemyHealth health;

    [SerializeField]
    private EnemyMeleeAttack melee;

    [SerializeField]
    private NavMeshAgent agent;

    private bool buffActive = false;

    private void Update()
    {
        if (buffActive == false)
        {
            float hpPercent = health.GetHealthPercent();

            if (hpPercent <= buffHealthActivationPercent)
            {
                buffActive = true;

                melee.MultiplyDamage(damageMultiplier);
                agent.speed = agent.speed * speedMultiplier;

                //vfx
                Vector3 origin = transform.position + (Vector3.up * 3f);
                Instantiate(buffEffect, origin, Quaternion.identity, transform);
            }
        }
    }
}
