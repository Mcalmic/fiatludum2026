using UnityEngine;

public class showending : MonoBehaviour
{
    [SerializeField] GameObject monsterEnding;
    [SerializeField] GameObject oxygenEnding;
    [SerializeField] GameObject bholeEnding;
    [SerializeField] GameObject goodEnding;

    [SerializeField] GameObject retryButton;
    
    void Start()
    {
        switch (SceneSwitcher.instance.endingType)
        {
            case "monster":
                monsterEnding.SetActive(true);
                break;
            case "oxygen":
                oxygenEnding.SetActive(true);
                break;
            case "bhole":
                bholeEnding.SetActive(true);
                break;
            case "good":
                goodEnding.SetActive(true);
                break;
        }

        retryButton.SetActive(true);
    }

}
