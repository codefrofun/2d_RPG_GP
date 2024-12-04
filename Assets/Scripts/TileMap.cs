using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wallTile;
    public Tile doorTile;
    public Tile chestTile;
    public Tile floorTile;
    public Tile playerTile;
    public Tile enemyTile;

    private Vector3Int playerTilePosition;
    private Vector3Int enemyTilePosition;

    public bool useGeneratedMap = true;
    public bool useTextFileMap = false;

    public string textFileName = "TextFile";
    public string generatedMapName = "GeneratedMap";

    private const char wall = '#';
    private const char door = 'O';
    private const char chest = '$';
    private const char floor = '-';
    private const char player = '@';
    private const char enemy = 'E';

    void Start()
    {
        LoadMap();
    }

    void OnValidate()
    {
        if (useGeneratedMap && useTextFileMap)
        {
            useTextFileMap = false;
        }

        LoadMap();
    }

    public void LoadMap()
    {
        tilemap.ClearAllTiles();
        if (useTextFileMap)
        {
            LoadTextFileMap();
        }
        else if (useGeneratedMap)
        {
            GenerateAndLoadMap();
        }
    }

    void LoadTextFileMap()
    {
        TextAsset mapDataAsset = Resources.Load<TextAsset>(textFileName);

        if (mapDataAsset != null)
        {
            string mapData = mapDataAsset.text;
            ConvertMapToTilemap(mapData);
        }
        else
        {
            Debug.LogError("Text file map not found: " + textFileName);
        }
    }

    void GenerateAndLoadMap()
    {
        string mapData = GenerateMapString(20, 15);
        ConvertMapToTilemap(mapData);
    }

    public string GenerateMapString(int width, int height)
    {
        char[,] map = new char[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[y, x] = wall;
                }
                else
                {
                    map[y, x] = floor;
                }
            }
        }

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if (Random.Range(0, 200) < 25)
                {
                    map[y, x] = wall;
                }
            }
        }

        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                if ((x == 1 || x == width - 2 || y == 1 || y == height - 2) &&
                    ((map[y - 1, x] == wall) || (map[y + 1, x] == wall) || (map[y, x - 1] == wall) || (map[y, x + 1] == wall)) &&
                    Random.Range(0, 100) < 5)
                {
                    map[y, x] = door;
                }
            }
        }


        map[1, width / 2] = player;
        enemyTilePosition = new Vector3Int(width / 2, height / 2, 0);

        int chestsToPlace = Random.Range(1, 5);
        Vector2Int[] corners = new Vector2Int[]
        {
            new Vector2Int(1,1),
            new Vector2Int(width - 2, 1),
            new Vector2Int(1, height  - 2),
            new Vector2Int(width - 2, height - 2)
        };

        for(int i = 0; i < chestsToPlace; i++)
        {
            int randomCornerIndex = Random.Range(0, corners.Length);
            Vector2Int corner = corners[randomCornerIndex];
            map[corner.y, corner.x] = chest;

        }

        string mapString = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapString += map[y, x];
            }
            mapString += "\n";
        }

        return mapString;
    }

    public Vector3Int GetEnemyTilePosition()
    {
        return enemyTilePosition;
    }

    public void SetEnemyTilePosition(Vector3Int newPosition)
    {
        enemyTilePosition = newPosition;
    }

    public void ConvertMapToTilemap(string mapData)
    {
        string[] rows = mapData.Split('\n');
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                char tile = rows[y][x];
                TileBase tileToPlace = null;
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (tile == '#') tileToPlace = wallTile;
                else if (tile == '$') tileToPlace = chestTile;
                else if (tile == 'O') tileToPlace = doorTile;
                else if (tile == '@') tileToPlace = playerTile;
                else if (tile == '-') tileToPlace = floorTile;
                else if (tile == 'E') tileToPlace = enemyTile;

                if (tileToPlace != null)
                {
                    tilemap.SetTile(tilePosition, tileToPlace);

                    if (tile == '@')
                    {
                        playerTilePosition = new Vector3Int(x, y, 0);
                    }
                    else if (tile == 'E') 
                    {
                        enemyTilePosition = new Vector3Int(x, y, 0);
                    }
                }
            }
        }
        tilemap.SetTile(playerTilePosition, playerTile);
        tilemap.SetTile(enemyTilePosition, enemyTile);
    }

    public Vector3Int GetPlayerTilePosition()
    {
        return playerTilePosition;
    }

    public void SetPlayerTilePosition(Vector3Int newPosition)
    {
        playerTilePosition = newPosition;
        tilemap.SetTile(playerTilePosition, playerTile);
    }
}