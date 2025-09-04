using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IslandGeneration : MonoBehaviour
{
    [Header("Small island")]
    [SerializeField] private List<GameObject> smallPrefabs;
    [SerializeField] private List<Transform> smallPositions;

    [Header("Middle island")]
    [SerializeField] private List<GameObject> middlePrefabs;
    [SerializeField] private List<Transform> middlePositions;

    [Header("Big island")]
    [SerializeField] private List<GameObject> bigPrefabs;
    [SerializeField] private List<Transform> bigPositions;

    public void Initialize()
    {
        GenerateIsland(bigPrefabs, bigPositions);
        GenerateIsland(middlePrefabs, middlePositions);
        GenerateIsland(smallPrefabs, smallPositions);
    }

    [ContextMenu("Generate Island")]
    public void Generate()
    {
        ClearTiles();
        Initialize();
    }

    [ContextMenu("Clean Island")]
    public void ClearTiles()
    {
        // Удаляем всё, что уже сгенерировано
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void GenerateIsland(List<GameObject> prefabs, List<Transform> positions)
    {
        List<Transform> newPositions = new List<Transform> ();
        foreach (Transform t in positions)
        {
            newPositions.Add(t);
        }

        for (int i = 0; i < positions.Count; i++)
        {
            int indexCurrentPosition = Random.Range(0, newPositions.Count);
            GameObject currentPrefab = prefabs[Random.Range(0, prefabs.Count)];

            Instantiate(currentPrefab, newPositions[indexCurrentPosition].position, Quaternion.Euler(0f, Random.Range(0, 359), 0f), transform);
            newPositions.Remove(newPositions[indexCurrentPosition]);
        }
    }
}
