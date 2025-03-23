using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Player(string name, int health, int speed) : base(name, health, speed) 
    {
        // Initialize specific stats for the player, like unique abilities
        abilities.Add(new Ability("Fireball", 20, 5, 0)); // Adding an example ability
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
        Debug.Log($"{entityName} has been defeated!");
        // Game over logic or respawn logic
    }
}
