using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; set; }
    public RectTransform healthbar;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthbar.sizeDelta = new Vector2((float)currentHealth / maxHealth * 100.0f, healthbar.sizeDelta.y);
    }
}
