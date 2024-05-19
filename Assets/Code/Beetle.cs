using UnityEngine;

public class Beetle : Enemy
{
    public override void attack(Collision2D playerCollision)
    {
        Debug.Log("Beetle attacks");
        animator.SetTrigger(AnimationStates.attackTrigger);
        var player = playerCollision.gameObject;
        //Give damage to player
        var playerScript = (PlantV2)player.GetComponent(typeof(PlantV2));
        playerScript.takeDamage(attackDamage);

        //Push player back
        float playerXPos = player.transform.position.x;
        int direction = playerXPos > transform.position.x ? 1 : -1;
        playerScript.knockback(new Vector2(direction * attackKnockback * 10, 2 * attackKnockback));
        currentCooldown = attackCooldown;
    }

    public override void die()
    {
        transform.Rotate(Vector3.left * -180);
        //Shoot enemy upwards and then let it fall through the ground ground
        enemyRigidBody.AddForceY(5f, ForceMode2D.Impulse);
        enemyCollider.isTrigger = true;
        Destroy(gameObject, 3f);
        Debug.Log("Beetle dieded");
    }

    public override void takeBulletDamage(Collision2D bulletCollision)
    {
        //Multiply static bullet damage with relative bullet velocity to calculate actual damage
        int damage = Mathf.RoundToInt(3 * bulletCollision.relativeVelocity.magnitude);
        if (damage > 0)
        {
            animator.SetTrigger(AnimationStates.damageTrigger);
            showDamageEffect();
        }
        Debug.Log(string.Format("Beetle is taking {0} bullet damage ", damage));
        currentHealth -= damage;
    }

    public override void takeJumpDamage(Collision2D playerCollision)
    {
        animator.SetTrigger(AnimationStates.damageTrigger);
        showDamageEffect();
        int damage = (maxHealth / 2);
        Debug.Log(string.Format("Beetle is taking {0} jump damage ", damage));
        currentHealth -= damage;
    }
}
