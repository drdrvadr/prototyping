using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private float moveCooldown = 0.3f;

    [SerializeField]
    private int startLength = 3;

    private TextMeshPro scoreText;
    private int score = 0;
    private LevelGrid levelGrid;
    private List<Transform> snake = new List<Transform>();
    private GameObject food;
    private Vector2 currentDirection = Vector2.right;
    private Vector2 requestedDirection = Vector2.right;

    private Vector3 headPosition;

    private void Awake()
    {
        scoreText = GetComponentInChildren<TextMeshPro>();
        levelGrid = FindObjectOfType<LevelGrid>();
        food = FindObjectOfType<Food>().gameObject;
        //то что можно прокинуть руками- лучше прокинуть руками_
    }

    private void Start()
    {
        var startCell = levelGrid.GetRandomCell();

        transform.position = startCell.transform.position;
        headPosition = startCell.transform.position;
        snake.Add(transform);
        for (int i = 0; i < Mathf.Max(0, startLength - 1); i++)
        {
            var snakeSegment = Instantiate(snakePrefab, transform.parent);
            snakeSegment.transform.position = startCell.transform.position;
            snake.Add(snakeSegment.transform);
            Move();
        }
        InvokeRepeating(nameof(Move), 0, moveCooldown);
        Debug.Log($"В змейке сейчас {snake.Count} сегмента!");
        levelGrid.UpdateCellState();

        //переписать на апдейт.
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
            currentDirection = requestedDirection;

        if (levelGrid.GetNextCell(headPosition, currentDirection, out var nextCell))
        {
            if (nextCell.GetComponent<Cell>().currentState != CellState.SNAKE)
            {
                var nextPosition = nextCell.transform.position;

                for (int i = snake.Count - 1; i > 0; i--)
                {
                    snake[i].position = snake[i - 1].position;
                }
                snake[0].position = nextPosition;
                headPosition = nextPosition;
                TryCollectFood(nextCell);
                levelGrid.UpdateCellState();
            }
            else
            {
                Debug.Log("Столкнулся сам с собой");
                Reset();
            }
        }
        else
        {
            Debug.Log("Выход за рамки");
            Reset();
        }
    }

    private void TryCollectFood(GameObject nextCell)
    {
        if (nextCell.GetComponent<Cell>().currentState == CellState.FOOD)
        {
            Grow();
            score++;
            UpdateScoreText();
            food.GetComponent<Food>().Respawn();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private void Reset()
    {
        SceneManager.LoadScene(0);
    }
}
