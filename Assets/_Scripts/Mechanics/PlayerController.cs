using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [Range(0f, 20f)]
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        float hMouse = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * hMouse * 2.5f);

        Vector3 hVel = Camera.main.transform.right * hInput * speed;
        Vector3 vVel = Camera.main.transform.forward * vInput * speed;

        rb.velocity = new Vector3(hVel.x + vVel.x, rb.velocity.y, hVel.z + vVel.z);
    }
}
