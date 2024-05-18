using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    public Animator animator;

    //The two point between which the enemy should move
    public Transform[] patrolPoints;

    public int patrolDestination;
    public int moveSpeed;

    //attack cooldown in sec
    public float attackCooldown;
    public float currentCooldown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public virtual void Update()
    {
        if (currentCooldown > 0)
        {
            //Prevent currentCooldown from becoming negative
            currentCooldown = Math.Max(currentCooldown - Time.deltaTime, 0);
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
        currentCooldown = 0;
        attackCooldown = 0.5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && currentCooldown <= 0)
        {
            attack(collision);
        }
    }

    public virtual void attack(Collision2D collision)
    {
        Debug.Log("Attacked");
        currentCooldown = attackCooldown;
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
            return;
        }
    }




   
}
