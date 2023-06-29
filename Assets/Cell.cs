using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellState
    {
        EMPTY,
        SNAKE,
        FOOD,
        OBSTACLE
    }

    public CellState currentState = CellState.EMPTY;

    public void OccupyCell(CellState state)
    {
        if (currentState == CellState.EMPTY)
        {
            currentState = state;
        }
    }
}
