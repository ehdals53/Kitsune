using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    // The Animator component attached to the boss
    private Animator animator;

    // The NavMeshAgent component attached to the boss
    private NavMeshAgent navMeshAgent;
    
    // The player character
    public GameObject player;

    // The distance at which the boss will start chasing the player
    public float chaseDistance = 10f;

    // The distance at which the boss will attack the player
    public float attackDistance = 5f;

    // The amount of time the boss will wait between attacks
    public float attackDelay = 2f;

    // The time of the last attack
    private float lastAttackTime = 0f;

    public float maxHealth = 100f;
    private float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator and NavMeshAgent components from the boss GameObject
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the boss and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        navMeshAgent.updateRotation = true;

        
        // Check if the boss is within the chase distance
        if (distance <= chaseDistance)
        {
            navMeshAgent.isStopped = false;
            animator.SetBool("isMove", true);
            
            // Set the destination of the NavMeshAgent to the player's position
            navMeshAgent.SetDestination(player.transform.position);

            // Check if the boss is within the attack distance
            if (distance <= attackDistance)
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity  = Vector3.zero;

                animator.SetBool("isMove", false);

                // Check if the attack delay has elapsed
                if (Time.time > lastAttackTime + attackDelay)
                {
                    navMeshAgent.updateRotation = false;
                    // Attack the player
                    animator.CrossFadeInFixedTime("Attack", 0.1f);
                    
                    //animator.SetTrigger("Attack");
                    Debug.Log("Attack");
                        
                    // Update the time of the last attack
                    lastAttackTime = Time.time;
                }
            }
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Damage : " + damage);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Die");
        }
    }
}