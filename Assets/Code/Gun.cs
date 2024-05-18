using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shotPos;
    public GameObject bullet;
    public int amountOfBullets;
    public float spread, cooldown;
    private float cooldownTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Check if the object where this script is attached is a child of the player
        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.CompareTag("Player"))
        {
            cooldownTimer = cooldown; 
            for (int i = 0; i < amountOfBullets; i++)
            {

                GameObject bulletIns = Instantiate(bullet, shotPos.position, transform.rotation);
                Rigidbody2D rb = bulletIns.GetComponent<Rigidbody2D>();
                /*//Vector2 dir = transform.rotation * Vector2.up;
                Vector2 dir = transform.up;
                Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);
                rb.velocity = (dir + pdir) * bulletSpeed;*/
                rb.AddForce(transform.up * Random.Range(0, spread), ForceMode2D.Force);

            }
        }
    }
}
