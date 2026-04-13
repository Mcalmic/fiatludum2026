using UnityEngine;
using UnityEngine.InputSystem;

public class Radar : MonoBehaviour
{
    [SerializeField] GameObject RadarPingPrefab;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void RadarGO()
    {
        //spawn ping at 0 0 

        if (gameManager.GetLocking()) return;

        Instantiate(RadarPingPrefab, transform);
        gameManager.IncreaseBattery(-1f);

    }
}
