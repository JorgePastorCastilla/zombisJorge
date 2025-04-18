using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{

    public GameObject player;
    public Animator enemyAnimator;
    public GameManager gameManager;
    
    public float damage = 20f;
    public float maxHealth = 100f;
    public float health;

    public Image HpBar;
    
    // Animacio i millora del xoc
    public bool playerInReach = false;
    public float attackDelayTimer = 0f;
    public float howMuchEarlierStartAttackAnimation = 1f; // inicialitzarem a 1f
    public float delayBetweenAttacks = 0.6f; // inicialitzarem a 0.6f


    private GameObject[] playersInScene;
    
    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //audio va aqui
        
        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            return;
        }

        GetClosestPlayer();
        if (player != null)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        // En primer lloc hem d'accedir a la velocitat del Zombiem, des del component NavMeshAgent
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }

    }
    
    // Detectar la col·lisió
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            playerInReach = true;
            // player.GetComponent<PlayerManager>().Hit(damage);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= delayBetweenAttacks - howMuchEarlierStartAttackAnimation && attackDelayTimer <= delayBetweenAttacks )
            {
                enemyAnimator.SetTrigger("isAttacking");
            }

            if (attackDelayTimer >= delayBetweenAttacks)
            {
                player.GetComponent<PlayerManager>().Hit(damage);
                attackDelayTimer = 0;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == player)
        {
            playerInReach = false;
            attackDelayTimer = 0;
        }
    }

    public void Hit(float damage)
    {
        photonView.RPC("TakeDamage", RpcTarget.All, damage, photonView.ViewID);
    }

    
    [PunRPC]
    public void TakeDamage(float damage, int viewID)
    {
        if (photonView.ViewID == viewID)
        {
            health -= damage;
            Debug.Log("Enemy got hit!");
            HpBar.fillAmount = health / maxHealth;
            if (health <= 0)
            {
                // Destrium a l'enemic quan la seva salut arriba a zero
                // feim referència a ell amb la variable gameObject, que fa referència al GO
                // que conté el componentn EnemyManager
                // Destroy(gameObject);
                enemyAnimator.SetTrigger("isDead");
                Destroy(gameObject,10f);
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(GetComponent<EnemyManager>());
                // Destroy(GetComponent<CapsuleCollider>());
                foreach (var capsuleCollider in gameObject.GetComponents<CapsuleCollider>())
                {
                    Destroy(capsuleCollider);
                }

                if (!PhotonNetwork.InRoom || ( PhotonNetwork.IsMasterClient && photonView.IsMine) )
                {
                    gameManager.enemiesAlive--;
                }
            }
        }
    }

    private void GetClosestPlayer()
    {
        float mindistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject p in playersInScene)
        {
            if (p != null)
            {
                float distance = Vector3.Distance(p.transform.position, currentPosition);
                if (distance < mindistance)
                {
                    player = p;
                    mindistance = distance;
                }
            }
        }
    }

}
