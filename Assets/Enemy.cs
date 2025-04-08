using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    
    
    public Enemy(string name, int health, int speed) : base(name, health, speed)
    {
        // Example of enemy abilities
        abilities.Add(new Ability("Cannon Shot", 5, 5, 0, 0.8));
        abilities.Add(new Ability("Minigun", 4, 6, 2, 1));
        abilities.Add(new Ability("Shield Slam", 20, 2, 0, 1.0)); // Strong basic tank attack
        abilities.Add(new Ability("Fortify", 0, 3, 0, 1.0)); // Defensive, maybe you handle buff elsewhere
        abilities.Add(new Ability("Taunt", 0, 1, 0, 1.0)); // Forces enemy to target tank (logic separate)
        abilities.Add(new Ability("Iron Defense", 0, 4, 0, 1.0)); // Boosts defense
        abilities.Add(new Ability("Body Block", 10, 2, 0, 1.0)); // Minor damage + protect ally
    }
    
    
    
    

    // Override die method for specific enemy behavior
    public override void Die()
    {
        Debug.Log($"{entityName} was defeated!");
        abilities.RemoveRange(0, abilities.Count);
        GetRandomAbilities();

        // Find the player (assumed method, you should define how to find the player)
        Entity player = FindPlayer();
        if (player.currentHealth < player.maxHealth) player.UseAbility(player.abilities.Find(ability => ability.abilityName == "Heal"));
        if (player.currentFuel < player.maxFuel) player.UseAbility(player.abilities.Find(ability => ability.abilityName == "Tank Fuel"));
        // If player is found and chance is met, give player an ability
        if (player != null && player.abilities.Count < 18)
        {
            float randChance = Random.value; // Generates a random float between 0 and 1
            if (randChance <= 0.25f) // 25% chance to give an ability (can adjust the value)
            {
                Ability enemyAbility = abilities[Random.Range(0, abilities.Count)];
                if (!player.abilities.Exists(a => a.abilityName == enemyAbility.abilityName))
                {
                    Debug.Log($"{player.entityName} gained the ability: {enemyAbility.abilityName}");
                    player.abilities.Add(enemyAbility); // Add the ability if it doesn't exist
                }
                else
                {
                    Debug.Log($"{player.entityName} already has the ability: {enemyAbility.abilityName}, not adding again.");
                }
            }
        }
        
        float multiplier = Random.Range(1.0f, 1.5f);
        Transform child = this.transform.GetChild(0);
        child.localScale = new Vector3(0.5f * multiplier, 1.0f * multiplier, this.transform.GetChild(0).localScale.z);
        
        // Get the SpriteRenderer
        SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

// Get the original color



// Change only the hue (random value between 0 and 1)
        float h = Random.Range(0.0f, 1.0f);

// Convert back to RGB
        Color newColor = Color.HSVToRGB(h, 1, 1);
        newColor.a = 1;
// Apply the new color
        sr.color = newColor;
        this.maxHealth = 20;
        this.currentHealth = maxHealth;


        
    }
    private System.Random random = new System.Random();
    public override void UseRandAbility()
    {
        PerformRandomAbility();
    }
    public void PerformRandomAbility()
    {
        if (abilities == null || abilities.Count == 0)
        {
            Debug.LogWarning("No abilities available for this enemy.");
            return;
        }

        int randomIndex = random.Next(abilities.Count);
        Ability selectedAbility = abilities[randomIndex];

        if (currentFuel >= selectedAbility.fuelCost)
        {
            this.UseAbility(selectedAbility, FindPlayer());
            Debug.Log($"{entityName} used {selectedAbility.abilityName}!");
        }
        else
        {
            int fuelToAdd = random.Next(1, maxFuel - currentFuel + 1); // to avoid over max
            currentFuel += fuelToAdd;
            Debug.Log($"{entityName} didn't have enough fuel. Recovered {fuelToAdd} fuel!");
        }
    }
    private Entity FindPlayer()
    {
        // You could search for the player in the scene here, or have a reference.
        // This is just an example of how you might find the player.
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            return playerObject.GetComponent<Entity>();
        }

        return null;
    }
    public void GetRandomAbilities()
    {
        abilities.Add(new Ability("Cannon Shot", 5, 5, 0, 0.8));
        abilities.Add(new Ability("Minigun", 4, 6, 2, 1));
        abilities.Add(new Ability("Shield Slam", 20, 2, 0, 1.0)); // Strong basic tank attack
        abilities.Add(new Ability("Fortify", 0, 3, 0, 1.0)); // Defensive, maybe you handle buff elsewhere
        abilities.Add(new Ability("Taunt", 0, 1, 0, 1.0)); // Forces enemy to target tank (logic separate)
        abilities.Add(new Ability("Iron Defense", 0, 4, 0, 1.0)); // Boosts defense
        abilities.Add(new Ability("Body Block", 10, 2, 0, 1.0)); // Minor damage + protect ally
        
    }

}

