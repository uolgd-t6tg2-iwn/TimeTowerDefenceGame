using UnityEngine;
using UnityEngine.AI;

public class AgentPathfinding : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    [SerializeField]
    private float searchRadius = 9999f;

    [SerializeField]
    private LayerMask moveSpotMask;

    [SerializeField]
    private float stopDistance = 2f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDistance;

        target = FindNearestMoveSpot();
    }

    private void Update()
    {
        target = FindNearestMoveSpot();
        Vector3 targetPos = target.position;

        float dist = (targetPos - transform.position).magnitude;

        //stops the agent when they get close to their desitnation 
        if (dist <= stopDistance)
        {
            if (agent.isStopped == false)
            {
                agent.isStopped = true;
            }
        }
        else
        {
            if (agent.isStopped == true)
            {
                agent.isStopped = false;
            }

            agent.SetDestination(targetPos);
        }
    }

    private Transform FindNearestMoveSpot()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, moveSpotMask, QueryTriggerInteraction.Collide);

        Transform closest = null;
        float bestDist = 0f;

        for (int i = 0; i < hits.Length; i++)
        {
            Transform spot = hits[i].transform;

            float dist = (spot.position - transform.position).magnitude;

            if (closest == null || dist < bestDist)
            {
                closest = spot;
                bestDist = dist;
            }
        }
        return closest;
    }
}
