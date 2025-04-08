using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Ability
{
    public string abilityName;
    public int power; // How much damage this ability does (can be expanded for more complex effects)
    public int fuelCost;
    public int cooldown; // Number of turns before ability can be used again

    // Constructor to initialize ability
    public Ability(string name, int power, int fuelCost, int cooldown, double chance)
    {
        abilityName = name;
        this.power = power;
        this.fuelCost = fuelCost;
        this.cooldown = cooldown;
    }
    

// Method to execute ability on the target
    public void Execute(Entity user, Entity target)
    {
        if (user is Player) // Example condition if abilities differ for player or enemy
        {
            Debug.Log($"{user.entityName} used {abilityName} on {target.entityName}!");
        }
        target.TakeDamage(power);
    }

    public void Execute(Entity user)
    {
        user.TakeDamage(power);
    }
}