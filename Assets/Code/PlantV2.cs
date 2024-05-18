using UnityEngine;

public class PlantV2 : MonoBehaviour
{
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


    private Rigidbody2D rb;
    [SerializeField] private GameObject floorCollider;    
    private Transform floorColliderPosition;
    
    // HP System:
    [SerializeField] public int health = 30;
    [SerializeField] public int shield = 100;
    private bool isBlocking = false;
    private KeyCode BLOCK = KeyCode.LeftShift;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move = new Vector2(0, 0);
        grounded = false;
        floorColliderPosition = floorCollider.GetComponent<Transform>();   
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {

        isBlocking = Input.GetKey(BLOCK);
        if (isBlocking){
            return; // If the player is blocking, do not allow movement
        }

        grounded = isGrounded();
        
        // if the player is grounded, move at max speed, else reduce the speed by the air_speed_factor if first jump, if second jump reduce by air_speed_factor_second_jump
        current_speed = grounded ? max_speed : doubleJump ? max_speed * air_speed_factor_second_jump : max_speed * air_speed_factor; 

        // else if the player is not blocking, move 
        rb.velocity = new Vector2(move.x * current_speed * Time.deltaTime, rb.velocity.y);

        if (rb.velocity.x < 0)
        {
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        }
        
        if (rb.velocity.x > 0)
        {
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        if (move.y > 0)
        {
            if (grounded || (!doubleJump && rb.velocity.y <= -doubleJumpThreshhold))
            {
                current_jump_force = grounded ? jumpForce : jumpForce * jumpForceSecondJumpFactor;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * current_jump_force, ForceMode2D.Impulse);
                doubleJump = !grounded;

            }
        }

    }


    

    bool isGrounded()
    {
     // raycast to check if the player is on the ground (collision with Floor Tagged object)
     RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(floorColliderPosition.position, Vector2.down, 0.1f);
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
        }
        else
        {
            Debug.Log(string.Format("Player ist taking {0} damage", damage));
            health -= damage;
        }
    }
}
