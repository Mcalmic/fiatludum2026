using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour
{
    
    [SerializeField] List<GameObject> popUpPrefabs;
    [SerializeField] GameObject oxygenPopUpPrefab;

    //scales over time, set by GameManager
    public int popUpIntensity = 1;
    [SerializeField] private float baseMinDelay = 20f;
    [SerializeField] private float baseMaxDelay = 60f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PopUpLoop());
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
