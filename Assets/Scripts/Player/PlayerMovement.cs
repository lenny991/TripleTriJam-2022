using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    //VALUES
    public float speed = 4;

    public float dashLength = 6;
    public float dashDelay = 2;

    bool canPlayWalkingSound = true;
    bool dashing;
    bool canDash = true;
    [HideInInspector] public bool isKnockingBack = false;

    //REFERENCES
    Rigidbody2D rb;
    PlayerVisuals visuals;
    Player player;
    public Transform rotateTowards;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        visuals = GetComponentInChildren<PlayerVisuals>();
        player = GetComponent<Player>();
    }

    private Vector2 movement;
    
    private void FixedUpdate()
    {
        if (CanMove())
            Move(movement);
        else
            Move(Vector2.zero);
    }

    private void Update()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")
            );

        if (Input.GetButtonDown("Dash") && canDash)
        {
            Dash(movement);
        }
    }

    void Move(Vector2 dir)
    {
        rb.velocity = dir * speed;
        visuals.moving = dir != Vector2.zero;
        visuals.SetDirection(rotateTowards.position - transform.position);
        if (dir != Vector2.zero && canPlayWalkingSound)
            StartCoroutine(WalkAudioCoroutine());

        IEnumerator WalkAudioCoroutine()
        {
            canPlayWalkingSound = false;
            int i = Random.Range(1, 4);
            yield return new WaitForSeconds(AudioManager.instance.Play("Footstep"+i));
            canPlayWalkingSound = true;
        }
    }

    void Dash(Vector2 dir)
    {
        Tween dash = rb.DOMove(rb.position + (dir * dashLength), .3f);
        dashing = true;
        canDash = false;

        dash.onComplete += () =>
        {
            dashing = false;
            StartCoroutine(CanDashAgain());

            IEnumerator CanDashAgain()
            {
                yield return new WaitForSeconds(dashDelay);
                canDash = true;
            }
        };
    }

    bool CanMove()
    {
        //RETURN FALSE IF YOU SHOULDNT BE ABLE TO MOVE
        if (dashing || player.dead || isKnockingBack)
            return false;
        return true;
    }

    public void KnockBack(Vector2 dir)
    {
        isKnockingBack = true;
        Tween knockBack = rb.DOJump(rb.position + dir, .3f, 1, .3f);
        knockBack.onComplete += () =>
        {
            isKnockingBack = false;
        };
    }
}
