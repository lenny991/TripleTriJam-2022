using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerVisuals : MonoBehaviour
{
    //References
    Animator anim;
    public SpriteRenderer[] allAffectedRenderers;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //TODO; HOOK UP THE ANIMATOR

    //States
    public bool moving
    {
        set
        {
            anim.SetBool("Moving", value);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        if(dir.x != 0)
            transform.localScale = new Vector2(dir.x > 0 ? .3f : -.3f, transform.localScale.y);
    }

    public void SetInvis(bool invis)
    {
        foreach (SpriteRenderer sr in allAffectedRenderers)
            sr.color = invis ? new Color(sr.color.r, sr.color.g, sr.color.b, .5f) : new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    public void SetRoll(bool a) =>
        anim.SetBool("Rolling", a);
}
