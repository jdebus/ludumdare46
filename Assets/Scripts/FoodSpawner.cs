using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] foodPrefabs;

    [SerializeField]
    int amount = 10;

    [SerializeField]
    float spawnRadius = 10;

    [ContextMenu("SpawnFood")]
    void SpawnFood()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 pos = ((Vector2)Random.insideUnitSphere) * spawnRadius + (Vector2)transform.position;
           
            var foodGO = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)], pos, Quaternion.identity);
            foodGO.transform.SetParent(this.transform);
        }
    }


    private void Start()
    {
        SpawnFood();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

    }

}
