using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverScript : MonoBehaviour
{
    //VALUES
    bool canHit;
    public float hitDelay = 1;

    //REFERENCES
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        canHit = true;
    }

    private void Update()
    {
        if(canHit && Input.GetButtonDown("Fire"))
        {
            anim.SetTrigger("Hit");
            StartCoroutine(CanHit());

            IEnumerator CanHit()
            {
                canHit = false;
                yield return new WaitForSeconds(hitDelay);
                canHit = true;
            }
        }
    }
}
