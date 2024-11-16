using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMap : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wallTile;
    public Tile doorTile;
    public Tile chestTile;
    public Tile floorTile;
    public Tile playerTile;

    private const char wall = '#';
    private const char door = 'O';
    private const char chest = '$';
    private const char floor = '-';
    private const char player = '@';

    public char[,] multidimensionalArray = new char[20, 20];

    private Vector3Int playerTilePosition;
    private bool isMoving = false;

    void Start()
    {
        string pathToMyFile = "Assets/Resources/TextMap/MapTextFile.txt";

        if (System.IO.File.Exists(pathToMyFile))
        {
            string[] myLines = System.IO.File.ReadAllLines(pathToMyFile);
            Debug.Log("Map file successfully loaded.");

            int countHowManyIs = 0;
            for (int y = 0; y < myLines.Length; y++)
            {
                string myLine = myLines[y];
                for (int x = 0; x < myLine.Length; x++)
                {
                    char myChar = myLine[x];
                    if (x < multidimensionalArray.GetLength(0) && y < multidimensionalArray.GetLength(1))
                    {
                        multidimensionalArray[x, y] = myChar;
                    }
                }
            }

            string loadedMap = string.Join("\n", myLines);
            Debug.Log("Loaded Map Data:\n" + loadedMap);

            string mapData = GenerateMapString(20, 15);
            ConvertMapToTilemap(mapData);
        }
        else
        {
            Debug.LogError("Map file not found at: " + pathToMyFile);
        }
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
                    ((map[y - 1, x] == wall) || (map[y + 1, x] == wall) || (map[y, x - 1] == wall) || (map[y, x + 1] == wall)) &&  //checking border, place doors adjacent to them
                    Random.Range(0, 100) < 10)
                {
                    map[y, x] = door;  //place door
                }
            }
        }


        int chestsToPlace = Random.Range(1, 5);
        Vector2Int[] corners = new Vector2Int[] {
            new Vector2Int(1, 1),  //top-left
            new Vector2Int(width - 2, 1),  //top-right
            new Vector2Int(1, height - 2),  //bottom-left
            new Vector2Int(width - 2, height - 2)  //bottom-right
        };

        System.Random rand = new System.Random();
        corners = corners.OrderBy(x => rand.Next()).ToArray();
        for (int i = 0; i < chestsToPlace; i++)
        {
            Vector2Int corner = corners[i];
            map[corner.y, corner.x] = chest;
        }

        map[1, width / 2] = player;

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

    public void ConvertMapToTilemap(string mapData)
    {
        string[] rows = mapData.Split('\n');

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                char tile = rows[y][x];
                TileBase tileToPlace = null;

                tileToPlace = floorTile;
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, tileToPlace);

                if (tile == '#') tileToPlace = wallTile;
                else if (tile == '$') tileToPlace = chestTile;
                else if (tile == 'O') tileToPlace = doorTile;
                else if (tile == '@') tileToPlace = playerTile;

                if (tileToPlace != null)
                {
                    tilemap.SetTile(tilePosition, tileToPlace);

                    if (tile == '@')
                    {
                        playerTilePosition = new Vector3Int(x, y, 0);
                    }
                }
            }
        }
        tilemap.SetTile(playerTilePosition, playerTile);
    }

    public Vector3Int GetPlayerTilePosition()
    {
        return playerTilePosition;
    }

    public void LoadPremadeMap(string Resources)
    {
        TextAsset mapDataAsset = Resources.Load<TextAsset>("TextFileMap/" + Resources);

        if (mapDataAsset != null)
        {
            Debug.Log("Premade map successfully loaded: " + Resources);

            string mapData = mapDataAsset.text; //this gets the text content from the file
            Debug.Log("Loaded map data:\n" + mapData);

            ConvertMapToTilemap(mapData); //this uses the data to display the map
        }
        else
        {
            Debug.LogError("Map file not found in Resources folder: " + Resources);
        }
    }
}
