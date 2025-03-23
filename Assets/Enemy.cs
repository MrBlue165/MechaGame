using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy(string name, int health, int speed) : base(name, health, speed)
    {
        // Example of enemy abilities
        abilities.Add(new Ability("Bite", 15, 3, 0)); 
    }

    // Override die method for specific enemy behavior
    public override void Die()
    {
        Debug.Log($"{entityName} was defeated!");
        // Additional behavior for enemy death (drop loot, etc.)
    }
}

