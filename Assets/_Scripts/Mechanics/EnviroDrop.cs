using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class EnviroDrop : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] float time;
    bool falling = false;
    float sPos;
    Rigidbody rb;

    private void Start()
    {
        sPos = transform.position.y;
        rb = GetComponent<Rigidbody>();
        drop();
    }

    void drop()
    {
        StartCoroutine(fall());
    }

    private void Update()
    {
        if (!falling)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (transform.position.y >= sPos)
            drop();
    }

    IEnumerator fall()
    {
        rb.useGravity = true;
        falling = true;
        yield return new WaitForSeconds(time);
        falling = false;
        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().hurt(damage);
            StopCoroutine(fall());
            falling = false;
            rb.useGravity = false;
        }
    }
}
