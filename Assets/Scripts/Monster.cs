using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Monster : MonoBehaviour
{
    [SerializeField] List<Location> possiblePoints;
    [SerializeField] Location currentLocation;
    
    public float moveInterval = 5f;
    public float attackInterval = 10f;

    Image monsterImage;

    GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.instance;
        monsterImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackInterval <= 0)
        {
            
            attackInterval = Random.Range(8f, 12f);
        }
        else
        {
            attackInterval -= Time.deltaTime;
        }

        if (moveInterval <= 0)
        {
            UpdateLocation();
            moveInterval = Random.Range(3f, 7f);
        }
        else
        {
            moveInterval -= Time.deltaTime;
        }
    }

    void UpdateLocation()
    {
        if (currentLocation == null) return;

        int distance = Random.Range(1, 4);

        for (int i = 0; i < distance; i++)
        {
            // Check if the current location actually has neighbors to move to
            if (currentLocation.neighbors != null && currentLocation.neighbors.Count > 0)
            {
                // Pick a random Location from the neighbors list
                int randomIndex = Random.Range(0, currentLocation.neighbors.Count);
                currentLocation = currentLocation.neighbors[randomIndex];
            }
        }

        // Physically move the monster to the new Location's transform
        transform.position = currentLocation.pointTransform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Reveal());
    }

    IEnumerator Reveal()
    {
        monsterImage.enabled = true;
        yield return new WaitForSeconds(2f);
        monsterImage.enabled = false;
    }
}
