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

    //REFERENCES
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 movement;
    
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
    }

    bool dashing;
    bool canDash = true;

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
        if (dashing)
            return false;
        return true;
    }
}
