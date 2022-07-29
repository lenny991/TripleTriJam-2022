using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //VALUES
    public int startHealth;
    public int health;
    bool invincible;
    [HideInInspector] public bool dead;

    //REFERENCES
    [Header("References")]
    PlayerVisuals visuals;
    public GameObject respawnCanvas;

    private void Start()
    {
        visuals = GetComponentInChildren<PlayerVisuals>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!invincible && collision.TryGetComponent<Enemy>(out var enemy))
        {
            //TAKE DAMAGE
            health -= enemy.damage;
            StartCoroutine(InvisFrames());
            IEnumerator InvisFrames()
            {
                invincible = true;
                visuals.SetInvis(true);
                yield return new WaitForSeconds(.7f);
                visuals.SetInvis(false);
                invincible = false;
            }
            if (health <= 0)
            {
                //DIE
                dead = true;
                respawnCanvas.SetActive(true);
            }
        }
    }
}
