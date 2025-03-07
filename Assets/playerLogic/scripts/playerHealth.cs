using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;  // Asignar desde el Inspector
    public LoseScreenManager loseScreenManager;  // Asegurar que está declarado y asignado

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        if(currentHealth>=maxHealth){
            currentHealth = 100;
        }
    }

    void Die()
    {
        if (loseScreenManager != null)
        {
            loseScreenManager.ShowLoseScreen();
        }
        else
        {
            Debug.LogError("LoseScreenManager no asignado en el Inspector.");
        }

        gameObject.SetActive(false); 
    }
}
