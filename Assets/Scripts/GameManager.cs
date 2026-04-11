using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private float batteryLevel = 100f;

    private float batteryDrainRate = 0f;


    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        batteryLevel -= batteryDrainRate * Time.deltaTime;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);
    }

    public float GetBatteryLevel()
    {
        return batteryLevel;
    }
}
