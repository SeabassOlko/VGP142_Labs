using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    GameObject weapon;
    public Transform weaponAttachPoint;

    [SerializeField] Transform spawn;
    [SerializeField] BoxCollider killBox;

    Animator anim;

    //Rigidbody rb;
    CharacterController cc;

    [Range(0f, 20f)]
    public float speed = 5f;

    private float gravity;
    private Vector2 direction;

    const int MAX_HEALTH = 10;
    public int health;
    HealthBar healthBar;

    [SerializeField] int kickDamage, meleeDamage;

    bool kicking = false;
    bool swinging = false;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        healthBar = GetComponent<HealthBar>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
        InputManager.Instance.controller = this;
        Cursor.lockState = CursorLockMode.Locked;

        healthBar.updateHealth(health, MAX_HEALTH);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !kicking)
            {
                StartCoroutine(kick());
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !swinging)
            {
                StartCoroutine(swing());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(death());
            }
            //float hInput = Input.GetAxis("Horizontal");
            //float vInput = Input.GetAxis("Vertical");

            float hMouse = Input.GetAxis("Mouse X");

            transform.Rotate(Vector3.up * hMouse * 2.5f);

            //transform.Rotate(0, Camera.main.transform.rotation.y, 0);

            //transform.rotation = Camera.main.transform.rotation;

            Vector3 hVel = Camera.main.transform.right * direction.x;
            Vector3 vVel = Camera.main.transform.forward * direction.y;

            hVel.Normalize();
            vVel.Normalize();

            Vector3 moveDirection = new Vector3(hVel.x + vVel.x, 0, hVel.z + vVel.z);

            anim.SetFloat("Speed", direction.magnitude);

            if (moveDirection.magnitude > 0)
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, moveDirection, 0.1f);
            }

            moveDirection *= speed;
            moveDirection.y = gravity;
            moveDirection *= Time.deltaTime;

            cc.Move(moveDirection);

            //rb.velocity = new Vector3(hVel.x + vVel.x, rb.velocity.y, hVel.z + vVel.z);
        }
    }

    IEnumerator kick()
    {
        kicking = true;
        anim.SetTrigger("Kick");
        yield return new WaitForSeconds(1.5f);
        kicking = false;
    }

    IEnumerator swing()
    {
        swinging = true;
        anim.SetTrigger("Swing");
        yield return new WaitForSeconds(2.1f);
        swinging = false;
    }

    IEnumerator death()
    {
        dead = true;
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(4.23f);
        dead = false;
        cc.transform.position = spawn.position;
        Physics.SyncTransforms();
        health = MAX_HEALTH;
        healthBar.updateHealth(health, MAX_HEALTH);
    }

    public void MoveStarted(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
    }

    public void MoveCanceled(InputAction.CallbackContext ctx)
    {
        direction = Vector2.zero;
    }

    public void DropWeapon(InputAction.CallbackContext ctx)
    {
        if (weapon)
        {
            weaponAttachPoint.DetachChildren();
            Physics.IgnoreCollision(weapon.GetComponent<Collider>(), GetComponent<Collider>(), false);
            Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;
            weaponRb.AddForce(transform.forward * 10, ForceMode.Impulse);
            weapon = null;
        }
    }

    public void respawn()
    {
        StartCoroutine(death());
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Weapon") && weapon == null)
        {
            weapon = hit.gameObject;
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
            weapon.transform.SetParent(weaponAttachPoint);
            Physics.IgnoreCollision(GetComponent<Collider>(), hit.collider);
        }
    }

    public void EnemyHit(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (kicking)
                collision.gameObject.GetComponent<EnemyGhost>().hit(kickDamage, "Kick_Death");
            else if (swinging)
                collision.gameObject.GetComponent<EnemyGhost>().hit(meleeDamage, "Melee_Death");
        }
    }

    public void hurt(int damage)
    {
        if (dead) return;

        if (health - damage <= 0)
        {
            health = 0;
            healthBar.updateHealth(health, MAX_HEALTH);
            respawn();
        }
        else
        {
            health -= damage;
            healthBar.updateHealth(health, MAX_HEALTH);
        }
        Debug.Log("Health is now: " + health);
    }

    public void heal(int hp)
    {
        if (health + hp > MAX_HEALTH) 
        {
            health = MAX_HEALTH;
            healthBar.updateHealth(health, MAX_HEALTH);
        }
        else
        {
            health += hp;
            healthBar.updateHealth(health, MAX_HEALTH);
        }
    }
}
