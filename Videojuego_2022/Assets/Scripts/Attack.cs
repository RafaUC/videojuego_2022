using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public WeaponData wd;
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake(){
        lifeTime = 50f;
    }

     public void SetAttackData(WeaponData weaponData, GameObject sourceToIgnore){
        wd = weaponData;        
        lifeTime = wd.reach/wd.velocity;
        Physics2D.IgnoreCollision(sourceToIgnore.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        GetComponent<Rigidbody2D>().mass = wd.mass;
        
        //Modifica el rango y la forma del ataque
        switch(wd.type){
            case 2:            
                break;
            case 1: //Ataque tipo escopeta
                transform.localScale = transform.localScale*wd.damage;
                break;
            default: //Es ataque tipo disparo
                transform.localScale = transform.localScale*wd.fieldOfAtack;
                break;

        }
        
    }

    void OnCollisionEnter2D(Collision2D collission){

        
        I_Damagable damagable = collission.gameObject.GetComponent<I_Damagable>();
        if (damagable != null){            
            damagable.Damage(wd.damage);
        }
        

                
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0 ){
            Destroy(this.gameObject);
        }        
    }
}
