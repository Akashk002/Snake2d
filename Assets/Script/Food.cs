using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    public Collider2D spawnArea;
    public Snake snake, snake2;
    public bool massBurner;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
    }

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = spawnArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));



        if (snake2)
        {
            while (snake.Occupies(x, y) && snake2.Occupies(x, y))
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
            while (snake.Occupies(x, y))
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

        massBurner = false;
        if (snake.GetSnakeSize() > 4)
            massBurner = (Random.Range(0, 10) <= 2);

        spriteRenderer.color = (massBurner == true) ? Color.red : Color.green;

        transform.position = new Vector2(x, y);
        int randomTime = Random.Range(4, 8);
        Invoke("RandomizePosition", randomTime);
    }

    public bool Occupies(int x, int y)
    {
        if (Mathf.RoundToInt(transform.position.x) == x &&
            Mathf.RoundToInt(transform.position.y) == y)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke("RandomizePosition");
        RandomizePosition();
    }

    private void OnDisable()
    {
        CancelInvoke("RandomizePosition");
    }
}
