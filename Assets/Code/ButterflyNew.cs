using UnityEngine;

public class ButterflyNew : Enemy
{
    public Transform shotPos;
    public GameObject spit;
    public int amountOfBullets = 1;
    private float spread = -15f;

    public override void Update()
    {
        base.Update();

        if (currentCooldown <= 0)
        {
            attack();
            currentCooldown = attackCooldown*5;
        }
    }

    public override void attack(Collision2D playerCollision) 
    {
        // No normal attack
    }

    public virtual void attack()
    {
        SoundManager.Instance.playRandom("butterfly_shoot", 0.6);
        animator.SetTrigger(AnimationStates.attackTrigger);
        for (int i = 0; i < amountOfBullets; i++)
            {
                GameObject spitInstance = Instantiate(spit, shotPos.position, transform.rotation);
                Rigidbody2D rb = spitInstance.GetComponent<Rigidbody2D>();
                rb.AddForce(transform.up * Random.Range(-spread, spread), ForceMode2D.Force);
            }
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
        showDamageEffect();
        Debug.Log("Butterfly is taking bullet damage (is onehit)");
        currentHealth = 0;
    }


    public override void die()
    {
        // rotate sprite, remove collider and destroy object after 1 second
        transform.Rotate(Vector3.left * -180);
        SoundManager.Instance.playRandom("butterfly_death", 2);
        GetComponent<AudioSource>().Stop();
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 1f);
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

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Bullet")
        {
            takeBulletDamage(collision);
        }
    }
}
