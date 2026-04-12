using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CaptchaPop : MonoBehaviour
{
    [SerializeField] Slider slider1;
    [SerializeField] Slider slider2;
    [SerializeField] Slider slider3;

    public float decreaseAmount = 0.05f;
    public float threshold = .95f;
    
    Animator anim;
    RectTransform rt;
    bool isClicked = false;

    GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Random.Range(0, 650f), Random.Range(-300f, 0f));

        gameManager = GameManager.instance;
        gameManager.SetLocking(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (slider1.value > 0)
        {
            slider1.value -= decreaseAmount * Time.deltaTime;
        }
        if (slider2.value > 0)
        {
            slider2.value -= decreaseAmount * Time.deltaTime;
        }
        if (slider3.value > 0)
        {
            slider3.value -= decreaseAmount * Time.deltaTime;
        }

        if (slider1.value >= threshold && slider2.value >= threshold && slider3.value >= threshold)
        {
            //wait a second before destroying to let the animation play
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        gameManager.SetLocking(false);
    }

    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            anim.SetTrigger("Clicked");
            slider1.gameObject.SetActive(true);
            slider2.gameObject.SetActive(true);
            slider3.gameObject.SetActive(true);
            return;
        }
    }
}
