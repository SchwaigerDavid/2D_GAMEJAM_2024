using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D enemyRigidBody;
    public Collider2D enemyCollider;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    //MOVEMENT MANAGEMENT
    public Transform[] patrolPoints; //The two point between which the enemy should move
    public int patrolDestination;
    public int moveSpeed;

    //ATTACK MANAGEMENT
    public int attackDamage;
    public float attackCooldown;
    public int attackKnockback;
    private float _currentCooldown;
    public float currentCooldown
    {
        get { return _currentCooldown; }

        set
        {
            _currentCooldown = value;
        }
    }

    private Color _originColor;
    private Color _damageEffectColor = Color.red;
    private float _damageEffectDuration = 1;
    private float _damageEffectDelay = 0.5f;

    private float _jumpOnKnockback = 6f;

    //HEALTH MANAGEMENT
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

    /* UNITY LIEFECYCLE METHODS */

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer != null )
        {
            _originColor = spriteRenderer.color;
        }
    }


    public virtual void Update()
    {
        if (_currentCooldown > 0)
        {
            //Prevent currentCooldown from becoming negative
            _currentCooldown = Math.Max(_currentCooldown - Time.deltaTime, 0);
        }

        if(patrolPoints.Length > 1)
        {
            //Keep patrolling
            patrol();
        }
        else
        {
            animator.SetBool(AnimationStates.isMoving, false);
        }
    }

    public virtual void Start()
    {
        //Set default values
        patrolDestination = 1;
        _currentCooldown = 0;
        _currentHealth = maxHealth; 
    }


    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Player")
        { 
            if (gotJumpedOn(collision))
            {
                //Player jumps on enemy, it takes half it maxHealth as damage
                takeJumpDamage(collision);
                //Player is knocked back up
                var playerScript = (PlantV2)obj.GetComponent(typeof(PlantV2));
                playerScript.knockback(new Vector2(0, _jumpOnKnockback));
            }
            else if (_currentCooldown <= 0)
            {
                //Enemy attacks player if it has not cooldown
                doAttack(collision);
                
            } 
        }else if (obj.tag == "Bullet")
        {
            takeBulletDamage(collision);
        }
        else
        {
            //collided with any other object, so should turn around
            patrolDestination = patrolDestination == 0 ? 1 : 0;
            transform.Rotate(Vector3.up * -180);
        }
    }

    private void doAttack(Collision2D collision)
    {
        animator.SetBool(AnimationStates.isMoving, false);
        /*
        float playerX = collision.transform.position.x;
        float ownX = transform.position.x;
        float currentRotation = transform.rotation.y;
        float dotProductt = Vector3.Dot(collision.transform.position, transform.position);
        Debug.Log(string.Format("Dot Product {0}", currentRotation));
        //Face player
        if (playerX < ownX && currentRotation > 0)
        {
            //Collision on the left while facing right
            transform.Rotate(Vector3.up * -180);
            patrolDestination = 0;
        }else if(playerX > ownX && currentRotation < 0)
        {
            //Collision on the right while facing left
            transform.Rotate(Vector3.up * -180);
            patrolDestination = 1;
        }
        Debug.Log(string.Format("New Rotation Y {0}", transform.rotation.y));
        */
        //Execute the attack
        attack(collision);
    }

    private Boolean gotJumpedOn(Collision2D collision)
    {
        //Still not sure if this works as intended...
        //But lets leave it like that until another bug appears.
        var colliderTransform = collision.gameObject.transform;
        var contact = collision.GetContact(0);
        var contactPoint = contact.point;
        var ownCenter = enemyCollider.bounds.center;
        //Y of contact point is either bigger than the enemy colider max or contact point is between enemy center and enemy collider max
        Boolean fromTop = contactPoint.y > enemyCollider.bounds.max.y || (contactPoint.y < enemyCollider.bounds.max.y && contactPoint.y > ownCenter.y);
        //X of player is between within the x-coordinates of the enemy
        Boolean aboveCollider = colliderTransform.position.x > enemyCollider.bounds.min.x && colliderTransform.position.x < enemyCollider.bounds.max.x;
        return fromTop && aboveCollider;
    }


    /* ENEMY ACTIONS */

    public virtual void patrol()
    {
        //The current patrol destination
        animator.SetBool(AnimationStates.isMoving, true);
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

    public virtual void takeMeleeDamage(int damage)
    {
        Debug.Log("Enemy took " + damage + " melee damage");
        currentHealth -= damage;
        showDamageEffect();
    }

    //Following methods have to be implemented by the child classes
    public abstract void attack(Collision2D collision);
    public abstract void die();
    public abstract void takeJumpDamage(Collision2D collision);
    public abstract void takeBulletDamage(Collision2D collision);


    /* ENEMY EFFECTS AND CO-ROUTINES */
    
    public void showDamageEffect()
    {
        StartCoroutine(DamageEffectSequence(_damageEffectColor, _damageEffectDuration, _damageEffectDelay));
    }

    private IEnumerator DamageEffectSequence(Color dmgColor, float duration, float delay)
    {
        // tint the sprite with damage color
        spriteRenderer.color = dmgColor;
        // you can delay the animation
        yield return new WaitForSeconds(delay);
        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime / duration)
        {
            spriteRenderer.color = Color.Lerp(dmgColor, _originColor, t);
            yield return null;
        }
        // restore origin color
        spriteRenderer.color = _originColor;
    }
}
