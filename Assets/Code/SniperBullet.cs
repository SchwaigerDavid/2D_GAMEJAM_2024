using UnityEngine;

public class NewEmptyCSharpScript: MonoBehaviour
{
    private double timeToDespawn = 3.0;
    private double timeLeft;

    public float speed = 10;
    
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
        if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("Player"))
        {
            timeLeft = 0;
        }
    }
}
