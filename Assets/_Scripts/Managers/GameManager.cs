using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public UnityEvent checkpoint = new UnityEvent();

    private bool loading = false;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndLevel");
    }

    public void CheckpointHit()
    {
        checkpoint.Invoke();
    }

    public void SaveGame()
    {
        LoadSaveManager.Instance.Save();
    }

    public void LoadGame()
    {
        LoadSaveManager.Instance.Load();
    }

    public bool GetLoad()
    {
        return loading;
    }

    public void SetLoad()
    {
        loading = true;
    }

    public void CancelLoad()
    {
        loading = false;
    }
}
