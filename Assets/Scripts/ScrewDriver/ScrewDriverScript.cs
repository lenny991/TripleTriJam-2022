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
    List<ScrewDriver> unlockedDrivers;
    int selection;

    public ScrewDriver screwDriver
    {
        get =>
            unlockedDrivers[selection];
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        canHit = true;

        sr = GetComponent<SpriteRenderer>();

        GameManager.waveUpdate.AddListener(x =>
        {
            unlockedDrivers = screwDrivers.GetUnlockedDrivers(x);
        });

        unlockedDrivers = screwDrivers.GetUnlockedDrivers(0);

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
            if (where >= unlockedDrivers.Count)
                where = 0;
            else if (where < 0)
                where = unlockedDrivers.Count - 1;
            selection = where;
            SelectDriver(screwDriver);
        }
    }

    public void SelectDriver(ScrewDriver driver)
    {
        sr.sprite = driver.sprite;
    }
}
