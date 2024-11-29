using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    [SerializeField] private FoodType foodType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private FoodSpawner foodSpawner;

    public FoodType GetFoodType()
    {
        return foodType;
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
        foodSpawner.RandomizePosition();
    }

    public void SetFoodColorAndType(Color color, FoodType FoodType)
    {
        spriteRenderer.color = color;
        foodType = FoodType;
    }
}

public enum FoodType { MassGainer, MassBurner }

