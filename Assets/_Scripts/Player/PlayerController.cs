using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Rigidbody rb;
    CharacterController cc;

    [Range(0f, 20f)]
    public float speed = 5f;

    private float gravity;
    private Vector2 direction;

    public bool health = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
        InputManager.Instance.controller = this;
    }

    // Update is called once per frame
    void Update()
    {
        //float hInput = Input.GetAxis("Horizontal");
        //float vInput = Input.GetAxis("Vertical");

        float hMouse = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * hMouse * 2.5f);

        Vector3 hVel = Camera.main.transform.right * direction.x;
        Vector3 vVel = Camera.main.transform.forward * direction.y;

        Vector3 moveDirection = new Vector3(hVel.x + vVel.x, 0, hVel.z + vVel.z);
        moveDirection *= speed;
        moveDirection.y = gravity;
        moveDirection *= Time.deltaTime;

        cc.Move(moveDirection);

        //rb.velocity = new Vector3(hVel.x + vVel.x, rb.velocity.y, hVel.z + vVel.z);
    }

    public void MoveStarted(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void MoveCanceled(InputAction.CallbackContext ctx)
    {
        direction = Vector2.zero;
    }
}
