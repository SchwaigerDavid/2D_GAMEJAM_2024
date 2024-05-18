

using UnityEngine;

public class Snail : Enemy
{

    public override void attack(Collision2D collision)
    {

        animator.SetTrigger(AnimationStates.attackTrigger);
        Debug.Log("Snail Attack");
        currentCooldown = attackCooldown;
    }
    
}
