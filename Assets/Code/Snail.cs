
using UnityEngine;

public class Snail : Enemy
{

    public override void attack(Collision2D collision)
    {

        animator.SetTrigger(AnimationStates.attackTrigger);
        Debug.Log("Snail Attack");
        currentCooldown = attackCooldown;
    }

    public override void die()
    {
        transform.Rotate(Vector3.left * -180);
        //Shoot enemy upwards and then let it fall through ground
        enemyRigidBody.AddForceY(5f, ForceMode2D.Impulse);
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 5f);
    }

    public override void takeBulletDamage(Collision2D collision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        //Multiply static bullet damage with relative bullet velocity to calculate actual damage
        int damage = Mathf.RoundToInt(2 * collision.relativeVelocity.magnitude);
        Debug.Log("Snail is taking bullet damage: " + damage);
        currentHealth -= damage;
    }

    public override void takeJumpDamage(Collision2D collision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        int damage = (maxHealth / 2);
        Debug.Log("Snail is taking jump damage: " + damage);
        currentHealth -= damage;
    }
}
