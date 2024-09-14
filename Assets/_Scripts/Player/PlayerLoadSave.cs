using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerLoadSave : MonoBehaviour
{
    private void Start()
    {
        CanvasManager canvasButtons = FindAnyObjectByType<CanvasManager>();
        canvasButtons.saveEvent.AddListener(SaveGamePrepare);
        canvasButtons.loadEvent.AddListener(LoadGameComplete);
        GameManager.Instance.checkpoint.AddListener(SaveGamePrepare);

        if (GameManager.Instance.GetLoad())
        {
            LoadGameComplete();
        }
    }
    private void SaveGamePrepare()
    {
        // Get Player Data Object
        LoadSaveManager.GameStateData.DataPlayer data = LoadSaveManager.Instance.gameStateData.player;
        PlayerController player = GetComponent<PlayerController>();

        // Fill in player data for save game
        data.hasSword = player.HasSword();
        data.hasAxe = player.HasAxe();
        data.health = player.GetHealth();
        data.lives = player.GetLives();

        Transform checkpoint = player.GetCheckpoint();

        data.checkpointPosRotScale.posX = checkpoint.position.x;
        data.checkpointPosRotScale.posY = checkpoint.position.y;
        data.checkpointPosRotScale.posZ = checkpoint.position.z;
        data.checkpointPosRotScale.rotX = checkpoint.rotation.x;
        data.checkpointPosRotScale.rotY = checkpoint.rotation.y;
        data.checkpointPosRotScale.rotZ = checkpoint.rotation.z;
        data.checkpointPosRotScale.scaleX = checkpoint.localScale.x;
        data.checkpointPosRotScale.scaleY = checkpoint.localScale.y;
        data.checkpointPosRotScale.ScaleZ = checkpoint.localScale.z;

        GameManager.Instance.SaveGame();
    }

    // Function called when loading is complete
    private void LoadGameComplete()
    {
        GameManager.Instance.LoadGame();

        // Get Player Data Object
        LoadSaveManager.GameStateData.DataPlayer data = LoadSaveManager.Instance.gameStateData.player;
        PlayerController player = GetComponent<PlayerController>();


        //Load data back to Player
        player.SetHealth(data.health);
        player.SetLives(data.lives);

        //Give player weapon, activate and destroy weapon power-up
        if (data.hasAxe)
        {
            //Find weapon in level
            GameObject weapon = GameObject.Find("Axe");

            //Call attach function
            player.AttachWeapon(weapon, weapon.name);
        }
        if (data.hasSword)
        {
            //Find weapon in level
            GameObject weapon = GameObject.Find("Sword");

            //Call attach function
            player.AttachWeapon(weapon, weapon.name);
        }

        //Set position
        transform.position = new Vector3(data.checkpointPosRotScale.posX, data.checkpointPosRotScale.posY, data.checkpointPosRotScale.posZ);

        //Set rotation
        transform.rotation = Quaternion.Euler(data.checkpointPosRotScale.rotX, data.checkpointPosRotScale.rotY, data.checkpointPosRotScale.rotZ);

        //Set scale
        transform.localScale = new Vector3(data.checkpointPosRotScale.scaleX, data.checkpointPosRotScale.scaleY, data.checkpointPosRotScale.ScaleZ);
    }
}
