using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    Inputs inputAction;
    public PlayerController controller;

    protected override void Awake()
    {
        base.Awake();
        inputAction = new Inputs();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.controls.Move.performed += controller.MoveStarted;
        inputAction.controls.Move.canceled += controller.MoveCanceled;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.controls.Move.performed -= controller.MoveStarted;
        inputAction.controls.Move.canceled -= controller.MoveCanceled;
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
