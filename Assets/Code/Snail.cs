
using UnityEngine;

public class Snail : Enemy
{

    public override void attack(Collision2D collision)
    {

        animator.SetTrigger(AnimationStates.attackTrigger);
        Debug.Log("Snail attacks");
        var player = collision.gameObject;
        var playerScript = (Plant)player.GetComponent(typeof(Plant));
        playerScript.takeDamage(attackDamage);
        currentCooldown = attackCooldown;
    }

    public override void die()
    {
        transform.Rotate(Vector3.left * -180);
        //Shoot enemy upwards and then let it fall through ground
        enemyRigidBody.AddForceY(5f, ForceMode2D.Impulse);
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 5f);
        Debug.Log("Snail dieded");
    }

    public override void takeBulletDamage(Collision2D collision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        //Multiply static bullet damage with relative bullet velocity to calculate actual damage
        int damage = Mathf.RoundToInt(2 * collision.relativeVelocity.magnitude);
        Debug.Log(string.Format("Snail is taking {0} bullet damage ", damage));
        currentHealth -= damage;
    }

    public override void takeJumpDamage(Collision2D collision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        int damage = (maxHealth / 2);
        Debug.Log(string.Format("Snail is taking {0} jump damage ", damage));
        currentHealth -= damage;
    }
}
