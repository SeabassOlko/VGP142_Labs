using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerLoadSave))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]Transform weaponAttachPoint;
    [SerializeField]Transform backHolder;
    [SerializeField]Transform hipHolder;

    [SerializeField] GameObject sword;
    [SerializeField] GameObject axe;
    bool holdingSword = false;
    bool holdingAxe = false;
    bool sheathing;

    [SerializeField] ParticleSystem FX_Healing;

    CharacterController playerCharacterController;
    PlayerLoadSave playerLoadSave;

    [SerializeField] Transform lastCheckpoint;
    KillBox killBox;

    public bool paused = false;

    Animator anim;
    
    const int MAX_HEALTH = 10;
    int health;
    HealthBar healthBar;
    const int MAX_Lives = 3;
    int lives;

    [SerializeField] int kickDamage, meleeDamage;

    bool canHit = true;
    bool kicking = false;
    bool swinging = false;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        lives = MAX_Lives;
        healthBar = GetComponent<HealthBar>();
        anim = GetComponent<Animator>();
        playerCharacterController = GetComponent<CharacterController>();
        killBox = GetComponentInChildren<KillBox>();
        playerLoadSave = GetComponent<PlayerLoadSave>();
        InputManager.Instance.playerController = this;
        Cursor.lockState = CursorLockMode.Locked;

        healthBar.UpdateHealth(health, MAX_HEALTH);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;

        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !kicking && !swinging && !sheathing)
            {
                StartCoroutine(Kick());
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !swinging && !kicking && !sheathing)
            {
                StartCoroutine(Swing());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(Death());
            }

            if (Input.GetKeyDown(KeyCode.R) && !swinging && !kicking && !sheathing)
            {
                StartCoroutine(ChangeWeapons());
            }
        }
    }

    IEnumerator Kick()
    {
        kicking = true;
        anim.SetTrigger("Kick");
        yield return new WaitForSeconds(1.0f);
        kicking = false;
    }

    IEnumerator Swing()
    {
        swinging = true;
        anim.SetTrigger("Swing");
        yield return new WaitForSeconds(1.0f);
        swinging = false;
    }

    IEnumerator Death()
    {
        dead = true;
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(4.23f);
        dead = false;   
        playerCharacterController.transform.position = lastCheckpoint.position;
        Physics.SyncTransforms();
        health = MAX_HEALTH;
        healthBar.UpdateHealth(health, MAX_HEALTH);
    }

    IEnumerator ChangeWeapons()
    {
        if (holdingSword || holdingAxe)
        {
            sheathing = true;
            anim.SetTrigger("SheathBack");
            yield return new WaitForSeconds(0.22f);
            if (holdingSword)
            {
                sword.transform.SetPositionAndRotation(backHolder.position, backHolder.rotation);
                sword.transform.SetParent(backHolder);
                holdingSword = false;
                if (axe)
                {
                    holdingAxe = true;
                    axe.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
                    axe.transform.SetParent(weaponAttachPoint);
                }
            }
            else if (holdingAxe)
            {
                axe.transform.SetPositionAndRotation(backHolder.position, backHolder.rotation);
                axe.transform.SetParent(backHolder);
                holdingAxe = false;
                if (sword)
                {
                    holdingSword = true;
                    sword.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
                    sword.transform.SetParent(weaponAttachPoint);
                }
            }
            yield return new WaitForSeconds(1.04f);
            sheathing = false;
        }
    }
    public void Cooldown()
    {
        StartCoroutine(HitCooldown());
    }

    IEnumerator HitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(1.0f);
        canHit = true;
    }

    public void DropWeapon(InputAction.CallbackContext ctx)
    {
        if (holdingSword)
        {
            weaponAttachPoint.DetachChildren();
            Physics.IgnoreCollision(sword.GetComponentInChildren<Collider>(), GetComponentInChildren<Collider>(), false);
            Rigidbody weaponRb = sword.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;
            weaponRb.AddForce(transform.forward * 10, ForceMode.Impulse);
            sword = null;
            holdingSword = false;
        }
        else if (holdingAxe)
        {
            weaponAttachPoint.DetachChildren();
            Physics.IgnoreCollision(axe.GetComponentInChildren<Collider>(), GetComponentInChildren<Collider>(), false);
            Rigidbody weaponRb = axe.GetComponent<Rigidbody>();
            weaponRb.isKinematic = false;
            weaponRb.AddForce(transform.forward * 10, ForceMode.Impulse);
            axe = null;
            holdingAxe = false;
        }
    }

    public void Respawn()
    {
        lives--;
        if (!CheckLives())
            StartCoroutine(Death());
        else
        {
            GameManager.Instance.GameOver();
        }
            
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Weapon") && hit.gameObject.name == "Axe" && !axe)
        {
            AttachWeapon(hit.gameObject, hit.gameObject.name);
        }
        else if (hit.collider.CompareTag("Weapon") && hit.gameObject.name == "Sword" && !sword)
        {
            AttachWeapon(hit.gameObject, hit.gameObject.name);
        }
    }

    public void AttachWeapon(GameObject weapon, string type)
    {
        if (type == "Axe")
        {
            axe = weapon;
            axe.GetComponentInParent<Rigidbody>().isKinematic = true;
            if (!holdingSword)
            {
                holdingAxe = true;
                axe.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
                axe.transform.SetParent(weaponAttachPoint);
            }
            else
            {
                axe.transform.SetPositionAndRotation(backHolder.position, backHolder.rotation);
                axe.transform.SetParent(backHolder);
            }
            Physics.IgnoreCollision(GetComponent<Collider>(), weapon.GetComponent<Collider>());
        }
        else if (type == "Sword")
        {
            sword = weapon;
            sword.GetComponentInParent<Rigidbody>().isKinematic = true;
            if (!holdingAxe)
            {
                holdingSword = true;
                sword.transform.SetPositionAndRotation(weaponAttachPoint.position, weaponAttachPoint.rotation);
                sword.transform.SetParent(weaponAttachPoint);
            }
            else
            {
                sword.transform.SetPositionAndRotation(backHolder.position, backHolder.rotation);
                sword.transform.SetParent(backHolder);
            }
            Physics.IgnoreCollision(GetComponent<Collider>(), weapon.GetComponent<Collider>());
        }
    }

    public void EnemyHit(Collider collision)
    {
        if (collision.CompareTag("Enemy") && canHit)
        {
            if (kicking)
            {
                Cooldown();
                collision.gameObject.GetComponent<Enemy>().hit(kickDamage, "Kick_Death");
            }
            else if (swinging)
            {
                Cooldown();
                collision.gameObject.GetComponent<Enemy>().hit(meleeDamage, "Melee_Death");
            }
        }
    }

    public void Hurt(int damage)
    {
        if (dead) return;

        if (health - damage <= 0)
        {
            health = 0;
            healthBar.UpdateHealth(health, MAX_HEALTH);
            Respawn();
        }
        else
        {
            health -= damage;
            healthBar.UpdateHealth(health, MAX_HEALTH);
        }
        Debug.Log("Health is now: " + health);
    }

    public void Heal(int hp)
    {
        FX_Healing.Play();
        if (health + hp > MAX_HEALTH) 
        {
            health = MAX_HEALTH;
            healthBar.UpdateHealth(health, MAX_HEALTH);
        }
        else
        {
            health += hp;
            healthBar.UpdateHealth(health, MAX_HEALTH);
        }
    }

    public void SetHealth(int hp)
    {
        health = hp;
        healthBar.UpdateHealth(health, MAX_HEALTH);
    }

    public void SetLives(int livesGiven)
    {
        lives = livesGiven;
    }

    private bool CheckLives()
    {
        if (lives > 0)
            return false;
        else 
            return true;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetLives()
    {
        return lives;
    }

    public Transform GetCheckpoint()
    {
        return lastCheckpoint;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
    }

    public bool HasSword()
    {
        return sword;
    }

    public bool HasAxe()
    {
        return axe;
    }
}
