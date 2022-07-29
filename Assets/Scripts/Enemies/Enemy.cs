using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Enemy : MonoBehaviour
{
    // tpyes && states
    private EnemyType type;
    private AIState aiState = AIState.Idle, lastAIState;
    bool isKnockingBack;

    // targets && references
    [SerializeField] private Vector3 target; //TODO: remove  [SerializeField]
    private Transform playerTransform;

    // variables
    [SerializeField] private float distanceToFindPlayer = 4, distanceToRoamingPoint = 0.2f, idleMoveSpeed = 0.8f;
    public int health = 10;
    [SerializeField] private float roamingDistance;
    [SerializeField] public float moveSpeed = 2;

    public int damage = 1;

    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        target = transform.position;
        lastAIState = AIState.Angry;
    }

    private void Update()
    {
        if (isKnockingBack)
            return;

        FindPlayer();

        AiStateCheck();
    }

    private void FindPlayer()
    {
        if (Vector2.Distance(playerTransform.position, transform.position) <= distanceToFindPlayer)
        {
            target = playerTransform.position;
            aiState = AIState.Angry;
        }
        else
            aiState = AIState.Idle;
    }

    private void AiStateCheck()
    {
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

    //TAKING DAMAGE AND SUCH;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ScrewDriverScript>(out var skr))
        {
            health--;
            if (health <= 0)
                Death();
            KnockBack((transform.position - skr.transform.position).normalized);
        }
    }

    public void KnockBack(Vector2 dir)
    {
        isKnockingBack = true;
        Vector2 pos = transform.position;
        Tween knockBack = transform.DOJump(pos + dir, .3f, 1, .3f);
        knockBack.onComplete += () =>
        {
            isKnockingBack = false;
        };
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