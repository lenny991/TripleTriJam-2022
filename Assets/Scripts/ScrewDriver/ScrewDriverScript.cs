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
    SpriteRenderer sr;

    //OTHER
    public List<ScrewDriver> screwDrivers;
    int selection;
    public ScrewDriver screwDriver
    {
        get =>
            screwDrivers[selection];
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        canHit = true;

        sr = GetComponent<SpriteRenderer>();
        SelectDriver(screwDriver);
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

        float scroll = Input.mouseScrollDelta.y;

        if(scroll != 0)
        {
            int where = selection + (scroll > 0 ? 1 : -1);
            if (where >= screwDrivers.Count)
                where = 0;
            else if (where < 0)
                where = screwDrivers.Count - 1;
            selection = where;
            SelectDriver(screwDrivers[where]);
        }
    }

    public void SelectDriver(ScrewDriver driver)
    {
        sr.sprite = driver.sprite;
    }
}
