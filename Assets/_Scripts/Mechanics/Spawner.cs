using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]Transform[] spawners;

    [SerializeField]GameObject[] pickups;

    private void Start()
    {
        foreach (Transform t in spawners)
        {
            Instantiate(pickups[Random.Range(0, pickups.Length)], t.position, t.rotation);
        }
    }
}
