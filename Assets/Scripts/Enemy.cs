using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Attributes")]
    [SerializeField] float health = 100;
    [SerializeField] int points = 100;
    [Header("Shooting Config")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.5f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float durationOfExplosion = 1f;
    [Header("Music and Sounds")]
    [SerializeField] AudioClip enemyExplosionSound;
    [SerializeField] AudioClip enemyLaserFireSound;
    [SerializeField] [Range(0, 1)] float enemyLaserVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float enemyExplosionVolume = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
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
        GameObject laser = Instantiate(
                      enemyLaserPrefab,
                     transform.position - new Vector3 (0, 2, 0),
                     Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        AudioSource.PlayClipAtPoint(enemyLaserFireSound, Camera.main.transform.position,enemyLaserVolume);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null) 
        {
            ProcessHit(damageDealer);
        }

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(enemyExplosionSound, Camera.main.transform.position, enemyExplosionVolume);
        Destroy(explosion, durationOfExplosion);
        Destroy(gameObject);
    }
}
