using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private int disableTime;
    [SerializeField] private Snake snake, snake2;
    [SerializeField] private Food food;
    [SerializeField] private PowerUp powerUp;
    [SerializeField] private int maxTime = 15, minTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartPowerUpTimer();
    }
    void StartPowerUpTimer()
    {
        int randomTime = Random.Range(minTime, maxTime);
        Invoke(nameof(EnablePowerUp), randomTime);
    }

    void EnablePowerUp()
    {
        powerUp.gameObject.SetActive(true);
        RandomizePosition();
        Invoke(nameof(DisablePowerUp), disableTime);
    }

    public void DisablePowerUp()
    {
        powerUp.gameObject.SetActive(false);
        StartPowerUpTimer();
        CancelInvoke(nameof(DisablePowerUp));
    }

    public void RandomizePosition()
    {
        Bounds bounds = spawnArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        while (snake.Occupies(x, y) || food.Occupies(x, y) || (snake2 && snake2.Occupies(x, y)))
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
        powerUp.transform.position = new Vector2(x, y);
    }
}
