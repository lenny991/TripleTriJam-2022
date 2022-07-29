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
    PlayerMovement movement;

    private void Start()
    {
        visuals = GetComponentInChildren<PlayerVisuals>();
        movement = GetComponent<PlayerMovement>();
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

            movement.KnockBack((transform.position - enemy.transform.position).normalized);

            if (health <= 0)
            {
                //DIE
                dead = true;
                respawnCanvas.SetActive(true);
            }
        }
    }
}
