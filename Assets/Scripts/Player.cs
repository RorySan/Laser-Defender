using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player Config")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.7f;
    [SerializeField] float health = 200f;
    [Header("Proyectile Config")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] float laserRateOfFire = 0.3f;
    [SerializeField] AudioClip playerDestructionSound;
    [SerializeField] AudioClip playerLaserSound;
    [SerializeField] [Range(0, 1)] float playerLaserVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float playerExplosionVolume = 0.7f;
    [SerializeField] GameObject sceneLoader;
    [SerializeField] HealthDisplay healthText;

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {        
        SetupMoveBoundaries();
        healthText = FindObjectOfType<HealthDisplay>();
        
    }

    

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();        
    }

    IEnumerator FireContinously()
    {
        while(true)
        {
            
            GameObject laser = Instantiate(
                    laserPrefab,
                    new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z),
                    Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(playerLaserSound, Camera.main.transform.position, playerLaserVolume);
        
            yield return new WaitForSeconds(laserRateOfFire);
        }
        
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinously());         
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

        private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp((transform.position.x + deltaX),xMin,xMax);
        var newYPos = Mathf.Clamp((transform.position.y + deltaY),yMin,yMax);
        
        transform.position = new Vector2(newXPos, newYPos);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            ProcessHit(damageDealer);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        healthText.UpdateHealth();
        damageDealer.Hit();
        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(playerDestructionSound, Camera.main.transform.position, playerExplosionVolume);
            sceneLoader.GetComponent<SceneLoader>().DelayedNextScene();
            Destroy(gameObject);
            
        }
    }

    private void SetupMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
