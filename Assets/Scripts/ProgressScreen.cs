using UnityEngine;
using TMPro;

public class ProgressScreen : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] TextMeshProUGUI distanceText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        distanceText.text = "Distance to Earth: " + Mathf.CeilToInt(gameManager.distanceLeft) + " km";
    }
}
