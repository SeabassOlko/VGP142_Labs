using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickupType
    {
        RedPickup,
        BluePickup,
        GreenPickup
    }

    [SerializeField] private PickupType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case PickupType.RedPickup:
                    try
                    {
                        if (other.gameObject.GetComponent<PlayerController>().GetHealth() < 0) throw new Exception();
                        else
                            other.gameObject.GetComponent<PlayerController>().Heal(4);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.Message + " Health not implemented on player yet");
                    }
                    break;
                case PickupType.BluePickup:
                    break;
                case PickupType.GreenPickup:
                    break;
            }
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        switch (type) 
    //        {
    //            case PickupType.RedPickup: 
    //                Debug.Log("Red hit");
    //                try
    //                {
    //                    if (collision.gameObject.GetComponent<PlayerController>().health == false) throw new Exception();
    //                    else
    //                        collision.gameObject.GetComponent<PlayerController>().health = true;
    //                        //subtract from health
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.Log(e.Message);
    //                }
    //                break;
    //            case PickupType.BluePickup:
    //                Debug.Log("Blue hit");
    //                break;
    //            case PickupType.GreenPickup:
    //                Debug.Log("Green hit");
    //                break;
    //        }
    //        Destroy(gameObject);
    //    }
    //}
}
