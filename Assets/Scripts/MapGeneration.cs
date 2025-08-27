using Unity.AI.Navigation;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private WaterTileGenerator waterGenerator;
    [SerializeField] private IslandGeneration islandGenerator;
    [SerializeField] private NavMeshSurface navMeshSurface;


    //Bootstrap
    //private void Start()
    //{
    //    waterGenerator.GenerateWater();
    //    islandGenerator.GenerateIsland();

    //    navMeshSurface.BuildNavMesh();
    //}

    public void Initialize()
    {
        enabled = false;
        waterGenerator.GenerateWater();
        islandGenerator.GenerateIsland();

        navMeshSurface.BuildNavMesh();
        enabled = true;
    }
}
