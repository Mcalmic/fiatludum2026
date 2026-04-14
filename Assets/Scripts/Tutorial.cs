using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    private int currentIndex = 0;

    void Start()
    {
        ShowPanel(0);
    }

    public void NextPanel()
    {
        currentIndex++;
        if (currentIndex < panels.Length)
            ShowPanel(currentIndex);
        else
            gameObject.SetActive(false);
    }

    private void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == index);
    }
}
