using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class WinManager : MonoBehaviour
{
    public TextMeshProUGUI winText;
    public Tilemap tilemap;
    public TileBase chestTile;
    public TileBase playerTile;
    private Vector3Int playerTilePosition;

    void Start()
    {
        winText.gameObject.SetActive(false);
    }

    void Update()
    {
        playerTilePosition = tilemap.WorldToCell(transform.position);

        if (tilemap.GetTile(playerTilePosition) == chestTile)
        {
            ShowWinMessage();
        }
    }

    public void ShowWinMessage()
    {
        winText.gameObject.SetActive(true);
        winText.text = "You Win!";
        Time.timeScale = 0f;
    }
}