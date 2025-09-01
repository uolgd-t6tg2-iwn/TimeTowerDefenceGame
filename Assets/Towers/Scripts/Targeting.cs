using UnityEngine;

public static class Targeting
{
        public static Transform FindClosestEnemy(Vector3 origin, float radius, LayerMask enemyLayer)
    {


        Collider[] hits = Physics.OverlapSphere(origin, radius, enemyLayer, QueryTriggerInteraction.Collide);
        if (hits.Length == 0)
        {
            return null;
        }
        //straight line distance
        Collider closest = hits[0];
        float closestDist = (closest.transform.position - origin).magnitude;
        for (int i = 1; i < hits.Length; i++)
        {
            float currentDist = (hits[i].transform.position - origin).magnitude;
            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closest = hits[i];
            }
        }
        return closest.transform;
    }
}
