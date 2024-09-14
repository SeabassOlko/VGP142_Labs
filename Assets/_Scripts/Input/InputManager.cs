using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    Inputs inputAction;
    public PlayerMovement playerMovement;
    public PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        inputAction = new Inputs();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.controls.Move.performed += playerMovement.MoveStarted;
        inputAction.controls.Move.canceled += playerMovement.MoveCanceled;
        inputAction.controls.Drop.performed += playerController.DropWeapon;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.controls.Move.performed -= playerMovement.MoveStarted;
        inputAction.controls.Move.canceled -= playerMovement.MoveCanceled;
        inputAction.controls.Drop.performed -= playerController.DropWeapon;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
