using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralPopUp : MonoBehaviour
{
    bool isClicked = false;

    RectTransform rt;

    GameManager gameManager;
    AudioManager audioManager;

    public float testX = 0f;
    public float testY = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Random.Range(30, 590f), Random.Range(-375f, -10f));

        gameManager = GameManager.instance;
        gameManager.SetLocking(true);
        
    }

    void Update()
    {
        rt.anchoredPosition = new Vector2(testX, testY);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        gameManager.SetLocking(false);
    }

    public void OnClick()
    {
        if (isClicked) return;
        
        isClicked = true;

        StartCoroutine(DestroyAfterDelay());
    }

}
