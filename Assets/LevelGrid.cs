using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private List<Transform> snake;
    private Food food;
    public GameObject cellPrefab;
    public GameObject[,] cellMatrix;
    public int dimentionX = 16;
    public int dimentionY = 9;

    private void Awake()
    {
        CreateCellMatrix();
        snake = FindObjectOfType<Snake>().GetSnake();
        food = FindObjectOfType<Food>();
    }

    private void CreateCellMatrix()
    {
        cellMatrix = new GameObject[dimentionX, dimentionY];
        for (int i = 0; i < dimentionX; i++)
        {
            for (int j = 0; j < dimentionY; j++)
            {
                var cell = Instantiate(
                    cellPrefab,
                    new Vector3(i, j, 0),
                    Quaternion.identity,
                    transform
                );
                cell.name = $"Cell {i}:{j}";
                cellMatrix[i, j] = cell;
            }
        }
    }

    public GameObject GetRandomCell()
    {
        return cellMatrix[Random.Range(0, dimentionX), Random.Range(0, dimentionY)];
    }

    public bool GetNextCell(Vector3 currentPosition, Vector2 direction, out GameObject result)
    {
        result = null;
        var nextX = Mathf.RoundToInt(currentPosition.x + direction.x);
        var nextY = Mathf.RoundToInt(currentPosition.y + direction.y);
        if (nextX > dimentionX - 1 || nextX < 0)
        {
            return false;
        }
        if (nextY > dimentionY - 1 || nextY < 0)
        {
            return false;
        }

        result = cellMatrix[nextX, nextY];
        return true;

        //использовать vector3int вместо раунд то инт.
        //возврат ретёрн фолс когда выходим за пределы массива
    }

    public void UpdateCellState()
    {
        foreach (var cell in cellMatrix)
        {
            var currentState = cell.GetComponent<Cell>().currentState;
            if (currentState != Cell.CellState.OBSTACLE)
            {
                cell.GetComponent<Cell>().currentState = Cell.CellState.EMPTY;
            }
        }

        foreach (var segment in snake)
        {
            var segmentPosition = segment.position;
            var cell = cellMatrix[
                Mathf.RoundToInt(segmentPosition.x),
                Mathf.RoundToInt(segmentPosition.y)
            ];
            cell.GetComponent<Cell>().currentState = Cell.CellState.SNAKE;
        }

        var foodPosition = food.transform.position;
        var foodCell = cellMatrix[
            Mathf.RoundToInt(foodPosition.x),
            Mathf.RoundToInt(foodPosition.y)
        ];
        foodCell.GetComponent<Cell>().currentState = Cell.CellState.FOOD;
    }
}
