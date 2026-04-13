using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoxPop : MonoBehaviour
{
    private int location1 = 0;
    private int location2 = 0;
    private int location3 = 0;

    public int boxesOpened = 0;

    [SerializeField] List<ClickyBox> boxes;
    
    bool isClicked = false;

    Animator anim;
    RectTransform rt;

    GameManager gameManager;
    AudioManager audioManager;

    // public float testX = 0f;
    // public float testY = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Random.Range(37, 609f), Random.Range(-277f, -23f));
        
        location1 = Random.Range(0, boxes.Count);
        location2 = Random.Range(0, boxes.Count);
        location3 = Random.Range(0, boxes.Count);
        while (location2 == location1)
        {
            location2 = Random.Range(0, boxes.Count);
        }
        while (location3 == location1 || location3 == location2)
        {
            location3 = Random.Range(0, boxes.Count);
        }



        gameManager = GameManager.instance;
        gameManager.SetLocking(true);

        audioManager = AudioManager.instance;

        //Debug.Log("Locations: " + location1 + ", " + location2 + ", " + location3);
        
    }

    // Update is called once per frame
    void Update()
    {
        //rt.anchoredPosition = new Vector2(testX, testY);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        gameManager.SetLocking(false);
        audioManager.PlaySound("longjingle");
    }

    public void OnClick()
    {
        if (isClicked) return;
        
        isClicked = true;
        anim.SetTrigger("Clicked");
        audioManager.PlaySound("jingle");

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].gameObject.SetActive(true);
            
            if (i == location1 || i == location2 || i == location3)
            {
                boxes[i].hasItem = "full"; 
            }
            else
            {
                boxes[i].hasItem = "none";
            }
        }
    }

    public void CheckForWin()
    {
        // Only run the check if all boxes are full
        if (boxesOpened < 9) return;

        bool won = false;

        for (int i = 0; i < boxes.Count; i++)
        {
            string currentType = boxes[i].hasItem;
            if (currentType == "empty") continue;

            // Check Horizontal (Indices 0, 3, 6)
            if (i % 3 == 0)
            {
                if (boxes[i + 1].hasItem == currentType && boxes[i + 2].hasItem == currentType)
                {
                    won = true;
                }
            }

            // Check Vertical (Indices 0, 1, 2)
            if (i < 3)
            {
                if (boxes[i + 3].hasItem == currentType && boxes[i + 6].hasItem == currentType)
                {
                    won = true;
                }
            }

            // Check Diagonal Right Down (Index 0)
            if (i == 0)
            {
                if (boxes[4].hasItem == currentType && boxes[8].hasItem == currentType)
                {
                    won = true;
                }
            }

            // Check Diagonal Left Down (Index 2)
            if (i == 2)
            {
                if (boxes[4].hasItem == currentType && boxes[6].hasItem == currentType)
                {
                    won = true;
                }
            }

            if (won) break; 
        }

        if (won)
        {
            GameManager.instance.IncreaseBattery(5f);
        }

        // Always destroy the pop-up once 9 boxes are opened, win or lose
        StartCoroutine(DestroyAfterDelay());
    }
}
