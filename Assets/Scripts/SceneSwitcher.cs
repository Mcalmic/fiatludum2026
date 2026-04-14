using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public string endingType = "";

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
