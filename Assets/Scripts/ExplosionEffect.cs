using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float timeWait = 1f;

    void Start()
    {
        Destroy(gameObject, timeWait);
    }

    void Update()
    {
    }
}
