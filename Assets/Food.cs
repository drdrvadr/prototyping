using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    public LevelGrid levelGrid;

    private void Start()
    {
        levelGrid = FindObjectOfType<LevelGrid>();
        Respawn();
    }

    public void Respawn()
    {
        var nextFoodCell = levelGrid.GetRandomCell();
        transform.position = nextFoodCell.transform.position;
        nextFoodCell.GetComponent<Cell>().OccupyCell(Cell.CellState.FOOD);
        levelGrid.UpdateCellState();
    }
}
