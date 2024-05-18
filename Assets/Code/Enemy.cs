using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    public Collider2D enemyCollider;
    public Animator animator;

    //The two point between which the enemy should move
    public Transform[] patrolPoints;

    public int patrolDestination;
    public int moveSpeed;

    //Attack cooldown in sec
    public float attackCooldown;
    private float _currentCooldown;

    public float currentCooldown
    {
        get { return _currentCooldown; }

        set
        {
            _currentCooldown = value;
        }
    }

    //Health management
    public int maxHealth;
    private int _currentHealth;
    
    public int currentHealth
    {
        get {
            return _currentHealth;
        }

        set
        {
            _currentHealth = value;
            if(_currentHealth <= 0)
            {
                die();
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }


    public virtual void Update()
    {
        if (_currentCooldown > 0)
        {
            //Prevent currentCooldown from becoming negative
            _currentCooldown = Math.Max(_currentCooldown - Time.deltaTime, 0);
        }

        //Keep patrolling
        patrol();
    }

    public virtual void Start()
    {
        if(moveSpeed <= 0)
        {
            //Default movement speed
            moveSpeed = 2;
        }
        patrolDestination = 1;
        _currentCooldown = 0;
        attackCooldown = 0.5f;
        maxHealth = 100;
        _currentHealth = maxHealth; 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject obj = collision.gameObject;
        if (obj.tag == "Player")
        {
            var contact = collision.GetContact(0);
            var contactPoint = contact.point;
            var ownCenter = enemyCollider.bounds.center;
            Debug.Log("Contact Point: " + contactPoint.y + ", Own y: " + ownCenter.y);
            if (contactPoint.y > ownCenter.y)
            {

                //Player jumps on enemy, it takes half it maxHealth as damage
                takeJumpDamage(collision);
            }
            else if (_currentCooldown <= 0)
            {
                //Enemy attacks player
                attack(collision);
            } 
        }else if (obj.tag == "Bullet")
        {
            takeBulletDamage(collision);
        }
    }

    public virtual void patrol()
    {
        //The current patrol destination
        Transform destination = patrolPoints[patrolDestination].GetComponent<Transform>();
        float enemyX = transform.position.x;
        float destinationX = destination.position.x;
        Boolean moveRight = destinationX > enemyX;

        //Caclulate distance between both points
        int direction = moveRight ? 1 : -1;
        float delta = direction * moveSpeed * Time.deltaTime;
        Vector3 movePosition = new Vector3(enemyX + delta, transform.position.y, transform.position.z);
        transform.position = movePosition;

        float distance = Math.Abs(destinationX - enemyX);
        if (distance < .2f)
        {
            patrolDestination = patrolDestination == 0 ? 1 : 0;

            //Rotate sprite in other direction
            transform.Rotate(Vector3.up * -180);
        }
    }

    //Following methods have to be implemented by the child classes
    public abstract void attack(Collision2D collision);
    public abstract void die();
    public abstract void takeJumpDamage(Collision2D collision);
    public abstract void takeBulletDamage(Collision2D collision);



}
