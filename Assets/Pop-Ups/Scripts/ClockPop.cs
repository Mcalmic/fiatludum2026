using UnityEngine;
using UnityEngine.UI;

public class ClockPop : MonoBehaviour
{
    
    private float angle = 0f;
    public float rotationSpeed = 90f; // degrees per second

    public float successThreshold = 15f; // degrees within which a click is successful

    [SerializeField] GameObject hand;

    [SerializeField] GameObject region1;
    [SerializeField] GameObject region2;
    [SerializeField] GameObject region3;

    float randomAngle1;
    float randomAngle2;
    float randomAngle3;
    
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
        
        randomAngle1 = Random.Range(0f, 360f);
        randomAngle2 = Random.Range(0f, 360f);
        randomAngle3 = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        angle = angle % 360f;

        if (hand != null)
        {
            hand.transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        if (region1Clicked && region2Clicked && region3Clicked)
        {
            //Debug.Log("All regions successfully clicked! Closing pop-up.");
            Destroy(gameObject);
        }

        // hand.GetComponent<Image>().color = Color.red;

        // if (angle >= randomAngle1 - successThreshold && angle <= randomAngle1 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }
        // else if (angle >= randomAngle2 - successThreshold && angle <= randomAngle2 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }
        // else if (angle >= randomAngle3 - successThreshold && angle <= randomAngle3 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }

        //rt.anchoredPosition = new Vector2(testX, testY);
    }

    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            anim.SetTrigger("Clicked");
            region1.SetActive(true);
            region2.SetActive(true);
            region3.SetActive(true);
            hand.SetActive(true);
            region1.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle1);
            region2.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle2);
            region3.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle3);
            return;
        }

        

        if (angle >= (randomAngle1 - successThreshold) % 360f && angle <= (randomAngle1 + successThreshold) % 360f)
        {
            //Debug.Log("Region 1 clicked!");
            region1.SetActive(false);
            region1Clicked = true;
        }
        else if (angle >= (randomAngle2 - successThreshold) % 360f && angle <= (randomAngle2 + successThreshold) % 360f)
        {
            //Debug.Log("Region 2 clicked!");
            region2.SetActive(false);
            region2Clicked = true;
        }
        else if (angle >= (randomAngle3 - successThreshold) % 360f && angle <= (randomAngle3 + successThreshold) % 360f)
        {
            //Debug.Log("Region 3 clicked!");
            region3.SetActive(false);
            region3Clicked = true;
        }
        else
        {
            Debug.Log("Missed! Try again.");
            anim.SetTrigger("Failure");
        }
    }
}
