using UnityEngine;

public class ClickyBox : MonoBehaviour
{
    
    public string hasItem = "none";

    private bool isOpen = false;
    
    Animator anim;

    AudioManager audioManager;
    [SerializeField] BoxPop parentPopUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        audioManager = AudioManager.instance;
    }

    public void OpenBox()
    {
        if (isOpen) return;
        isOpen = true;

        parentPopUp.boxesOpened++;
        
        if (hasItem == "none")
        {
            //Debug.Log("You found: " + hasItem);
            anim.SetTrigger("Open");
            audioManager.PlaySound("negative");
            hasItem = "empty";
        }
        else
        {
            //Debug.Log("You found: " + hasItem);
            anim.SetTrigger("Has");
            audioManager.PlaySound("positive");
        }

        parentPopUp.CheckForWin();
    }
}
