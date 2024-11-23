using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public bool Snake2;
    public Transform snakePart;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int snakeSize = 4;
    public PowerUpType currenPowerUpType;
    public bool moveThroughWalls = false;
    public Food food;

    Vector2 IntialPosition;
    Vector2Int direction = Vector2Int.right;
    List<Transform> segments = new List<Transform>();
    float nextUpdate;

    private void OnEnable()
    {
        IntialPosition = transform.position;
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

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
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
        transform.position = IntialPosition;

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
        if (other.gameObject.CompareTag("Food"))
        {
            if (other.GetComponent<Food>())
            {
                Food food = other.GetComponent<Food>();

                if (food.massBurner)
                {
                    GameManager.Instance.UpdateScore(-1, Snake2);
                    Shrink();
                }
                else
                {
                    int valAdd = (currenPowerUpType == PowerUpType.ScoreBoost) ? 2 : 1;
                    GameManager.Instance.UpdateScore(valAdd, Snake2);
                    Grow();
                }
            }
        }
        else
        if (other.gameObject.CompareTag("PowerUp"))
        {
            if (other.GetComponent<PowerUp>())
            {
                currenPowerUpType = other.GetComponent<PowerUp>().powerUpType;

                if (currenPowerUpType == PowerUpType.SpeedUp)
                {
                    speed *= 2;
                }

                GameManager.Instance.UpdatePowerUp(currenPowerUpType, Snake2);

                GameManager.Instance.DisablePowerUp();
                Invoke("DisablePowerUp", GameManager.Instance.powerUpCoolDownTime);
            }
        }
        else
        if (currenPowerUpType != PowerUpType.Shield && (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Obstacle2")))
        {
            GameManager.Instance.GameOver();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls)
            {
                Traverse(other.transform);
            }
            else
            {
                ResetState();
            }
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
            if (!Snake2 && Input.GetKeyDown(KeyCode.W) || (Snake2 && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                direction = Vector2Int.up;
            }
            else if (!Snake2 && Input.GetKeyDown(KeyCode.S) || (Snake2 && Input.GetKeyDown(KeyCode.DownArrow)))
            {
                direction = Vector2Int.down;
            }
        }
        else if (direction.y != 0f)
        {
            if (!Snake2 && Input.GetKeyDown(KeyCode.D) || (Snake2 && Input.GetKeyDown(KeyCode.RightArrow)))
            {
                direction = Vector2Int.right;
            }
            else if (!Snake2 && Input.GetKeyDown(KeyCode.A) || (Snake2 && Input.GetKeyDown(KeyCode.LeftArrow)))
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
        GameManager.Instance.UpdatePowerUp(currenPowerUpType, Snake2);
    }

}
