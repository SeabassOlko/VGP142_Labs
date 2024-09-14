using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CanvasManager : MonoBehaviour
{
    public UnityEvent saveEvent = new UnityEvent();
    public UnityEvent loadEvent = new UnityEvent();

    [Header("Button")]
    public Button quitButton;
    public Button playButton;
    public Button returnToMenu;
    public Button resumeButton;
    public Button saveButton;
    public Button loadButton;
    public Button menuLoadButton;

    [Header("Menus")]
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);
        if (playButton)
            playButton.onClick.AddListener(delegate { loadScene("Level1"); });
        if (resumeButton)
            resumeButton.onClick.AddListener(delegate {
                Cursor.lockState = CursorLockMode.Locked;
                PlayerController player = FindAnyObjectByType<PlayerController>();
                player.paused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            });
        if (returnToMenu)
            returnToMenu.onClick.AddListener(delegate {
                Time.timeScale = 1.0f;
                loadScene("MainMenu");
            });
        if (saveButton)
            saveButton.onClick.AddListener(delegate {
                saveEvent.Invoke();
            });
        if (loadButton)
            loadButton.onClick.AddListener(delegate {
                GameManager.Instance.LoadGame();
                Cursor.lockState = CursorLockMode.Locked;
                PlayerController player = FindAnyObjectByType<PlayerController>();
                player.paused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
                loadEvent.Invoke();
            });
        if (menuLoadButton)
            menuLoadButton.onClick.AddListener(delegate
            {
                GameManager.Instance.SetLoad();
                loadScene("Level1");
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!pauseMenu) return;

            if (pauseMenu.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.None;
                PlayerController player = FindAnyObjectByType<PlayerController>();
                player.paused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                PlayerController player = FindAnyObjectByType<PlayerController>();
                player.paused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Quit()
    {
        saveEvent.RemoveAllListeners();
        loadEvent.RemoveAllListeners();
        GameManager.Instance.checkpoint.RemoveAllListeners();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
