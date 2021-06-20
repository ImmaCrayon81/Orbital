using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed; //How fast the player can move in the game world

    private Rigidbody2D rb; //Rigidbody2D variable contains all the physics inside Unity
    private Animator anim;

    private Vector2 moveAmount;

    public int health;

    public Image[] hearts; //array to store all our hearts
    public Sprite redHeart; //red heart
    public Sprite blackHeart; //black heart

    private Transitions sceneTransitions;

    private bool invincible = false;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //Enables access to everything in the Animator, able to tweak Player Animator settings via code
        rb = GetComponent<Rigidbody2D>(); //setting rb variable equal to the rigidbody2d component that is attached to player character
        sceneTransitions = FindObjectOfType<Transitions>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 variable = x,y coordinate. Use this variable to detect what keys the user is pressing. 
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveAmount = moveInput.normalized * speed; //.normalized = ensures player does not move faster when moving diagonally

            if (moveInput == Vector2.zero) //if character is running, set isRunning = true. (0 = idle, !0 = not idle)
            {
                anim.SetBool("isRunning", false);
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
    }

    //FixedUpdate gets called every single physics frame
    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime); //Time.fixedDeltaTime makes it framerate independent
    }

    public void TakeDamage(int enemyDamage)
    {
        if (!invincible) {
            health -= enemyDamage;
            UpdateHealthUI(health);
            if (health <= 0)
            {
                Destroy(gameObject);
                sceneTransitions.loadScene("GameOver");
            } else
            {
                invincible = true;
                Invoke("resetInvulnerability", 1);
            }
        }
    }

    public void ChangeWeapon(Weapon weaponToEquip)
    {
        Destroy(GameObject.FindGameObjectWithTag("Weapon"));
        Instantiate(weaponToEquip, transform.position, transform.rotation, transform);
    }

    void resetInvulnerability()
    {
        invincible = false;
    }

    void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) 
            {
                hearts[i].sprite = redHeart;
            } else
            {
                hearts[i].sprite = blackHeart;
            }
        }
    }

    public void Heal(int healAmount)
    {
        if (health + healAmount > 5)
        {
            health = 5;
        } else {
            health += healAmount;
        }
        UpdateHealthUI(health);
    }
}
