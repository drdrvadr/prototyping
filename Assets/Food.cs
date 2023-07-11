using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public LevelGrid levelGrid;

    private void Start()
    {
        levelGrid = FindObjectOfType<LevelGrid>();
        Respawn();
    }

    public void Respawn()
    {
        var nextFoodCell = levelGrid.GetRandomCell();
        if (nextFoodCell.GetComponent<Cell>().currentState == Cell.CellState.EMPTY)
        {
            transform.position = nextFoodCell.transform.position;
        }
        else
        {
            Respawn();
        }
        levelGrid.UpdateCellState();
    }
}
