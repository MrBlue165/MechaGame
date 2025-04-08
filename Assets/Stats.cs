using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    
    GameObject player;
    GameObject enemy;
    
    [SerializeField] GameObject playerHealthBar;
    [SerializeField] GameObject playerFuelBar;
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI playerFuel;

    [SerializeField] GameObject enemyHealthBar;
    [SerializeField] GameObject enemyFuelBar;
    [SerializeField] TextMeshProUGUI enemyHealth;
    [SerializeField] TextMeshProUGUI enemyFuel;
    
    Entity playerEntity;
    Entity enemyEntity;
    // Start is called before the first frame update
    void Start()
    {
        // Find child objects named Obj1 and Obj2
        Transform obj1 = transform.Find("PlayerStats");
        Transform obj2 = transform.Find("EnemyStats");
        
        // Find Player and Enemy by tag
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        
        playerEntity = player.GetComponent<Entity>();
        enemyEntity = enemy.GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerHealthRatio = (float)playerEntity.currentHealth / playerEntity.maxHealth;
        float playerFuelRatio = (float)playerEntity.currentFuel / playerEntity.maxFuel;
        playerHealth.text =$"HP: {playerEntity.currentHealth} / {playerEntity.maxHealth}";
        playerFuel.text =$"F: {playerEntity.currentFuel} / {playerEntity.maxFuel}";
        playerHealthBar.transform.GetChild(0).localScale = new Vector3(playerHealthRatio, playerHealthBar.transform.GetChild(0).localScale.y, playerHealthBar.transform.GetChild(0).localScale.z);
        playerFuelBar.transform.GetChild(0).localScale = new Vector3(playerFuelRatio,playerFuelBar.transform.GetChild(0).localScale.y, playerFuelBar.transform.GetChild(0).localScale.z);
        
        
        // Calculate ratios for Enemy
        float enemyHealthRatio = (float)enemyEntity.currentHealth / enemyEntity.maxHealth;
        float enemyFuelRatio = (float)enemyEntity.currentFuel / enemyEntity.maxFuel;
        enemyHealth.text = $"HP: {enemyEntity.currentHealth} / {enemyEntity.maxHealth}";
        enemyFuel.text = $"F: {enemyEntity.currentFuel} / {enemyEntity.maxFuel}";
        enemyHealthBar.transform.GetChild(0).localScale = new Vector3(enemyHealthRatio, enemyHealthBar.transform.GetChild(0).localScale.y, enemyHealthBar.transform.GetChild(0).localScale.z);
        enemyFuelBar.transform.GetChild(0).localScale = new Vector3(enemyFuelRatio,enemyFuelBar.transform.GetChild(0).localScale.y, enemyFuelBar.transform.GetChild(0).localScale.z);
        
    }
}
