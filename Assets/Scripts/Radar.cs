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
        //spawn ping at 0 0 

        if (gameManager.GetLocking()) return;

        Instantiate(RadarPingPrefab, transform);
        audioManager.PlaySound("radar");
        gameManager.IncreaseBattery(-1f);

    }
}
