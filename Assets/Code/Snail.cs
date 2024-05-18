
using UnityEngine;

public class Snail : Enemy
{
    public override void attack(Collision2D playerCollision)
    {
        Debug.Log("Snail attacks");
        animator.SetTrigger(AnimationStates.attackTrigger);
        var player = playerCollision.gameObject;

        //Give damage to player
        var playerScript = (Plant)player.GetComponent(typeof(Plant));
        playerScript.takeDamage(attackDamage);
        
        //Push player back
        float playerXPos = player.transform.position.x;
        int direction = playerXPos > transform.position.x ? 1 : -1;
        knockbackPlayer(direction * attackKnockback, 2 * attackKnockback);
        
        currentCooldown = attackCooldown;
    }

    public override void die()
    {
        transform.Rotate(Vector3.left * -180);
        //Shoot enemy upwards and then let it fall through the ground ground
        enemyRigidBody.AddForceY(5f, ForceMode2D.Impulse);
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 3f);
        Debug.Log("Snail dieded");
    }

    public override void takeBulletDamage(Collision2D bulletCollision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        showDamageEffect();
        //Multiply static bullet damage with relative bullet velocity to calculate actual damage
        int damage = Mathf.RoundToInt(3 * bulletCollision.relativeVelocity.magnitude);
        Debug.Log(string.Format("Snail is taking {0} bullet damage ", damage));
        currentHealth -= damage;
    }

    public override void takeJumpDamage(Collision2D playerCollision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        showDamageEffect();
        int damage = (maxHealth / 2);
        Debug.Log(string.Format("Snail is taking {0} jump damage ", damage));
        currentHealth -= damage;
    }
}
