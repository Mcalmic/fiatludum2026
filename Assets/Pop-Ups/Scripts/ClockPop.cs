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
        
        randomAngle1 = Random.Range(0f, 360f);
        randomAngle2 = Random.Range(0f, 360f);
        randomAngle3 = Random.Range(0f, 360f); 

        gameManager = GameManager.instance;
        gameManager.SetLocking(true);

        audioManager = AudioManager.instance;
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
            gameManager.SetLocking(false);
            audioManager.PlaySound("longjingle");

        }

        // hand.GetComponent<Image>().color = Color.red;

        // if (angle >= randomAngle1  && angle <= randomAngle1 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }
        // else if (angle >= randomAngle2  && angle <= randomAngle2 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }
        // else if (angle >= randomAngle3  && angle <= randomAngle3 + successThreshold)
        // {
        //     hand.GetComponent<Image>().color = Color.green;
        // }

        // rt.anchoredPosition = new Vector2(testX, testY);
    }

    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            anim.SetTrigger("Clicked");
            audioManager.PlaySound("jingle");
            
            // Show regions and hand
            region1.SetActive(true);
            region2.SetActive(true);
            region3.SetActive(true);
            hand.SetActive(true);

            // Rotate visual regions to their targets
            region1.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle1);
            region2.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle2);
            region3.transform.localRotation = Quaternion.Euler(0f, 0f, -randomAngle3);
            return;
        }

        // We check the center of the success zone
        // Since your zone starts at randomAngle and goes for successThreshold degrees:
        float offset = successThreshold / 2f;

        if (!region1Clicked && IsInRegion(angle, randomAngle1 + offset))
        {
            region1.SetActive(false);
            region1Clicked = true;
            audioManager.PlaySound("positive");
        }
        else if (!region2Clicked && IsInRegion(angle, randomAngle2 + offset))
        {
            region2.SetActive(false);
            region2Clicked = true;
            audioManager.PlaySound("positive");
        }
        else if (!region3Clicked && IsInRegion(angle, randomAngle3 + offset))
        {
            region3.SetActive(false);
            region3Clicked = true;
            audioManager.PlaySound("positive");
        }
        else
        {
            audioManager.PlaySound("negative");
        }
    }

    private bool IsInRegion(float currentAngle, float targetCenterAngle)
    {
        // Mathf.DeltaAngle returns the shortest difference (e.g., -5 instead of 355)
        float diff = Mathf.DeltaAngle(currentAngle, targetCenterAngle);
        
        // Check if we are within half the threshold of the center
        return Mathf.Abs(diff) <= (successThreshold / 2f);
    }
}
