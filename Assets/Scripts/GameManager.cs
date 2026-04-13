using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool monitorLocked = false;

    private float batteryLevel = 100f;
    private float batteryDrainRate = 0f;
    [SerializeField] TextMeshProUGUI batteryUsageText;

    private int screen = 0; //0 = navigation, 1 = map, 2 = medical, 3 = progress

    private bool oxygenOn = true;
    public float oxygenDrainRate = 0.1f;
    public float oxygenRegenRate = 0.2f;
    public float oxygenLevel = 100f;
    public float oxygencost = 0.1f;
    [SerializeField] GameObject OxygenIcon;
    [SerializeField] Vignette vignette;
    [SerializeField] ChromaticAberration chromaticAberration;

    [SerializeField] GameObject navPanel;
    [SerializeField] GameObject mapPanel;
    [SerializeField] GameObject medicalPanel;
    [SerializeField] GameObject progressPanel;
    [SerializeField] private EnergyBar batteryBar;
    [SerializeField] private float batteryBarUpdateInterval = 0.5f;
    private float batteryBarTimer = 0f;

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
        batteryBarTimer -= Time.deltaTime;
        if (batteryBarTimer <= 0f)
        {
            batteryBar.SetValue(batteryLevel / 100f);
            batteryBarTimer = batteryBarUpdateInterval;
        }
        batteryUsageText.text = (batteryDrainRate * 60).ToString("F1") + "%/min";

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
        //vignette.smoothness.value = (1f - (oxygenLevel / 100f));
        //chromaticAberration.intensity.value = (1f - (oxygenLevel / 100f)) * .2f;
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

    public void DrainBattery(float amount)
    {
        batteryLevel -= amount;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, 100f);
        batteryBar.SetValue(batteryLevel / 100f);
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
