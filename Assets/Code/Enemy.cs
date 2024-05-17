using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;

    //The two point between which the enemy should move
    public Transform[] patrolPoints;

    public int patrolDestination;
    public int moveSpeed;


    //attack cooldown in sec
    public int attackCooldown;
    public float currentCooldown;

    private void Update()
    {

        
        if (isPlayerInSight() && currentCooldown <= 0)
        {
            //TODO: Move towards player
        }
        else
        {
            //Keep patrolling
            patrol();
            if(currentCooldown > 0)
            {
                currentCooldown = Math.Max(currentCooldown - Time.deltaTime, 0);
                Debug.Log("Current Cooldown: " + currentCooldown);
            }
            
        }
    }

    private void Start()
    {
        if(moveSpeed <= 0)
        {
            //Default movement speed
            moveSpeed = 2;
        }
        patrolDestination = 0;
        currentCooldown = 0;
        attackCooldown = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //TODO: Attack player
            attack();
        }
    }

    public Boolean isPlayerInSight()
    {
        Transform destination = patrolPoints[patrolDestination].GetComponent<Transform>();
        float enemyX = transform.position.x;
        float destinationX = destination.position.x;
        Boolean isMovingRight = destinationX > enemyX;

        var raycastDirection = transform.TransformDirection(isMovingRight ? Vector2.right : Vector2.left);

        var distance = 1f;
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, raycastDirection);
        var player = GameObject.FindWithTag("Player").transform;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log(distanceToPlayer);
        return distanceToPlayer < 0.5;
    }

    public void attack()
    {
        Debug.Log("Attacked");
        currentCooldown = attackCooldown;
    }

    private void patrol()
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
            transform.Rotate(Vector3.left * -180);
            return;
        }
    }




   
}
