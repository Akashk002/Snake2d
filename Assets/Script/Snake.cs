using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private SnakeType snakeType;
    [SerializeField] private Vector2 intialPosition;
    [SerializeField] private Transform snakePart;
    [SerializeField] private float speed = 20f;
    [SerializeField] private int snakeSize = 4;
    [SerializeField] private int powerUpCoolDownTime = 6;
    [SerializeField] private PowerUpType currenPowerUpType;
    [SerializeField] private Food food;
    [SerializeField] private PowerUpController powerUpController;
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameOverController gameOverController;

    Vector2Int direction = Vector2Int.right;
    List<Transform> segments = new List<Transform>();
    float nextUpdate;

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        ChangeDirection();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time < nextUpdate)
        {
            return;
        }

        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }


        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        nextUpdate = Time.time + (1f / speed);
    }

    public void Grow()
    {
        Transform segment = Instantiate(snakePart);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void Shrink()
    {
        Destroy(segments[segments.Count - 1].gameObject);
        segments.RemoveAt(segments.Count - 1);
    }

    public void ResetState()
    {
        direction = Vector2Int.right;
        transform.position = intialPosition;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < snakeSize - 1; i++)
        {
            Grow();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Food food = other.GetComponent<Food>();
        PowerUp powerUp = other.GetComponent<PowerUp>();

        if (food)
        {
            if (food.GetFoodType() == FoodType.MassBurner)
            {
                AudioManager.Instance.Play(SoundType.CollectMassBurner);
                scoreController.UpdateScore(-1, snakeType);
                Shrink();
            }
            else
            {
                AudioManager.Instance.Play(SoundType.CollectMassGainer);
                int valAdd = (currenPowerUpType == PowerUpType.ScoreBoost) ? 2 : 1;
                scoreController.UpdateScore(valAdd, snakeType);
                Grow();
            }
        }
        else
        if (powerUp)
        {
            AudioManager.Instance.Play(SoundType.CollectPowerUp);

            currenPowerUpType = powerUp.GetPowerType();

            if (currenPowerUpType == PowerUpType.SpeedUp)
            {
                speed *= 2;
            }

            scoreController.UpdatePowerUp(currenPowerUpType, snakeType);

            powerUpController.DisablePowerUp();
            Invoke(nameof(DisablePowerUp), powerUpCoolDownTime);
        }
        else
        if (other.gameObject.CompareTag("Wall"))
        {
            Traverse(other.transform);
        }
        else
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play(SoundType.CollideWithTail);
            gameOverController.GameOver(snakeType, true);
        }
        else
        if (currenPowerUpType != PowerUpType.Shield && (snakeType == SnakeType.Snake1 && other.gameObject.CompareTag("Obstacle") || snakeType == SnakeType.Snake2 && other.gameObject.CompareTag("Obstacle2")))
        {
            AudioManager.Instance.Play(SoundType.CollideWithTail);
            gameOverController.GameOver(snakeType);
        }
        else
        if (snakeType == SnakeType.Snake2 && other.gameObject.CompareTag("Obstacle") || snakeType == SnakeType.Snake1 && other.gameObject.CompareTag("Obstacle2"))
        {
            AudioManager.Instance.Play(SoundType.CollideWithTail);
            gameOverController.GameOver(snakeType);
        }
    }

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y)
            {
                return true;
            }
        }

        return false;
    }


    void ChangeDirection()
    {
        if (direction.x != 0f)
        {
            if (snakeType == SnakeType.Snake1 && Input.GetKeyDown(KeyCode.W) || (snakeType == SnakeType.Snake2 && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                direction = Vector2Int.up;
            }
            else if (snakeType == SnakeType.Snake1 && Input.GetKeyDown(KeyCode.S) || (snakeType == SnakeType.Snake2 && Input.GetKeyDown(KeyCode.DownArrow)))
            {
                direction = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (snakeType == SnakeType.Snake1 && Input.GetKeyDown(KeyCode.D) || (snakeType == SnakeType.Snake2 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                direction = Vector2Int.right;
            }
            else if (snakeType == SnakeType.Snake1 && Input.GetKeyDown(KeyCode.A) || (snakeType == SnakeType.Snake2 && Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                direction = Vector2Int.left;
            }
        }
    }

    void Traverse(Transform wall)
    {

        if (direction.x != 0)
        {
            transform.position = new Vector2(-wall.position.x + direction.x, transform.position.y);
        }
        else
        if (direction.y != 0)
        {
            transform.position = new Vector2(transform.position.x, -wall.position.y + direction.y);
        }

    }

    public int GetSnakeSize()
    {
        return segments.Count;
    }

    void DisablePowerUp()
    {
        if (currenPowerUpType == PowerUpType.SpeedUp)
        {
            speed *= 0.5f;
        }
        currenPowerUpType = PowerUpType.None;
        scoreController.UpdatePowerUp(currenPowerUpType, snakeType);
    }

}

public enum SnakeType { Snake1, Snake2 }