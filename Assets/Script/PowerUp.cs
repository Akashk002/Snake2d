using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color[] powerUpColor;

    private void OnEnable()
    {
        SetPowerUpType();
    }

    public PowerUpType GetPowerType()
    {
        return powerUpType;
    }

    void SetPowerUpType()
    {
        var ranVal = Random.Range(0, powerUpColor.Length);
        powerUpType = (PowerUpType)(ranVal + 1);
        spriteRenderer.color = powerUpColor[ranVal];
    }
}

public enum PowerUpType { None, Shield, ScoreBoost, SpeedUp };
