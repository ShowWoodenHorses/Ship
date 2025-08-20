using System.Collections.Generic;
using UnityEngine;

public class WaterTileGenerator : MonoBehaviour
{
    public GameObject waterTilePrefab; // prefab с твоим plane и шейдером
    public int tilesX = 10;
    public int tilesZ = 10;
    public float tileSize = 10f;
    public int[][] array = new int[4][]; // Добавили 4-е направление

    void Start()
    {
    }

    public void GenerateWater()
    {
        array[0] = new int[] { tilesX, tilesZ };  // Вправо и вверх
        array[1] = new int[] { -tilesX, tilesZ }; // Влево и вверх
        array[2] = new int[] { tilesX, -tilesZ }; // Вправо и вниз
        array[3] = new int[] { -tilesX, -tilesZ }; // Влево и вниз
        for (int i = 0; i < array.Length; i++)
        {
            GenerateTiles(array[i][0], array[i][1]);
        }
    }

    void GenerateTiles(int tilesX, int tilesZ)
    {
        // Определяем направление генерации
        int xStart = tilesX > 0 ? 0 : tilesX + 1;
        int xEnd = tilesX > 0 ? tilesX : 1;

        int zStart = tilesZ > 0 ? 0 : tilesZ + 1;
        int zEnd = tilesZ > 0 ? tilesZ : 1;

        for (int x = xStart; x < xEnd; x++)
        {
            for (int z = zStart; z < zEnd; z++)
            {
                Vector3 pos = new Vector3(x * tileSize, 0, z * tileSize);
                Instantiate(waterTilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 offset = new Vector3(
            (transform.position.x + (tilesX * tileSize) / 2) - tileSize / 2,
            0,
            (transform.position.z + (tilesZ * tileSize) / 2) - tileSize / 2);
        Gizmos.DrawWireCube(transform.position, new Vector3(tilesX * tileSize * 2, 0, tilesZ * tileSize * 2));
    }
}