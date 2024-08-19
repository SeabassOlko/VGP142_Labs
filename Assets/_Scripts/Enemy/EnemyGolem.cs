using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGolem : Enemy
{
    [SerializeField] int damage = 5;

    HealthBar healthBar;

    EnemyMovement movement;
    EnemyKillBox killBox;

    [SerializeField]Transform player;

    bool canSwing = true;
    bool swinging = false;
    bool canHit = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        killBox = GetComponentInChildren<EnemyKillBox>();
        healthBar = GetComponent<HealthBar>();
        movement = GetComponent<EnemyMovement>();
        healthBar.updateHealth(health, MAX_HEALTH);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; 

        if (Vector3.Distance(transform.position, player.position) <= 15f)
        {
            movement.chase();
            if (Vector3.Distance(transform.position, player.position) <= 2.0f && canSwing)
            {
                StartCoroutine(swing());
            }
        }
        else
            movement.patrol();

        anim.SetFloat("Speed", transform.TransformDirection(transform.position).magnitude);
    }

    IEnumerator swing()
    {
        canSwing = false;
        movement.stopMove();
        anim.SetTrigger("Swing");
        yield return new WaitForSeconds(0.2f);
        swinging = true;
        yield return new WaitForSeconds(0.3f);
        swinging = false;
        yield return new WaitForSeconds(2.0f);
        movement.startMove();
        canSwing = true;
    }

    public override void hit(int damage, string damageType)
    {
        base.hit(damage, damageType);

        if (health <= 0)
        {
            death();
        }
        else
        {
            healthBar.updateHealth(health, MAX_HEALTH);
        }
    }

    public override void hitPlayer(Collider player)
    {
        base.hitPlayer(player);
        if (swinging && canHit)
        {
            cooldown();
            player.gameObject.GetComponent<PlayerController>().hurt(damage);
        }
    }

    public void cooldown()
    {
        StartCoroutine(hitCooldown());
    }

    IEnumerator hitCooldown()
    {
        canHit = false;
        yield return new WaitForSeconds(1.0f);
        canHit = true;
    }

    void death()
    {
        health = 0;
        healthBar.updateHealth(health, MAX_HEALTH);
        isDead = true;
        movement.isDead = isDead;
        movement.stopMove();
        anim.SetTrigger("Death");
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Instantiate(drop, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }
}
