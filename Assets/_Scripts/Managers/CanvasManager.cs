using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{

    public Button quitButton;
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);
        if (playButton)
            playButton.onClick.AddListener(delegate { loadScene("Level1"); });
    }

    void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
