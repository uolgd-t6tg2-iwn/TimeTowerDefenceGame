using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BoxCollider area; 
    

    //spawn enemy in random position in box colider
    public GameObject SpawnOnce()
    {
        Vector3 p = RandomPointInBox(area.bounds);
        return Instantiate(enemyPrefab, p, Quaternion.identity);
    }

       public GameObject SpawnCustom(GameObject prefab)
    {
        Vector3 p = RandomPointInBox(area.bounds);
        return Instantiate(prefab, p, Quaternion.identity);
    }

    private Vector3 RandomPointInBox(Bounds pos)
    {
        return new Vector3(
            Random.Range(pos.min.x, pos.max.x),
            Random.Range(pos.min.y, pos.max.y),
            Random.Range(pos.min.z, pos.max.z)
        );
    }
}
