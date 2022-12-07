using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFish : EnemySeeker
{
    public override void SetTarget(GameObject newTarget)
    {
        //Do nothing
    }

    public override void searchPlayer()
    {
        //Do nothing
    }

    public override void OnCollisionEnter2D(Collision2D collission){
        I_Damagable damagable = collission.gameObject.GetComponent<I_Damagable>();
        if (damagable != null && player.GetComponent<I_Damagable>() == damagable){            
            damagable.Damage(1);
        }
    }
}
