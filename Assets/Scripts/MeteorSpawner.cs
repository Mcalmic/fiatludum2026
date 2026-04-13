using UnityEngine;
using System.Collections.Generic;

public class MeteorSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private List<GameObject> meteorPrefabs = new List<GameObject>();

    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaOffset = new Vector2(15f, 15f);
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(20f, 20f);

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int maxMeteors = 50;

    private List<Meteor> activeMeteors = new List<Meteor>();
    private float timer;
    private int currentCount;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }

    }

    [Header("Meteor Motion")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float predictionTime = 2f;

    private void TrySpawn()
    {
        if (currentCount >= maxMeteors || meteorPrefabs.Count == 0) return;

        Vector2 center = (Vector2)player.position + spawnAreaOffset;
        Vector2 spawnPos = center + new Vector2(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );

        GameObject prefab = meteorPrefabs[Random.Range(0, meteorPrefabs.Count)];
        GameObject meteorObj = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        Meteor meteor = meteorObj.GetComponent<Meteor>();
        Vector2 predictedPos = (Vector2)player.position + (playerRb != null ? playerRb.linearVelocity * predictionTime : Vector2.zero);
        Vector2 toPlayer = (predictedPos - spawnPos).normalized;
        Vector2 velocity = toPlayer * Random.Range(minSpeed, maxSpeed);
        meteor.Initialize(this, velocity);
        activeMeteors.Add(meteor);
        currentCount++;
    }

    public void OnMeteorDestroyed(Meteor meteor)
    {
        activeMeteors.Remove(meteor);
        currentCount--;
    }

    public List<Meteor> GetActiveMeteors()
    {
        return activeMeteors;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Vector2 center = (Vector2)player.position + spawnAreaOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(center, spawnAreaSize);
    }
}
