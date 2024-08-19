using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillBox : MonoBehaviour
{
    Enemy enemyController;

    private void Start()
    {
        enemyController = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hitPlayer trigger being called");
            enemyController.hitPlayer(other);
        }
    }

}

