//////////////////////////////////////////////////////
// Assignment/Lab/Project: Roll A Ball Game
//Name: John Hair
//Section: 2023SP.SGD.113.0001
//Instructor: George Cox
// Date: 04/23/2023
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health;
    public int maxHealth;

    public bool isInvincible;

    public float speed = 10.0f;


    public AudioClip coinSound;
    public AudioClip usePotionSound;
    public AudioClip pickupPotionSound;
    public AudioClip hitSound;
    public AudioClip shieldSound;
    public AudioSource audioSource;

    public GameManager gameManager;
    public GameObject shieldPrefab;


    public HealthBar healthBar;
    private Rigidbody rb;

    public Camera playerCamera;

    private float invincibilityTimeRemaining = 0f;
    public TMP_Text shieldTimer;
    private GameObject instantiatedShield;


    void Start()
    {
        // Initialize variables and UI
        health = 100;
        maxHealth = 100;
        isInvincible = false;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        gameManager.UpdateData(health);
        shieldTimer.gameObject.SetActive(false);

    }

    private void Update()
    {
        // Press R key to use Red potion
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameManager.potions[0] > 0)
            {
                audioSource.PlayOneShot(usePotionSound);
                gameManager.potions[0]--;
                health = Mathf.Min(health + 10, maxHealth);
                gameManager.UpdateData(health);
            }
        }

        // Press G key to use Green potion
        if (Input.GetKeyDown(KeyCode.G) && !isInvincible && gameManager.potions[1] > 0)
        {
            audioSource.PlayOneShot(usePotionSound);
            gameManager.potions[1]--;
            isInvincible = true;
            gameManager.UpdateData(health);
            invincibilityTimeRemaining = 5.0f;
            StartCoroutine(InvincibilityCountdown());
        }

        // Press B key to use Blue potion
        if (Input.GetKeyDown(KeyCode.B) && !isInvincible && gameManager.potions[2] > 0)
        {
            audioSource.PlayOneShot(usePotionSound);
            gameManager.potions[2]--;
            isInvincible = true;
            gameManager.UpdateData(health);
            invincibilityTimeRemaining = 10.0f;
            StartCoroutine(InvincibilityCountdown());
        }
    }

    void FixedUpdate()
    {
        // If Game Over is true, exit the function
        if (gameManager.gameOver) return;

        // Get input values for horizontal and vertical movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a Vector3 to store the movement input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Transform the input movement vector to be relative to the camera's orientation
        movement = playerCamera.transform.TransformDirection(movement);

        // Set the y component of the movement vector to 0 to prevent vertical movement
        movement.y = 0;

        // Apply force to the Rigidbody based on the transformed movement vector and speed
        rb.AddForce(movement.normalized * speed);
    }

    private void LateUpdate()
    {
        if (instantiatedShield != null)
        {
            instantiatedShield.transform.position = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameManager.gameOver) return;

        // Check for collisions with pickups and projectiles
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(coinSound);
            gameManager.coins++;
            Debug.Log($"{gameManager.coins}");
            gameManager.UpdateData(health);

        }
        else if (other.CompareTag("Potion"))
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(pickupPotionSound);
            gameManager.potions[0]++;
            gameManager.UpdateData(health);

        }
        else if (other.CompareTag("Shield"))
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(pickupPotionSound);
            gameManager.potions[1]++;
            gameManager.UpdateData(health);
        }
        else if (other.CompareTag("ShieldPlus"))
        {
            Destroy(other.gameObject);
            audioSource.PlayOneShot(pickupPotionSound);
            gameManager.potions[2]++;
            gameManager.UpdateData(health);

        }
        else if (other.CompareTag("Bomb"))
        {
            
            Destroy(other.gameObject);
            audioSource.PlayOneShot(hitSound);
            if (!isInvincible)
            {
                health -= 20;
            } else
            {
                audioSource.PlayOneShot(shieldSound);
            }
            gameManager.UpdateData(health);
        }

    }

    private IEnumerator InvincibilityCountdown()
    {
        healthBar.SetInvincibleColor();
        shieldTimer.gameObject.SetActive(true); // Make the shieldTimer text visible
        instantiatedShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity); // Instantiate the shield


        while (invincibilityTimeRemaining > 0)
        {
            yield return new WaitForEndOfFrame();
            invincibilityTimeRemaining -= Time.deltaTime;
            shieldTimer.text = $"{invincibilityTimeRemaining:F1}";
        }
        shieldTimer.gameObject.SetActive(false);    // Make the shieldTimer text invisible
        Destroy(instantiatedShield); // Destroy the shield GameObject when invincibility ends
        healthBar.ResetColor();
        instantiatedShield = null;
        EndInvincibility();
    }
    private void EndInvincibility()
    {
        isInvincible = false;
    }
}



