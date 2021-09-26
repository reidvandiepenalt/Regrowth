using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TilemapManager : MonoBehaviour
{
    public static int worldWidth = 150;
    public static int worldHeight = 150;
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase dirt;
    [SerializeField] TileBase dirtStone;
    [SerializeField] List<TileBase> waterVariants;
    [SerializeField] List<TileBase> plantVariants;
    [SerializeField] Camera camera;
    [SerializeField] CameraController camController;
    [SerializeField] CanFillScript canFill;
    [SerializeField] float minRandomTime;
    [SerializeField] float maxRandomTime;
    [SerializeField] GameObject plantTop1;
    [SerializeField] GameObject plantTop2;
    [SerializeField] GameObject speech;
    [SerializeField] GameObject pointer;
    [SerializeField] float pointerRadius;
    [SerializeField] Text counter;
    int lifeEssenceCount = 0;
    public static int cameraTileWidth = 28;
    public static int cameraTileHeight = 16;
    float intensity = 0.9f;
    float xMult = 0.9f;
    float yMult = 0.9f;
    float scale = 1f;
    GameTile[,] gameTiles;
    public Dictionary<GameObject, GameObject> notifications = new Dictionary<GameObject, GameObject>();

    float randomTickTime;
    float timer = 0;

    bool xMovedRight = false;
    bool yMovedUp = false;


    // Start is called before the first frame update
    void Start()
    {
        RenderMap(GenerateArray(worldWidth + cameraTileWidth, worldHeight + cameraTileHeight));

        randomTickTime = Random.Range(minRandomTime, maxRandomTime);
        for(int i = 0; i < 15; i++)
        {
            RandomTick();
        }
    }

    public void ShiftRight()
    {
        xMovedRight = true;
        for (int x = 0; x < cameraTileWidth; x++)
        {
            if (yMovedUp)
            {
                for (int y = cameraTileHeight; y <= gameTiles.GetUpperBound(1); y++)
                {
                    gameTiles[worldWidth + x, y] = gameTiles[x, y];
                    gameTiles[x, y].Reset();
                    if (gameTiles[worldWidth + x, y].speechBubble != null)
                    {
                        gameTiles[worldWidth + x, y].speechBubble.transform.position =
                            new Vector3(worldWidth + x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[worldWidth + x, y].topSprite != null)
                    {
                        gameTiles[worldWidth + x, y].topSprite.transform.position = new Vector3(worldWidth + x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(worldWidth + x, y, 0), tilemap.GetTile(new Vector3Int(x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            else
            {
                for (int y = 0; y <= worldHeight; y++)
                {
                    gameTiles[worldWidth + x, y] = gameTiles[x, y];
                    gameTiles[x, y].Reset();
                    if (gameTiles[worldWidth + x, y].speechBubble != null)
                    {
                        gameTiles[worldWidth + x, y].speechBubble.transform.position =
                            new Vector3(worldWidth + x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[worldWidth + x, y].topSprite != null)
                    {
                        gameTiles[worldWidth + x, y].topSprite.transform.position = new Vector3(worldWidth + x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(worldWidth + x, y, 0), tilemap.GetTile(new Vector3Int(x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    public void ShiftLeft()
    {
        xMovedRight = false;
        for (int x = 0; x < cameraTileWidth; x++)
        {
            if (yMovedUp)
            {
                for (int y = cameraTileHeight; y <= gameTiles.GetUpperBound(1); y++)
                {
                    gameTiles[x, y] = gameTiles[worldWidth + x, y];
                    gameTiles[worldWidth + x, y].Reset();
                    if (gameTiles[x, y].speechBubble != null)
                    {
                        gameTiles[x, y].speechBubble.transform.position = new Vector3(x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[x, y].topSprite != null)
                    {
                        gameTiles[x, y].topSprite.transform.position = new Vector3(x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilemap.GetTile(new Vector3Int(worldWidth + x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x + worldWidth, y, 0), null);
                }
            }
            else
            {
                for (int y = 0; y <= worldHeight; y++)
                {
                    gameTiles[x, y] = gameTiles[worldWidth + x, y];
                    gameTiles[worldWidth + x, y].Reset();
                    if (gameTiles[x, y].speechBubble != null)
                    {
                        gameTiles[x, y].speechBubble.transform.position = new Vector3(x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[x, y].topSprite != null)
                    {
                        gameTiles[x, y].topSprite.transform.position = new Vector3(x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilemap.GetTile(new Vector3Int(worldWidth + x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x + worldWidth, y, 0), null);
                }
            }
        }
    }

    public void ShiftUp()
    {
        yMovedUp = true;
        for (int y = 0; y < cameraTileHeight; y++)
        {
            if (xMovedRight)
            {
                for (int x = cameraTileWidth; x <= gameTiles.GetUpperBound(0); x++)
                {
                    gameTiles[x, y + worldHeight] = gameTiles[x, y];
                    gameTiles[x, y].Reset();
                    if (gameTiles[x, y + worldHeight].speechBubble != null)
                    {
                        gameTiles[x, y + worldHeight].speechBubble.transform.position =
                            new Vector3(x - 0.25f, y + worldHeight + 1, 30);
                    }
                    if (gameTiles[x, y + worldHeight].topSprite != null)
                    {
                        gameTiles[x, y + worldHeight].topSprite.transform.position = new Vector3(x, y + worldHeight + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y + worldHeight, 0), tilemap.GetTile(new Vector3Int(x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            else
            {
                for (int x = 0; x <= worldWidth; x++)
                {
                    gameTiles[x, y + worldHeight] = gameTiles[x, y];
                    gameTiles[x, y].Reset();
                    if (gameTiles[x, y + worldHeight].speechBubble != null)
                    {
                        gameTiles[x, y + worldHeight].speechBubble.transform.position =
                            new Vector3(x - 0.25f, y + worldHeight + 1, 30);
                    }
                    if (gameTiles[x, y + worldHeight].topSprite != null)
                    {
                        gameTiles[x, y + worldHeight].topSprite.transform.position = new Vector3(x, y + worldHeight + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y + worldHeight, 0), tilemap.GetTile(new Vector3Int(x, y, 0)));
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    public void ShiftDown()
    {
        yMovedUp = false;
        for (int y = 0; y < cameraTileHeight; y++)
        {
            if (xMovedRight)
            {
                for (int x = cameraTileWidth; x <= gameTiles.GetUpperBound(0); x++)
                {
                    gameTiles[x, y] = gameTiles[x, y + worldHeight];
                    gameTiles[x, y + worldHeight].Reset();
                    if (gameTiles[x, y].speechBubble != null)
                    {
                        gameTiles[x, y].speechBubble.transform.position = new Vector3(x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[x, y].topSprite != null)
                    {
                        gameTiles[x, y].topSprite.transform.position = new Vector3(x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilemap.GetTile(new Vector3Int(x, y + worldHeight, 0)));
                    tilemap.SetTile(new Vector3Int(x, y + worldHeight, 0), null);
                }
            }
            else
            {
                for (int x = 0; x <= worldWidth; x++)
                {
                    gameTiles[x, y] = gameTiles[x, y + worldHeight];
                    gameTiles[x, y + worldHeight].Reset();
                    if (gameTiles[x, y].speechBubble != null)
                    {
                        gameTiles[x, y].speechBubble.transform.position = new Vector3(x - 0.25f, y + 1, 30);
                    }
                    if (gameTiles[x, y].topSprite != null)
                    {
                        gameTiles[x, y].topSprite.transform.position = new Vector3(x, y + 1, 30);
                    }
                    tilemap.SetTile(new Vector3Int(x, y, 0), tilemap.GetTile(new Vector3Int(x, y + worldHeight, 0)));
                    tilemap.SetTile(new Vector3Int(x, y + worldHeight, 0), null);
                }
            }
        }
    }

    private void Update()
    {
        //camera reposition tiles
        if(camera.transform.position.x > (worldWidth - (2 * cameraTileWidth)) && !xMovedRight && camController.lastDirection.x > 0)
        {
            ShiftRight();
        }
        else if (camera.transform.position.x < (2 * cameraTileWidth) && xMovedRight && camController.lastDirection.x < 0)
        {
            ShiftLeft();
        }

        if(camera.transform.position.y > (worldWidth - (cameraTileHeight * 2)) && !yMovedUp && camController.lastDirection.y > 0)
        {
            ShiftUp();
        }
        else if (camera.transform.position.y < cameraTileHeight * 2 && yMovedUp && camController.lastDirection.y < 0)
        {
            ShiftDown();
        }

        //update pointers
        foreach(KeyValuePair<GameObject, GameObject> entry in notifications)
        {
            //check if in camera
            Vector3 viewportPoint = camera.WorldToViewportPoint(entry.Key.transform.position + Vector3.forward);
            if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
            {
                entry.Value.SetActive(false);
            }
            else
            {
                entry.Value.SetActive(true);
                Vector2 direction = (camera.transform.position - entry.Key.transform.position).normalized;
                float rotation = Mathf.Atan2(direction.y, direction.x) + Mathf.PI;
                entry.Value.transform.rotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);
                entry.Value.transform.position = new Vector3((Mathf.Cos(rotation) * pointerRadius) + camera.transform.position.x,
                   (Mathf.Sin(rotation) * pointerRadius) + camera.transform.position.y, -10);
            }
        }


        //player click
        if (Input.GetMouseButtonDown(0))
        {
            /*Vector3 clickScreenPos = Input.mousePosition;
            Debug.Log("lensed " + lensDistort(clickScreenPos));
            Vector3 clickWorldPos = camera.ScreenToWorldPoint(lensDistort(clickScreenPos) + Vector3.forward * 30);
            Debug.Log("click pos: " + clickWorldPos);
            TileBase clickedTile = tilemap.GetTile(tilemap.WorldToCell(clickWorldPos));*/
            Vector3 clickWorldPos = camera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 30);
            Vector3Int tilePos = tilemap.WorldToCell(clickWorldPos);
            if (gameTiles[tilePos.x, tilePos.y].type == TileType.Water)//water
            {
                canFill.ReFill();
            }
            else if(gameTiles[tilePos.x, tilePos.y].type == TileType.Plant)//plants
            {
                if(gameTiles[tilePos.x, tilePos.y].plantStage == 0)
                {
                    gameTiles[tilePos.x, tilePos.y].plantStage = 1;
                    tilemap.SetTile(tilePos, plantVariants[1]);
                }
                else if(gameTiles[tilePos.x, tilePos.y].plantStage == 6)
                {
                    Destroy(gameTiles[tilePos.x, tilePos.y].topSprite);
                    gameTiles[tilePos.x, tilePos.y].type = TileType.Dirt;
                    gameTiles[tilePos.x, tilePos.y].plantStage = 0;
                    gameTiles[tilePos.x, tilePos.y].waterable = false;
                    tilemap.SetTile(tilePos, dirt);
                    lifeEssenceCount++;
                    counter.text = lifeEssenceCount.ToString();
                }
                else if(gameTiles[tilePos.x, tilePos.y].plantStage != plantVariants.Count - 1 && gameTiles[tilePos.x, tilePos.y].waterable)
                {
                    if (canFill.UseCan())
                    {
                        gameTiles[tilePos.x, tilePos.y].waterable = false;
                        gameTiles[tilePos.x, tilePos.y].plantStage += 1;
                        Destroy(notifications[gameTiles[tilePos.x, tilePos.y].speechBubble]);
                        notifications.Remove(gameTiles[tilePos.x, tilePos.y].speechBubble);
                        Destroy(gameTiles[tilePos.x, tilePos.y].speechBubble);
                        tilemap.SetTile(tilePos, plantVariants[gameTiles[tilePos.x, tilePos.y].plantStage]);
                        if (gameTiles[tilePos.x, tilePos.y].plantStage == 5)
                        {
                            gameTiles[tilePos.x, tilePos.y].topSprite = Instantiate(plantTop1, 
                                new Vector3(tilePos.x, tilePos.y + 1, -5), Quaternion.identity);
                        }
                        else if (gameTiles[tilePos.x, tilePos.y].plantStage == 6)
                        {
                            Destroy(gameTiles[tilePos.x, tilePos.y].topSprite);
                            gameTiles[tilePos.x, tilePos.y].topSprite = Instantiate(plantTop2,
                                new Vector3(tilePos.x, tilePos.y + 1, -5), Quaternion.identity);
                        }
                    }
                }
            }
        }


        //random ticks
        if(timer >= randomTickTime)
        {
            timer = 0;
            randomTickTime = Random.Range(minRandomTime, maxRandomTime);
            RandomTick();
        }
        if(timer < randomTickTime)
        {
            timer += Time.deltaTime;
        }
    }

    void RandomTick()
    {
        List<GameTile> randomDirtTiles = new List<GameTile>();
        List<GameTile> randomPlantTiles = new List<GameTile>();
        List<Vector2Int> dirtTilePositions = new List<Vector2Int>();
        List<Vector2Int> plantTilePositions = new List<Vector2Int>();
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                if (gameTiles[x, y].type == TileType.Dirt) 
                {
                    randomDirtTiles.Add(gameTiles[x, y]);
                    dirtTilePositions.Add(new Vector2Int(x, y));
                }
                else if (gameTiles[x, y].type == TileType.Plant && gameTiles[x, y].plantStage !=0 &&
                    gameTiles[x, y].plantStage != 6 && !gameTiles[x, y].waterable)
                {
                    randomPlantTiles.Add(gameTiles[x, y]);
                    plantTilePositions.Add(new Vector2Int(x, y));
                }
            }
        }

        if(randomDirtTiles.Count > 0)
        {
            int dirtIndex = Random.Range(0, randomDirtTiles.Count);
            Vector2Int dirtPosition = dirtTilePositions[dirtIndex];
            //set to seed
            gameTiles[dirtPosition.x, dirtPosition.y].type = TileType.Plant;
            tilemap.SetTile((Vector3Int)dirtPosition, plantVariants[0]);

        }

        if(randomPlantTiles.Count > 0)
        {
            
            int plantIndex = Random.Range(0, randomPlantTiles.Count);
            Vector2Int plantPosition = plantTilePositions[plantIndex];
            //set to waterable
            gameTiles[plantPosition.x, plantPosition.y].waterable = true;
            gameTiles[plantPosition.x, plantPosition.y].speechBubble = Instantiate(speech,
                new Vector3(plantPosition.x - 0.25f, plantPosition.y + 1, 0), Quaternion.identity);
            notifications.Add(gameTiles[plantPosition.x, plantPosition.y].speechBubble, Instantiate(pointer));
        }
    }

    Vector3 lensDistort(Vector3 clickPos)
    {
        Vector2 screenSize = new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight);
        Vector2 centeredClickPos = new Vector2(clickPos.x - (screenSize.x / 2f), clickPos.y - (screenSize.y / 2f));
        centeredClickPos = centeredClickPos * scale;
        centeredClickPos = new Vector2(centeredClickPos.x * (1 + xMult * intensity), centeredClickPos.y * (1 + intensity * yMult));
        return new Vector3(centeredClickPos.x + (screenSize.x / 2f), centeredClickPos.y + (screenSize.y / 2f), 0);
    }


    public int[,] GenerateArray(int width, int height)
    {
        int[,] map = new int[width, height];
        gameTiles = new GameTile[width, height];
        for(int x = 0; x < worldWidth; x++)
        {
            for(int y = 0; y < worldHeight; y++)
            {
                float random = Random.Range(0f, 1f);
                if (random < 0.95f)
                {
                    map[x, y] = 0;
                    gameTiles[x, y] = new GameTile(TileType.Dirt);
                }
                else
                {
                    map[x, y] = 1;
                    gameTiles[x, y] = new GameTile(TileType.Dirt);
                }
                float water = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                if (water < 0.1f)
                {
                    map[x, y] = 2;
                    gameTiles[x, y] = new GameTile(TileType.Water);
                } 
            }
        }

        return map;
    }



    public int[,] RenderMap(int[,] map)
    {
        tilemap.ClearAllTiles();

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
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


    public struct GameTile
    {
        public TileType type;
        public int plantStage;
        public bool waterable;
        public GameObject topSprite;
        public GameObject speechBubble;

        public GameTile(TileType tileType)
        {
            type = tileType;
            plantStage = 0;
            waterable = false;
            topSprite = null;
            speechBubble = null;
        }

        public void Reset()
        {
            plantStage = 0;
            waterable = false;
            //Destroy(topSprite);
            //Destroy(speechBubble);
        }
    }

    public enum TileType
    {
        Dirt,
        Water,
        Plant
    }
}
