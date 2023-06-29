using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{
    [SerializeField]
    private GameObject snakePrefab;
    private TextMeshPro scoreText;
    private int score = 0;
    public LevelGrid levelGrid;
    public float moveCooldown = 0.3f;
    public int startLength = 3;

    private List<Transform> snake = new List<Transform>();
    private GameObject food;
    private GameObject nextCell;
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 requestedDirection = Vector2.zero;

    private Vector3 headPosition;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshPro>();
        food = FindObjectOfType<Food>().gameObject;
        levelGrid = FindObjectOfType<LevelGrid>();
        var startCell = levelGrid.GetRandomCell();

        transform.position = startCell.transform.position;
        headPosition = startCell.transform.position;

        snake.Add(transform);
        for (int i = 0; i < startLength; i++)
        {
            Grow();
        }
        Debug.Log($"В змейке сейчас {snake.Count} сегмента!");
        levelGrid.UpdateCellState();
        InvokeRepeating(nameof(Move), 0, moveCooldown);
    }

    public List<Transform> GetSnake()
    {
        return snake;
    }

    private void Grow()
    {
        var snakeSegment = Instantiate(snakePrefab, transform.parent);
        snakeSegment.transform.position = snake[snake.Count - 1].position;
        snake.Add(snakeSegment.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && currentDirection != Vector2.down)
        {
            requestedDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentDirection != Vector2.up)
        {
            requestedDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentDirection != Vector2.left)
        {
            requestedDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentDirection != Vector2.right)
        {
            requestedDirection = Vector2.left;
        }
    }

    private bool IsValidDirection(Vector2 dir)
    {
        return (dir + currentDirection) != Vector2.zero;
    }

    private void Move()
    {
        if (IsValidDirection(requestedDirection))
        {
            currentDirection = requestedDirection;
        }
        nextCell = levelGrid.GetNextCell(headPosition, currentDirection);
        var nextPosition = nextCell.transform.position;

        for (int i = snake.Count - 1; i > 0; i--)
        {
            snake[i].position = snake[i - 1].position;
        }
        snake[0].position = nextPosition;
        headPosition = nextPosition;
        TryCollectFood();
        levelGrid.UpdateCellState();
    }

    private void TryCollectFood()
    {
        if (nextCell.GetComponent<Cell>().currentState == Cell.CellState.FOOD)
        {
            Grow();
            score++;
            UpdateScoreText();
            food.GetComponent<Food>().Respawn();
            nextCell.GetComponent<Cell>().currentState = Cell.CellState.EMPTY;
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
