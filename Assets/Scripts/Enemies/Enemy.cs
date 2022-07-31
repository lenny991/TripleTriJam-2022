using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;
using Cinemachine;

public class Enemy : MonoBehaviour
{
    // tpyes && states
    private EnemyType type;
    private AIState aiState = AIState.Idle, lastAIState;
    bool isKnockingBack;
    bool isDying;
    bool canPlayWalkingSound = true;

    // targets && references
    [SerializeField] private Vector3 target; //TODO: remove  [SerializeField]
    private Transform playerTransform;
    public ScrewDriver acceptedDriver;
    SpriteRenderer sr;
    CinemachineImpulseSource impulse;
    Player player;
    PlayerMovement playerMovement;

    // variables
    [SerializeField] private float distanceToFindPlayer = 4, distanceToRoamingPoint = 0.2f, idleMoveSpeed = 0.8f;
    public int health = 10;
    [SerializeField] private float roamingDistance;
    [SerializeField] public float moveSpeed = 2;
    
    private bool outside = false;

    public int damage = 1;

    const float screen_x = 8.5f;
    const float screen_y = 4.5f;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        impulse = GetComponent<CinemachineImpulseSource>();
        player = Player.Instance;
        playerMovement = player.movement;

        playerTransform = Player.Instance.transform;
        target = transform.position;
        lastAIState = AIState.Angry;
    }

    private void Update()
    {
        if (isKnockingBack || isDying)
            return;

        CheckBoundaries();

        FindPlayer();

        if (!outside)
            AiStateCheck();
        else
            MoveTowardsTarget(moveSpeed);
    }

    private void CheckBoundaries()
    {
        outside = true;
        if (transform.position.x > screen_x)
            target = new Vector3(transform.position.x - 3, transform.position.y + Random.Range(-roamingDistance, roamingDistance), transform.position.z);
        else if (transform.position.x < -screen_x)
            target = new Vector3(transform.position.x + 3, transform.position.y + Random.Range(-roamingDistance, roamingDistance), transform.position.z);
        else if (transform.position.y > screen_y)
            target = new Vector3(transform.position.x + Random.Range(-roamingDistance, roamingDistance), transform.position.y - 3, transform.position.z);
        else if (transform.position.y < -screen_y)
            target = new Vector3(transform.position.x + Random.Range(-roamingDistance, roamingDistance), transform.position.y + 3, transform.position.z);
        else
            outside = false;
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
        GameManager.Instance.enemies.Remove(gameObject);
        GetComponent<Collider2D>().enabled = false;
    }

    bool dead;

    // reapeating code functions

    private void MoveTowardsTarget(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (canPlayWalkingSound)
            StartCoroutine(ScrewWalkingSoundCoroutine());
    }

    IEnumerator ScrewWalkingSoundCoroutine()
    {
        canPlayWalkingSound = false;
        int i = Random.Range(1, 4);
        yield return new WaitForSeconds(AudioManager.instance.Play("Screw walking " + i) + .3f);
        canPlayWalkingSound = true;
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
            if(skr.screwDriver == acceptedDriver)
            {
                //impulse.GenerateImpulse(); 
                health--;
                if (health <= 0)
                {
                    KnockBack((transform.position - playerTransform.position).normalized);
                    Death();
                    isDying = true;
                    AudioManager.instance.Play("Screw death");
                    GameManager.Instance.combo += 1;
                }
                else
                {
                    KnockBack((transform.position - playerTransform.position).normalized);
                }

                Tween fade = sr.DOColor(Color.red, .04f);
                fade.onComplete += () =>
                {
                    sr.DOColor(Color.white, .3f);
                    if (isDying)
                    {
                        fade = sr.DOFade(0, .4f);
                        fade.onComplete += () =>
                        {
                            Destroy(gameObject);
                        };
                    }
                };
            }
            else
            {
                //DO KNOCKBACK, mayve shake screen
                playerMovement.KnockBack((playerTransform.position - transform.position).normalized);
                player.InvokeImpulse();
            }
        }
    }

    public void KnockBack(Vector2 dir, UnityAction doAfter = null)
    {
        AudioManager.instance.Play("Screwdriver attack - end");
        isKnockingBack = true;
        Vector2 pos = transform.position;
        Tween knockBack = transform.DOJump(pos + dir, .3f, 1, .3f);
        knockBack.onComplete += () =>
        {
            isKnockingBack = false;
            if (doAfter != null)
                doAfter();
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