using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towerPrefabs;
    [SerializeField]
    private bool occupied = false;


    public bool IsOccupied()
    {
        return occupied;
    }

    public GameObject GetPrefabByIndex(int index)
    {  
        return towerPrefabs[index];
    }

    public void SetOccupied(bool state)
    {
        occupied = state;
    }

    public bool TryPlace(int index)
    {
        if (occupied)
        {
            return false;
        }
        Vector3 spawnPos = transform.position + Vector3.up * 0.3f;
        //set tower as parent of placement spot without changing transform scale
        GameObject placed = Instantiate(towerPrefabs[index], spawnPos, transform.rotation);
        placed.transform.SetParent(transform,true); 
        occupied = true;
        return occupied;
    }
    
    public bool SupportsIndex(int index)
{   
    //prevent out of bounds error
    if (index >= towerPrefabs.Length)
        {
            
            return false;
        }

    return true;
}


}
