using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGhost : Enemy
{
    [SerializeField] Transform player;

    [SerializeField] GameObject projectile;

    [SerializeField]SkinnedMeshRenderer ren;

    HealthBar healthBar;

    bool canShoot = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        health = MAX_HEALTH;
        healthBar = GetComponent<HealthBar>();
        healthBar.updateHealth(health,MAX_HEALTH);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.K)) 
        {
            death("Shot_Death");
        }

        if (Vector3.Distance(transform.position, player.position) <= 10f)
        {
            transform.LookAt(player.position);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

            Vector3 lookDir = transform.position - player.position;
            float lookAngle = Vector3.Angle(player.forward, lookDir);

            if (lookAngle <= 8)
            {
                ren.enabled = false;
            }
            else if (!ren.enabled)
            {
                ren.enabled = true;
            }

            if (ren.enabled && canShoot)
            {
                StartCoroutine(shoot());
            }
        }
    }

    public void death(string deathType)
    {
        ren.enabled = true;
        isDead = true;
        anim.SetTrigger(deathType);
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Instantiate(drop, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator shoot()
    {
        canShoot = false;
        anim.SetTrigger("Throw");
        yield return new WaitForSeconds(0.43f);
        Instantiate(projectile, transform.position + new Vector3(0, 0.75f, 0.5f), transform.rotation);
        yield return new WaitForSeconds(4f);
        canShoot = true;
    }

    public override void hit(int damage, string damageType)
    {
        base.hit(damage, damageType);

        if (health <= 0)
        {
            health = 0;
            healthBar.updateHealth(health, MAX_HEALTH);
            death(damageType);
        }
        else
        {
            healthBar.updateHealth(health, MAX_HEALTH);
        }
    }
}
