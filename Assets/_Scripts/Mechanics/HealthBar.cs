using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void updateHealth(float health, float maxHealth)
    {
        slider.value = health / maxHealth;
    }
}
