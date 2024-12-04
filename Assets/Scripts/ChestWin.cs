using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

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
        if (playerTilePosition == chestTilePosition)
        {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (playerInventory != null && playerInventory.hasKey && !isOpened)
            {
                OpenChest();
                playerInventory.hasKey = false;
            }
        }
    }
    private void OpenChest()
    {
        isOpened = true;
        if (winManager != null)
        {
            winManager.ShowWinMessage();
        }
    }
}
