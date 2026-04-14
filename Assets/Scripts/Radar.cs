using UnityEngine;
using UnityEngine.InputSystem;

public class Radar : MonoBehaviour
{
    [SerializeField] GameObject RadarPingPrefab;

    GameManager gameManager;
    [SerializeField]AudioManager audioManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void RadarGO()
    {
        if (gameManager.GetLocking()) return;
        if (gameManager.GetBatteryLevel() <= 0f) return;

        Instantiate(RadarPingPrefab, transform);
        audioManager.PlaySound("radar");
        gameManager.IncreaseBattery(-1f);
    }
}
