using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] bool followPlayer = true;
    [SerializeField] Transform player;

    private void Update()
    {
        if (followPlayer && Vector3.Distance(transform.position, player.position) <= 20) 
        {
            slider.transform.LookAt(player);
            slider.transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    public void updateHealth(float health, float maxHealth)
    {
        slider.value = health / maxHealth;
    }
}
