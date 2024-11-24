using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PowerUp : MonoBehaviour
{
    public Collider2D spawnArea;
    public Snake snake, snake2;
    private Food food;
    public PowerUpType powerUpType;
    public SpriteRenderer spriteRenderer;
    public Color[] powerUpColor;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
    }

    private void OnEnable()
    {
        RandomizePosition();
    }

    void SetPowerUpType()
    {
        var ranVal = Random.Range(0, 3);
        powerUpType = (PowerUpType)(ranVal + 1);
        spriteRenderer.color = powerUpColor[ranVal];
    }

    public void RandomizePosition()
    {
        SetPowerUpType();

        Bounds bounds = spawnArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        if (snake2)
        {
            while (snake.Occupies(x, y) && food.Occupies(x, y) && snake2.Occupies(x, y))
            {
                x++;

                if (x > bounds.max.x)
                {
                    x = Mathf.RoundToInt(bounds.min.x);
                    y++;

                    if (y > bounds.max.y)
                    {
                        y = Mathf.RoundToInt(bounds.min.y);
                    }
                }
            }
        }
        else
        {
            // Prevent the food from spawning on the snake
            while (snake.Occupies(x, y) && food.Occupies(x, y))
            {
                x++;

                if (x > bounds.max.x)
                {
                    x = Mathf.RoundToInt(bounds.min.x);
                    y++;

                    if (y > bounds.max.y)
                    {
                        y = Mathf.RoundToInt(bounds.min.y);
                    }
                }
            }
        }
        transform.position = new Vector2(x, y);
    }
}

public enum PowerUpType { None, Shield, ScoreBoost, SpeedUp };
