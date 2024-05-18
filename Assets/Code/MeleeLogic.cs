using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    private float cooldown = 0.5f;
    private float cooldownTimer = 0;
    private int damage = 3;

    private float range = 0.75f;
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && transform.childCount == 0)
        {
            Hit();
        }
    }

    void Hit()
    {
        //Debug.Log("Trying to hit " + range);
        cooldownTimer = cooldown;    
        // check if an enemy is in range (Tag = "Enemy")
        // if so, deal damage to the enemy
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position, transform.right, range);
        Debug.DrawRay(transform.position, transform.right * range, Color.red, 2f);
        foreach (RaycastHit2D hit in raycastHits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().takeMeleeDamage(damage);
            }
        }
        
    }
}