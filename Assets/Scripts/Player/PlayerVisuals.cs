using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{

    //References
    Animator anim;
    SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //TODO; HOOK UP THE ANIMATOR

    //States
    public bool moving
    {
        set
        {
            //anim.SetBool("Moving", value);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        //anim.SetFloat("X", dir.x);
        //anim.SetFloat("Y", dir.y);
    }

    public void SetInvis(bool invis) =>
        sr.color = invis ? new Color(sr.color.r, sr.color.g, sr.color.b, .5f) : new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
}
