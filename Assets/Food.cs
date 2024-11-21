using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    public Collider2D spawnArea;
    private Snake snake;
    public bool massBurner;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
    }

    private void Start()
    {
        //RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = spawnArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        // Prevent the food from spawning on the snake
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

        massBurner = false;
        if (snake.GetSnakeSize() > 4)
            massBurner = (Random.Range(1, 10) < 6);

        spriteRenderer.color = (massBurner == true) ? Color.red : Color.green;

        transform.position = new Vector2(x, y);
        int randomTime = Random.Range(4, 8);
        Invoke("RandomizePosition", randomTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CancelInvoke("RandomizePosition");
        RandomizePosition();
    }
}
