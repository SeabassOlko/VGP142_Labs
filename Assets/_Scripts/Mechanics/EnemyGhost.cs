using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] GameObject projectile;

    [SerializeField]SkinnedMeshRenderer ren;

    [SerializeField]Animator anim;

    bool isDead = false;

    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.P)) 
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
}
