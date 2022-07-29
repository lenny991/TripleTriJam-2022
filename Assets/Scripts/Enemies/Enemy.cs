using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // tpyes && states
    private EnemyType type;
    private AIState aiState = AIState.Idle, lastAIState;

    // targets && references
    [SerializeField] private Vector3 target; //TODO: remove  [SerializeField]
    private Transform playerTransform;

    // variables
    [SerializeField] private float distanceToFindPlayer = 4, distanceToRoamingPoint = 0.2f, moveSpeed = 2, idleMoveSpeed = 0.8f;
    public int health = 10;
    [SerializeField] private float roamingDistance;

    public int damage = 1;
    

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        target = transform.position;
    }

    private void Update()
    {
        if (health <= 0)
            Death();

        FindPlayer();

        AiStateCheck();
    }



    private void FindPlayer()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) <= distanceToFindPlayer)
            target = playerTransform.position;
    }

    private void AiStateCheck()
    {
        if (aiState != lastAIState) return;
        switch (aiState)
        {
            case AIState.Idle:
                IdleState();
                break;
            case AIState.Angry:
                AngryState();
                break;
            default:
                IdleState();
                break;
        }
        lastAIState = aiState;
    }

    private void IdleState()
    {
        if(IsNearTarget())
            target = new Vector3(transform.position.x + Random.Range(-roamingDistance, roamingDistance), transform.position.y + Random.Range(-roamingDistance, roamingDistance), transform.position.z);
        MoveTowardsTarget(idleMoveSpeed);
    }

    private void AngryState()
    {
        MoveTowardsTarget(moveSpeed);
    }

    private void Death()
    {
        
        Destroy(gameObject);
    }



    // reapeating code functions

    private void MoveTowardsTarget(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private bool IsNearTarget()
    {
        return Vector3.Distance(transform.position, target) <= distanceToRoamingPoint;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, target);
    }
}

public enum EnemyType
{
    
} 

public enum AIState
{
    Idle,
    Angry
}