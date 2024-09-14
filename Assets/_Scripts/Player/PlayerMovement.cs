using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const string speedAnimationTrigger = "Speed";
    CharacterController playerCharacterController;

    [Range(0f, 20f)]
    [SerializeField] float moveSpeed = 5f;
    [Range(0f, 10f)]
    [SerializeField] float mouseLookSpeed = 2.5f;
    private float gravity;
    private Vector2 direction;

    Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        gravity = Physics.gravity.y;
        playerCharacterController = GetComponent<CharacterController>();
        InputManager.Instance.playerMovement = this;
    }

    // Update is called once per frame
    void Update()
    {
        float hMouse = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * hMouse * mouseLookSpeed);

        Vector3 hVel = Camera.main.transform.right * direction.x;
        Vector3 vVel = Camera.main.transform.forward * direction.y;

        hVel.Normalize();
        vVel.Normalize();

        Vector3 moveDirection = new Vector3(hVel.x + vVel.x, 0, hVel.z + vVel.z);

        playerAnimator.SetFloat(speedAnimationTrigger, direction.magnitude);

        moveDirection *= moveSpeed;
        moveDirection.y = gravity;
        moveDirection *= Time.deltaTime;

        playerCharacterController.Move(moveDirection);
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
