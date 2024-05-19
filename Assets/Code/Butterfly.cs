using UnityEngine;

public class Butterfly : Enemy
{
    private Vector2[] vectors = new Vector2[8];
    private float attackRange = 1.5f;
    
    private float currentAngle = -0.610865f;
    private float addAngle = 0.174533f;

    private int facingRight = 1;

    public override void Start()
    { // call normale start and then calculate the attack vectors
        base.Start();
        for (int i = 0; i < vectors.Length; i++) 
        {
            vectors[i] = calculateUnitVector(currentAngle);
            currentAngle += addAngle;
        }
    }

    public override void Update()
    {
        base.Update();
        
        facingRight = transform.rotation.y == 0 ? 1 : -1;
        for (int i = 0; i < vectors.Length; i++)
        {
            Debug.DrawRay(transform.position, vectors[i]*attackRange*facingRight, Color.red);
        }

        if (currentCooldown <= 0)
        {
            searchForPlayer();
        }
    }

    void searchForPlayer()
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(transform.position, vectors[i]*facingRight, attackRange);
            foreach (RaycastHit2D hit in raycastHit2Ds)
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                {
                    raycastAttack(hit);
                    return;
                }
            }
        }
    }

    public override void attack(Collision2D playerCollision) 
    {
        // No normal attack
    }
    public virtual void raycastAttack(RaycastHit2D playerObject)
    {
        var player = playerObject.collider.gameObject;
        
        // TODO: Implement attack
        player.GetComponent<PlantV2>().takeDamage(attackDamage);

        currentCooldown = attackCooldown;
    }

    public override void patrol()
    {
        // TODO: Implement patrol    
        //The current patrol destination
        Transform destination = patrolPoints[patrolDestination].GetComponent<Transform>();
        Vector2 destinationPosition = new Vector2(destination.position.x, destination.position.y);
        
        // current Position
        Vector2 thisPosition = new Vector2(transform.position.x, transform.position.y);

        // move towards destination
        transform.position = Vector2.MoveTowards(thisPosition, destinationPosition, moveSpeed * Time.deltaTime);

        if (destinationPosition.x < thisPosition.x) 
        {
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        } else {
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        

        float distance = Vector2.Distance(destinationPosition, thisPosition);
        if (distance < .2f)
        {
            patrolDestination = (patrolDestination + 1) % patrolPoints.Length;
        }
    }

    
    public override void takeBulletDamage(Collision2D bulletCollision)
    {
        // Onehit kill
        Debug.Log("Butterfly is taking bullet damage (is onehit)");
        currentHealth -= currentHealth;
    }


    public override void die()
    {
        // rotate sprite, remove collider and destroy object after 5 seconds
        transform.Rotate(Vector3.left * -180);
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 5f);
        Debug.Log("Butterfly died");
    }

    public override void takeJumpDamage(Collision2D playerCollision)
    {
        // no jump damage possible
    }

    public override void takeMeleeDamage(int damage)
    {
        // no meelee damage possible
    }
    Vector2 calculateUnitVector(float angle) { 
        return new Vector2(
            (float) Mathf.Cos(angle),   
            (float) Mathf.Sin(angle) 
        );
    }

}
