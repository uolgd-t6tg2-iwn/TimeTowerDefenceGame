using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem; // <- new Input System

public class PlayerBuild : MonoBehaviour
{

    [SerializeField]
    private float interactRadius = 2.5f;
    [SerializeField]
    private LayerMask towerSpotMask;

    private TowerPlacement currentSpot;
    private ResourceSystem resources;

    private void Start()
    {
        resources = FindFirstObjectByType<ResourceSystem>();
    }

    void Update()
    {
        currentSpot = FindNearestFreeSpot();
    }
    public void OnBuild1(InputAction.CallbackContext context)
    {
        // check if build spot e xists and isnt already occupied
        if (currentSpot == null)
        {
            return;
        }
        if (currentSpot.IsOccupied() == true)
        {
            return;
        }

        if (context.performed)
        {
            //if its a barrier or mine stop button from working
            GameObject prefab = currentSpot.GetPrefabByIndex(0);
            if (prefab.CompareTag("Mine") == true || prefab.CompareTag("Barrier") == true)
            {
                return;
            }

            if (currentSpot.SupportsIndex(0))
            {
                TryPlace(0);
            }
        }
    }

    public void OnBuild2(InputAction.CallbackContext context)
    {

        if (currentSpot == null)
        {
            return;
        }
        if (currentSpot.IsOccupied() == true)
        {

            return;
        }
        if (context.performed)
        {
            if (currentSpot.SupportsIndex(1))
            {
                TryPlace(1);
            }
        }
    }

    public void OnBuild3(InputAction.CallbackContext context)
    {

        if (currentSpot == null)
        {
            return;
        }
        if (currentSpot.IsOccupied() == true)
        {

            return;
        }

        if (context.performed)
        {
            if (currentSpot.SupportsIndex(2))
            {
                TryPlace(2);
            }
        }
    }

    public void OnBuild4(InputAction.CallbackContext context)
    {
        if (currentSpot == null)
        {
            return;
        }
        if (currentSpot.IsOccupied() == true)
        {

            return;
        }
        GameObject prefab = currentSpot.GetPrefabByIndex(0);
        if (prefab.CompareTag("Barrier") == true)
        {
            if (context.performed)
            {
                if (currentSpot.SupportsIndex(0))
                {
                    TryPlace(0);
                }
            }
        }
    }

    public void OnBuild5(InputAction.CallbackContext context)
    {
        if (currentSpot == null)
        {
            return;
        }
        if (currentSpot.IsOccupied() == true)
        {

            return;
        }
        GameObject prefab = currentSpot.GetPrefabByIndex(0);
        if (prefab.CompareTag("Mine") == true)
        {
            if (context.performed)
            {
                if (currentSpot.SupportsIndex(0))
                {
                    TryPlace(0);
                }
            }
        }
    }


    //check if spot is valid to place tower
    private void TryPlace(int index)
    {

        GameObject prefab = currentSpot.GetPrefabByIndex(index);

        float cost = GetCostFromPrefab(prefab);

        bool paid = resources.TrySpend(cost);

        //only allow building if you have enough resources
        if (paid == true)
        {
            Debug.Log($"Paid {cost}");
            currentSpot.TryPlace(index);
        }
        else
        {
            Debug.Log("Not enough resources!");
        }
    }

    private float GetCostFromPrefab(GameObject prefab)
    {

        CannonTower cannon = prefab.GetComponent<CannonTower>();
        if (cannon != null)
        {
            return cannon.buildCost;
        }

        CrossbowTower crossbow = prefab.GetComponent<CrossbowTower>();
        if (crossbow != null)
        {
            return crossbow.buildCost;
        }

        LaserTower laser = prefab.GetComponent<LaserTower>();
        if (laser != null)
        {
            return laser.buildCost;
        }

        WoodenBarrier barrier = prefab.GetComponent<WoodenBarrier>();
        if (barrier != null)
        {
            return barrier.buildCost;
        }
        Mine mine = prefab.GetComponent<Mine>();
        if (mine != null)
        {
            return mine.buildCost;
        }

        Debug.Log("Nothing to place!");
        return 0f;
    }


    //find the nearest free build spot
    private TowerPlacement FindNearestFreeSpot()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius, towerSpotMask, QueryTriggerInteraction.Collide);
        TowerPlacement closest = null;
        float bestDist = 0f;

        for (int i = 0; i < hits.Length; i++)
        {
            TowerPlacement spot = hits[i].GetComponentInParent<TowerPlacement>();

            if (!spot.IsOccupied())
            {

                float dist = (spot.transform.position - transform.position).magnitude;

                if (closest == null || dist < bestDist)
                {
                    closest = spot;
                    bestDist = dist;
                }
            }
        }
        return closest;
    }
}
