using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public PowerUp powerUp;
    public int disableTime;

    // Start is called before the first frame update
    void Start()
    {
        StartPowerUpTimer();
    }
    void StartPowerUpTimer()
    {
        int randomTime = Random.Range(10, 15);
        Invoke("EnablePowerUp", randomTime);
    }

    void EnablePowerUp()
    {
        powerUp.gameObject.SetActive(true);
        Invoke("DisablePowerUp", disableTime);
    }

    public void DisablePowerUp()
    {
        powerUp.gameObject.SetActive(false);
        StartPowerUpTimer();
        CancelInvoke("DisablePowerUp");
    }
}
