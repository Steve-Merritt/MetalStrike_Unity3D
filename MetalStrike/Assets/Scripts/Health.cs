using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
    public RectTransform healthbar;
    public Tank OwningTank;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OwningTank.Die();
        }

        healthbar.sizeDelta = new Vector2(currentHealth, healthbar.sizeDelta.y);
    }
}
