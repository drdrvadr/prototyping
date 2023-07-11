using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellState
    {
        EMPTY,
        SNAKE,
        FOOD,
        OBSTACLE //Пока не использую
    }

    public CellState currentState = CellState.EMPTY;
}
