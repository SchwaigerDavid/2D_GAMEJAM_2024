using UnityEngine;

public class Sniper : MonoBehaviour
{
    public Transform shotPos;
    public GameObject bullet;
    public float cooldown;
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


        if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.CompareTag("Player") && !transform.parent.parent.GetComponent<PlantV2>().isBlocking)
        {
            double plantFacing = transform.parent.parent.GetComponent<Transform>().rotation.y;
            rotateToMouse(plantFacing);
            if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0)
            {
                Shoot();
            }
    }
    }

    void rotateToMouse(double plantFacing)
    { 
        double rotationAdd = plantFacing == 0 ? 0 : 180;
        // Rotate the object to face the mouse if the angle between the x-axis and the mouse is less than 30 degrees
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 30 + rotationAdd && angle > -30 + rotationAdd)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    void Shoot()
    {
    
        cooldownTimer = cooldown; 
        GameObject bulletIns = Instantiate(bullet, shotPos.position, transform.rotation);
        Rigidbody2D rb = bulletIns.GetComponent<Rigidbody2D>();
        SoundManager.Instance.playRandom("a_salt_rifle", 0.1); 
            
    }
}
