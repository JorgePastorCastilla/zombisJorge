using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        health -= damage;
        
        gameManager.HpBar.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            gameManager.GameOver();
        }
        
    }
}
