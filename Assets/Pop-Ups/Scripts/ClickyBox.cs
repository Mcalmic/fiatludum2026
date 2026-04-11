using UnityEngine;

public class ClickyBox : MonoBehaviour
{
    
    public string hasItem = "none";
    
    Animator anim;
    [SerializeField] BoxPop parentPopUp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenBox()
    {
        if (hasItem == "none")
        {
            //Debug.Log("You found: " + hasItem);
            anim.SetTrigger("Open");
            hasItem = "empty";
            return;
        }
        else if (hasItem == "one")
        {
            //Debug.Log("You found: " + hasItem);
            anim.SetTrigger("Has");
            hasItem = "empty";
            parentPopUp.item1Found = true;
            return;
        }
        else if (hasItem == "two")
        {
            //Debug.Log("You found: " + hasItem);
            anim.SetTrigger("Has");
            hasItem = "empty";
            parentPopUp.item2Found = true;
            return;
        }
    }
}
