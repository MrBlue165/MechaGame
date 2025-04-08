using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public string entityName;
    public int maxHealth;
    public int currentHealth;
    public int maxFuel;
    public int currentFuel;
    public List<Ability> abilities; // A list of abilities for the entity
    public int speed; // Determines how quickly the entity acts (priority in turn-based combat)
    
    // Constructor to initialize basic stats
    public Entity(string name, int health, int speed)
    {
        entityName = name;
        maxHealth = health;
        currentHealth = health;
        abilities = new List<Ability>();
        this.speed = speed;
        
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); // Handle death logic (e.g., remove from combat, etc.)
        }
    }

    // Method for death logic
    public virtual void Die()
    {
        // Placeholder for death behavior
        Debug.Log($"{entityName} has died!");
    }

    // Method to use ability
    public virtual void UseAbility(Ability ability, Entity target)
    {
        // Use ability on target (can be modified for different abilities)
        if (currentFuel >= ability.fuelCost)
        {
            currentFuel -= ability.fuelCost;
            ability.Execute(this, target);
            
        }
        else Debug.Log("Not enough fuel!");
    }

    public virtual void UseAbility(Ability ability)
    {
        if (currentFuel >= ability.fuelCost)
        {
            currentFuel -= ability.fuelCost;
            ability.Execute(this);
        }
    }

    public virtual void UseRandAbility()
    {
        
    }
}