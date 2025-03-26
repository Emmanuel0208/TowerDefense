using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

[System.Serializable]
public class TowerData
{
    public GameObject prefab; // Prefab de la torre
    public int cost;          // Costo de la torre
    public TextMeshProUGUI priceText; // Texto para mostrar el precio
}

public class TowerBuilder : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerData redTower;
    public TowerData blueTower;
    public TowerData pinkTower;
    public TowerData greenTower;

    [Header("Tilemap")]
    public Tilemap buildableTilemap;

    [Header("Other References")]
    public GameManager gameManager; // Referencia al GameManager

    [Header("Placed Towers")]
    public List<GameObject> placedTowers = new List<GameObject>();

    private TowerData selectedTower;

    void Start()
    {
        // Asignar precios automáticamente en la UI
        redTower.priceText.text = "$" + redTower.cost;
        blueTower.priceText.text = "$" + blueTower.cost;
        pinkTower.priceText.text = "$" + pinkTower.cost;
        greenTower.priceText.text = "$" + greenTower.cost;
    }

    void Update()
    {
        HandleTowerSelection();
        HandleTowerPlacement();
    }

    void HandleTowerSelection()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            selectedTower = redTower;
        if (Input.GetKeyDown(KeyCode.W))
            selectedTower = blueTower;
        if (Input.GetKeyDown(KeyCode.E))
            selectedTower = pinkTower;
        if (Input.GetKeyDown(KeyCode.R))
            selectedTower = greenTower;
    }

    void HandleTowerPlacement()
    {
        if (Input.GetMouseButtonDown(0) && selectedTower != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = buildableTilemap.WorldToCell(mouseWorldPos);
            TileBase tile = buildableTilemap.GetTile(cellPos);

            if (tile != null)
            {
                // Verificamos si tiene suficiente dinero
                if (gameManager.SpendMoney(selectedTower.cost))
                {
                    Vector3 spawnPos = buildableTilemap.GetCellCenterWorld(cellPos);
                    GameObject tower = Instantiate(selectedTower.prefab, spawnPos, Quaternion.identity);
                    placedTowers.Add(tower);

                    // Quitamos el tile para que no se pueda construir de nuevo
                    buildableTilemap.SetTile(cellPos, null);
                }
                else
                {
                    Debug.Log("Dinero insuficiente para esta torre.");
                }
            }
        }
    }
}
