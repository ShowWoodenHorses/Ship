using Unity.AI.Navigation;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private WaterTileGenerator waterGenerator;
    [SerializeField] private IslandGeneration islandGenerator;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private void Start()
    {
        waterGenerator.GenerateWater();
        islandGenerator.GenerateIsland();

        navMeshSurface.BuildNavMesh();
    }
}
