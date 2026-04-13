using UnityEngine;

public class DestroyAfterSecs : MonoBehaviour
{
    [SerializeField] float seconds = 1f;

    void Start()
    {
        Destroy(gameObject, seconds);
    }
}
