using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnviroLiquid : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    bool damaging = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !damaging)
        {
            other.GetComponent<PlayerController>().Hurt(damage);
            StartCoroutine(damageCooldown());
        }
    }

    IEnumerator damageCooldown()
    {
        damaging = true;
        yield return new WaitForSeconds (1f);
        damaging = false;
    }
}
