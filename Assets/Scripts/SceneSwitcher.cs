using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;

    public string endingType = "";

    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
