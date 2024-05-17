using UnityEngine;

public class movableObject : MonoBehaviour
{
[SerializeField] public float speed = 5.0f;
[SerializeField] public float jumpForce = 7.0f;

public Rigidbody2D rigidbody;
private KeyCode RIGHT = KeyCode.D;
private KeyCode LEFT = KeyCode.A;
private KeyCode UP = KeyCode.W;

private bool doubleJump = false;

void Start()
{
    rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.gravityScale = 2.0f;
}
void Update()
{
    if (Input.GetKey(RIGHT))
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
    }

    if (Input.GetKey(LEFT))
    {
        transform.position += new Vector3(-speed * Time.deltaTime, 0.0f, 0.0f);
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
        else if (!doubleJump)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            doubleJump = true;
        
    }
}
}
}
