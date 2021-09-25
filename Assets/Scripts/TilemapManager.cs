using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] int worldWidth;
    [SerializeField] int worldHeight;
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase dirt;
    [SerializeField] TileBase dirtStone;
    [SerializeField] List<TileBase> waterVariants;
    [SerializeField] List<TileBase> plantVariants;

    // Start is called before the first frame update
    void Start()
    {
        RenderMap(GenerateArray(worldWidth, worldHeight));
    }


    public static int[,] GenerateArray(int width, int height)
    {
        int[,] map = new int[width, height];
        for(int x = 0; x < map.GetUpperBound(0); x++)
        {
            for(int y = 0; y < map.GetUpperBound(1); y++)
            {
                float random = Random.Range(0f, 1f);
                if(random < 0.9f)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
                float water = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                if (water < 0.1f)
                {
                    map[x, y] = 2;
                }
            }
        }
        return map;
    }



    public int[,] RenderMap(int[,] map)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                int borders = 0;
                int tileType = map[x, y];

                //above
                try
                {
                    if (map[x, y + 1] != tileType)
                    {
                        borders += 1;
                    }
                }
                catch
                {
                    if(map[x, 0] != tileType)
                    {
                        borders += 1;
                    }
                }

                //right
                try
                {
                    if(map[x + 1, y] != tileType)
                    {
                        borders += 2;
                    }
                }
                catch
                {
                    if(map[0, y] != tileType)
                    {
                        borders += 2;
                    }
                }

                //down
                try
                {
                    if(map[x, y - 1] != tileType)
                    {
                        borders += 4;
                    }
                }
                catch
                {
                    if(map[x, map.GetUpperBound(1)] != tileType)
                    {
                        borders += 4;
                    }
                }

                //left
                try
                {
                    if (map[x - 1, y] != tileType)
                    {
                        borders += 8;
                    }
                }
                catch
                {
                    if(map[map.GetUpperBound(0), y] != tileType)
                    {
                        borders += 8;
                    }
                }
                
                //set tile
                //water tiles
                if(tileType == 2)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), waterVariants[borders]);

                }
                else if (tileType == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), dirtStone);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }


            }
        }
        return map;
    }
}
