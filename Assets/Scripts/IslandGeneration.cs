using System.Collections.Generic;
using UnityEngine;

public class IslandGeneration : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private List<Transform> positions;
    [SerializeField] private int count;

    private void Start()
    {
    }

    public void GenerateIsland()
    {
        for(int i = 0; i < count; i++)
        {
            int indexCurrentPosition = Random.Range(0, positions.Count);
            GameObject currentPrefab = prefabs[Random.Range(0, prefabs.Count)];

            Instantiate(currentPrefab, positions[indexCurrentPosition].position, Quaternion.identity, transform);
            positions.Remove(positions[indexCurrentPosition]);
        }
    }
}
