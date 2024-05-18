using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public float jumpForce = 7.0f;

    public Rigidbody2D rigidbody;
    private KeyCode RIGHT = KeyCode.D;
    private KeyCode LEFT = KeyCode.A;
    private KeyCode UP = KeyCode.W;

    private bool doubleJump = false;


    // HP System:
    [SerializeField] public int health = 30;
    [SerializeField] public int shield = 100;
    private bool isBlocking = false;
    private KeyCode BLOCK = KeyCode.LeftShift;



    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 2.0f;
    }
    void FixedUpdate()
    {

        if (Input.GetKeyDown(BLOCK))
        {
            isBlocking = true;
        }
        if (Input.GetKeyUp(BLOCK))
        {
            isBlocking = false;
        }


        // --------------------------------------------------------------------------------
        // Insert anything over this line, further down an execution can not be guaranteed

        if (isBlocking)
        {
            // If the player is blocking, do not allow movement
            return;
        }

        if (Input.GetKey(RIGHT))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
            //rigidbody.velocity += new Vector2(speed * Time.deltaTime, 0.0f);
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        }

        if (Input.GetKey(LEFT))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0.0f, 0.0f);
            //rigidbody.velocity += new Vector2(-speed * Time.deltaTime, 0.0f);
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
        }

        if (Input.GetKeyDown(UP))
        {
            // if the player is on the gound jump and set doubleJump to false
            // else if player is in air, but hasn't double jumped, jump and set doubleJump to true
            if (Mathf.Abs(rigidbody.velocity.y) < 0.001f)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
            }
            else if (!doubleJump && rigidbody.velocity.y < 0)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = true;

            }
        }
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
