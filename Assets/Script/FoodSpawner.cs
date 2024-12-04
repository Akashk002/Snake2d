using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private Snake snake, snake2;
    [SerializeField] private Food food;
    [SerializeField] private int maxTime = 8, minTime = 4;

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        CancelInvoke(nameof(RandomizePosition));

        Bounds bounds = spawnArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));


        while (snake.Occupies(x, y) || (snake2 && snake2.Occupies(x, y)))
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

        FoodType foodType = (snake.GetSnakeSize() > 4 && Random.Range(0, 10) <= 2 && snake2 == null) ? FoodType.MassBurner : FoodType.MassGainer;

        Color color = (foodType == FoodType.MassBurner) ? Color.red : Color.green;

        food.SetFoodColorAndType(color, foodType);

        food.transform.position = new Vector2(x, y);
        int randomTime = Random.Range(4, 8);
        Invoke(nameof(RandomizePosition), randomTime);
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
        CancelInvoke(nameof(RandomizePosition));
        RandomizePosition();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RandomizePosition));
    }
}
