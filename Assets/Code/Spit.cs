using UnityEngine;

public class Spit : MonoBehaviour
{
    private double timeToDespawn = 3.0;
    private double timeLeft;

    public float speed = 2;

    public int attackDamage = 15;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeLeft = timeToDespawn;
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        timeLeft -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject;
            //Give damage to player
            var playerScript = (PlantV2)player.GetComponent(typeof(PlantV2));
            playerScript.takeDamage(attackDamage);
            Destroy(gameObject);
            Debug.Log("Hit player");
        }
        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            timeLeft = timeLeft > 1 ? 1 : timeLeft;
        }
    }
}
