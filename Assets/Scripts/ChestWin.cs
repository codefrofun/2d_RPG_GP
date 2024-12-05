using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class ChestWin : MonoBehaviour
{
    private bool isOpened = false;

    public WinManager winManager;

    public Tilemap tilemap;
    public TileBase chestTile;
    public Vector3Int chestTilePosition;

    void Start()
    {
        chestTilePosition = tilemap.WorldToCell(transform.position);
    }

    private void Update()
    {
        Vector3Int playerTilePosition = tilemap.WorldToCell(GameObject.FindGameObjectWithTag("Player").transform.position);
        if (playerTilePosition == chestTilePosition && !isOpened)
        {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.hasKey)
            {
                OpenChest();
                playerInventory.hasKey = false;
            }
        }
    }

    public void OpenChest()
    {
        isOpened = true;
        if (winManager != null)
        {
            winManager.ShowWinMessage();
        }

        tilemap.SetTile(chestTilePosition, null);

        Invoke("ReturnToMainMenu", 2f);
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
