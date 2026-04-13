using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private bool monitorLocked = false;

    private float batteryLevel = 100f;

    private float batteryDrainRate = 0f;

    private int screen = 0; //0 = navigation, 1 = map, 2 = medical, 3 = progress

    private bool oxygenOn = true;
    public float oxygenDrainRate = 0.1f;
    public float oxygenRegenRate = 0.2f;
    private float oxygenLevel = 100f;
    public float oxygencost = 0.1f;
    [SerializeField] GameObject OxygenIcon;

    [SerializeField] GameObject navPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject medicalPanel;
    [SerializeField] GameObject progressPanel;
    [SerializeField] GameObject batteryBar;

    public static GameManager instance;
    
    
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
        
        //battery updates
        if (oxygenOn)
        {
            batteryDrainRate = oxygencost;
        }
        else
        {
            batteryDrainRate = 0f;
        }

        batteryLevel -= batteryDrainRate * Time.deltaTime;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);
        batteryBar.transform.localScale = new Vector3(batteryLevel / 100f, 1f, 1f);
        batteryBar.transform.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, batteryLevel / 100f);

        //oxygen updates
        if (oxygenOn)
        {
            oxygenLevel += oxygenRegenRate * Time.deltaTime;
        }
        else
        {
            oxygenLevel -= oxygenDrainRate * Time.deltaTime;
        }
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0f, 100f);
    }

    public float GetBatteryLevel()
    {
        return batteryLevel;
    }

    public void IncreaseBattery(float amount)
    {
        batteryLevel += amount;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);
    }

    public void SetLocking(bool isLocked)
    {
        monitorLocked = isLocked;
    }

    public void switchScreen(int screenNum)
    {
        if (monitorLocked) return;
        
        screen = screenNum;
        navPanel.SetActive(screen == 0);
        mapPanel.SetActive(screen == 1);
        medicalPanel.SetActive(screen == 2);
        progressPanel.SetActive(screen == 3);
    }

    public void ToggleOxygen()
    {
        if (monitorLocked) return;
    
        oxygenOn = !oxygenOn;
        OxygenIcon.SetActive(oxygenOn);
    }
}
