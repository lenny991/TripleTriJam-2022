using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class ScrewDriverScript : MonoBehaviour
{
    //VALUES
    bool canHit;
    public float hitDelay = 1;

    //REFERENCES
    Animator anim;
    SpriteRenderer sr;

    //OTHER
    [DisplayWithoutEdit] public List<ScrewDriver> screwDrivers;
    List<ScrewDriver> unlockedDrivers;
    int selection;

    Player player;

    //UI
    [Header("UI")]
    [SerializeField] private Image screwdriverImage;


    public ScrewDriver screwDriver
    {
        get
        {
            Debug.Log("Selection " + selection);
            return unlockedDrivers[selection];
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        canHit = true;

        sr = GetComponent<SpriteRenderer>();

        screwDrivers.Sort((x, y) => x.waveUnlock.CompareTo(y.waveUnlock));

        player = Player.Instance;

        screwDrivers = Resources.LoadAll<ScrewDriver>("ScrewDrivers").ToList();

        GameManager.waveUpdate.AddListener(x =>
        {
            unlockedDrivers = screwDrivers.GetUnlockedDrivers(x);
        });

        unlockedDrivers = screwDrivers.GetUnlockedDrivers(0);

        SelectDriver(screwDriver);
    }

    private void Update()
    {
        if (player.dead)
            return;

        if (canHit && Input.GetButtonDown("Fire"))
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

        ScrollWheel();

        for(int i = 0; i < unlockedDrivers.Count; i++)
        {
            if(Input.GetKeyDown(unlockedDrivers[i].keyCode))
            {
                selection = (i >= (unlockedDrivers.Count - 1) ? 0 : i);
                SelectDriver(unlockedDrivers[1]);
            }
        }
    }

    private void ScrollWheel()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
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
        screwdriverImage.sprite = driver.spriteUI;
        int i = Random.Range(1, 4);
        AudioManager.instance.Play("Screwdriver switching " + Random.Range(1, 4));
    }
}
