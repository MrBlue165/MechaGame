using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public Player(string name, int health, int speed) : base(name, health, speed) 
    {
        // Initialize specific stats for the player, like unique abilities
        abilities.Add(new Ability("Cannon Shot", 20, 5, 0, 0.8)); // Adding an example ability
    }

    // Additional methods specific to the player
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        Debug.Log($"{entityName} healed {amount} health!");
    }

    // Override die method if needed for player-specific behavior
    public override void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
