using UnityEngine;

public class PlantV2 : MonoBehaviour
{

    //Animator
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Movement:
    [SerializeField] public float max_speed = 300;
    [SerializeField] public float air_speed_factor = 0.85f;
    [SerializeField] public float air_speed_factor_second_jump = 0.75f;
    [SerializeField] public float jumpForce = 7.0f;
    [SerializeField] public float jumpForceSecondJumpFactor = 0.6f;

    [SerializeField] public float doubleJumpThreshhold = 1;
    
    private float current_speed;
    private float current_jump_force;

    private Vector2 move;
    public bool doubleJump = false;
    public bool grounded;

    public float rayCastThreshold = 0.05f;

    private Rigidbody2D rb;
    [SerializeField] private GameObject floorCollider;    
    private Transform floorColliderPosition;
    
    // HP System:
    [SerializeField] public int maxHealth = 30;
    private int health;
    private float healthCooldown = 3f;
    private float cuurentHealthCooldown = 0f;
    [SerializeField] public int shield = 100;
    public bool isBlocking = false;
    private KeyCode BLOCK = KeyCode.LeftShift;


    // if crouching drop weapon timer
    private float dropWeaponTimer = 0.8f;
    private float currentDropWeaponTimer = 0f;
    private float moveSoundTimeElapsed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        move = new Vector2(0, 0);
        grounded = false;
        floorColliderPosition = floorCollider.GetComponent<Transform>();   
        health = maxHealth;
        spriteRenderer.sprite = FaceManager.Instance.selectedFace;
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isBlocking = Input.GetKey(BLOCK);
        moveSoundTimeElapsed += Time.deltaTime;
    }

    void FixedUpdate()
    {

        isBlocking = Input.GetKey(BLOCK);
        animator.SetBool(AnimationStates.isBlocking, isBlocking);
        if (isBlocking){
            animator.SetBool(AnimationStates.isMoving, false);
            if (currentDropWeaponTimer > 0)
            {
                currentDropWeaponTimer -= Time.deltaTime;
            }
            else
            {
                DropWeapon();
            }
            return; // If the player is blocking, do not allow movement
        } else {
            currentDropWeaponTimer = dropWeaponTimer;
        }

        grounded = isGrounded();

        animator.SetBool(AnimationStates.isMoving, move.x != 0 && move.y <= 0);
        animator.SetBool(AnimationStates.isJumping, move.y > 0);
        if (move.x != 0)
        {
            moveInXDirection();
        }

        if (move.y > 0)
        {
            moveInYDirection();
        }

    }

    void moveInXDirection()
    {
        // if the player is grounded, move at max speed, else reduce the speed by the air_speed_factor if first jump, if second jump reduce by air_speed_factor_second_jump
        current_speed = grounded ? max_speed : doubleJump ? max_speed * air_speed_factor_second_jump : max_speed * air_speed_factor; 

        /*
        // if direction is changed, and the player is in the air, don't change the speed
        if (move.x * rb.velocity.x < 1 && !grounded)
        {
            //Debug.Log(move.x);
            move.x = 0.5f * (rb.velocity.x > 0 ? 1 : -1);
        }
        // else if the player is not blocking, move 
        */

        rb.velocity = new Vector2(move.x * current_speed * Time.deltaTime, rb.velocity.y);
        
        if (rb.velocity.x < 0)
        {
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        }
        
        if (rb.velocity.x > 0)
        {
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        if(grounded && moveSoundTimeElapsed > 0.2) {
            SoundManager.Instance.playRandom("plant_walk/metal", 1.5);
            moveSoundTimeElapsed = 0;
            Debug.Log(rb.velocity.x);
        }
    }
    void moveInYDirection(){
        if (grounded || (!doubleJump && rb.velocity.y <= -doubleJumpThreshhold))
            {
                current_jump_force = grounded ? jumpForce : jumpForce * jumpForceSecondJumpFactor;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * current_jump_force, ForceMode2D.Impulse);
                doubleJump = !grounded;
                SoundManager.Instance.playRandom("plant_jump_whoosh", 0.7);
            }
    }

    bool isGrounded()
    {
     // raycast to check if the player is on the ground (collision with Floor Tagged object)
     RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(floorColliderPosition.position, Vector2.down, rayCastThreshold);
        foreach (RaycastHit2D hit in raycastHit2Ds)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Floor"))
            {
                return true;
            }
        }
        return false;
    }


    public void takeDamage(int damage)
    {
        if (isBlocking)
        {
            Debug.Log(string.Format("Player shield ist taking {0} damage", damage));
            shield -= damage;
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
            SoundManager.Instance.playRandom("pot_land", 0.7);
        }
        else
        {
            Debug.Log(string.Format("Player ist taking {0} damage", damage));
            health -= damage;
            SoundManager.Instance.playRandom("plant_damage/leaves_rustle", 0.2);
            SoundManager.Instance.playRandom("plant_damage/twig_snap");
            SoundManager.Instance.playRandom("plant_damage/plant_damage_voice");
        }
    }


    public void knockback(Vector2 knockbackVector)
    {
        Debug.Log(string.Format("Knockback: {0} {1}", knockbackVector.x, knockbackVector.y));
        rb.AddForce(knockbackVector, ForceMode2D.Impulse);
    }

    public void heal(int healAmount)
    {
        if (cuurentHealthCooldown > 0)
        {
            cuurentHealthCooldown -= Time.deltaTime;
        } 
        else
        {
            Debug.Log(string.Format("Player is healing {0} health", healAmount));
            if (health + healAmount <= maxHealth)
            {
                health += healAmount;
            }
            cuurentHealthCooldown = healthCooldown;
            health += healAmount;
        }
        
    }

    private void DropWeapon()
    {
        GetComponent<WeaponManager>().DropWeapon(); // If the player is blocking, drop the weapon
    }
}