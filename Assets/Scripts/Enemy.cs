using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
     float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.5f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownANdShoot();
    }

    private void CountDownANdShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // If there is no damageDealer or if it is not damageDealer then return (don't do anything forther than return)
        ProcessHit(damageDealer);

        //  health -= damageDealer.GetDamage();
        //  if(health <= 0)
        //  {
        //      Destroy(gameObject);
        //  }
    }

    // you are creating a promise to C# that says any call to ProcessHit will send a DamageDealer argument(up there), and then that ProcessHit will not return a value.
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
           
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
