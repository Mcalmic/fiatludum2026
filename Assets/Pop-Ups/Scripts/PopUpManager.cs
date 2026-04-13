using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour
{
    
    [SerializeField] List<GameObject> popUpPrefabs;
    [SerializeField] GameObject oxygenStopPrefab;
    [SerializeField] GameObject oxygenWarningPrefab;
    [SerializeField] GameObject shieldStoppedPrefab;
    [SerializeField] GameObject autoPilotStoppedPrefab;

    GameManager gameManager;

    public static PopUpManager instance;

    //scales over time, set by GameManager
    public int popUpIntensity = 1;
    [SerializeField] private float baseMinDelay = 20f;
    [SerializeField] private float baseMaxDelay = 60f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
        StartCoroutine(OxygenWarningLoop());
        StartCoroutine(PopUpLoop());

        instance = this;
    }

    IEnumerator OxygenWarningLoop()
    {
        while (true)
        {
            if (gameManager.oxygenLevel <= 50f)
            {
                Instantiate(oxygenWarningPrefab, transform);
            }            
            yield return new WaitForSeconds(10f); // Check every 5 seconds
        }
    }

    public void SpawnOxygenStop()
    {
        Instantiate(oxygenStopPrefab, transform);
    }

    public void SpawnShieldStopped()
    {
        Instantiate(shieldStoppedPrefab, transform);
    }

    public void SpawnAutoPilotStopped()
    {
        Instantiate(autoPilotStoppedPrefab, transform);
    }

    IEnumerator PopUpLoop()
    {
        while (true)
        {
            // Calculate delay based on intensity (higher intensity = lower delay)
            float min = baseMinDelay / Mathf.Max(1, popUpIntensity);
            float max = baseMaxDelay / Mathf.Max(1, popUpIntensity);
            
            float waitTime = Random.Range(min, max);
            yield return new WaitForSeconds(waitTime);

            SpawnPopUp();
        }
    }

    public void SpawnPopUp()
    {
        if (popUpPrefabs == null || popUpPrefabs.Count == 0) return;

        int index = Random.Range(0, popUpPrefabs.Count);
        GameObject prefabToSpawn = popUpPrefabs[index];

        Instantiate(prefabToSpawn, transform);
    }
}
