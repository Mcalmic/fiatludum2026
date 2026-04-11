using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoxPop : MonoBehaviour
{
    private int location1 = 0;
    private int location2 = 0;

    public bool item1Found = false;
    public bool item2Found = false;

    [SerializeField] List<ClickyBox> boxes;
    
    bool isClicked = false;
    bool region1Clicked = false;
    bool region2Clicked = false;
    bool region3Clicked = false;

    Animator anim;
    RectTransform rt;

    // public float testX = 0f;
    // public float testY = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Random.Range(0, 650f), Random.Range(-300f, 0f));
        
        location1 = Random.Range(0, boxes.Count);
        location2 = Random.Range(0, boxes.Count);
        while (location2 == location1)
        {
            location2 = Random.Range(0, boxes.Count);
        }



        Debug.Log("Locations: " + location1 + ", " + location2);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (item1Found && item2Found)
        {
            //wait a second before destroying to let the animation play
            StartCoroutine(DestroyAfterDelay());
        }

        //rt.anchoredPosition = new Vector2(testX, testY);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            anim.SetTrigger("Clicked");
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].gameObject.SetActive(true);
                if (i == location1)
                {
                    boxes[i].hasItem = "one";
                }
                else if (i == location2)
                {
                    boxes[i].hasItem = "two";
                }
                else
                {
                    boxes[i].hasItem = "none";
                }
            }
            return;
        }
    }
}
