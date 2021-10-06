using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float padding = 1;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f;



    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10;
    [SerializeField] float projectileFiringPeriod = 3f;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;

    float ymin;
    float yMax;
    
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
        
    }

   

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } // If there is no damageDealer or if it is not damageDealer then return (don't do anything forther than return)
        ProcessHit(damageDealer);
    }
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
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth()
    {
        return health;
    }
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
           firingCoroutine= StartCoroutine(FireContinuesly());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuesly()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        
    }

    void Move()
    {
        // NOTICE :  We could use Rigidbody2D but i wanted to show that there are many ways to move your player. 
        var moveX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var moveY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + moveX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + moveY, ymin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundries()
    {
        // Transforms position from viewport space into world space.
        // It converts the position of something as it relates to camera view into a world space value.
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        ymin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;

    }
}
     
