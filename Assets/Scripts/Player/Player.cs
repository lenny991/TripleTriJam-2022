using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Cinemachine;

public class Player : Singleton<Player>
{
    //VALUES
    [Header("Values")]
    public int startHealth;
    public int health;
    private bool invincible;
    [HideInInspector] public bool dead;

    //REFERENCES
    [Header("References")]
    private PlayerVisuals visuals;
    public GameObject respawnCanvas;
    private PlayerMovement movement;
    [SerializeField] private TMP_Text healthText;
    CinemachineImpulseSource impulse;

    private void Start()
    {
        visuals = GetComponentInChildren<PlayerVisuals>();
        movement = GetComponent<PlayerMovement>();
        impulse = GetComponent<CinemachineImpulseSource>();
        healthText.text = health.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!invincible && collision.TryGetComponent<Enemy>(out var enemy))
        {
            if (health <= 0) return;

            //TAKE DAMAGE
            health -= enemy.damage;
            healthText.text = health.ToString();
            AudioManager.instance.Play("PlayerGetHit");
            GameManager.Instance.combo = 0;

            impulse.GenerateImpulse();

            StartCoroutine(InvisFrames());
            IEnumerator InvisFrames()
            {
                healthText.color = Color.red;
                healthText.fontSize = 150;
                invincible = true;
                visuals.SetInvis(true);
                yield return new WaitForSeconds(.7f);
                healthText.color = Color.white;
                healthText.fontSize = 100;
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
