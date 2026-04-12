using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool monitorLocked = false;

    private float batteryLevel = 100f;

    private float batteryDrainRate = 0f;

    public static GameManager instance;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //instance = this;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void SetLocking(bool isLocked)
    {
        monitorLocked = isLocked;
    }
}
