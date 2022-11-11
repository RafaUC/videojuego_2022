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
        
        //Modifica el rango y la forma del ataque
        switch(wd.type){
            case 2:            
                break;
            case 1:            
                break;
            default: //Es ataque tipo disparo
                transform.localScale = transform.localScale*wd.fieldOfAtack;
                break;

        }
        
    }

    void OnCollisionEnter2D(Collision2D collission){

        // if (collission.gameObject.CompareTag("Player")){
        //     Debug.Log("Daño Player");
        //     playerScript.recibirDano(1);
        // }

        
        Debug.Log(collission.collider.gameObject.name);
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
