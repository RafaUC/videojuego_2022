using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponData wd;
    [SerializeField] public GameObject attackPoint;
    [SerializeField] public GameObject bulletDefault;
    private GameObject parent;
    

    private float timeSinceShoot = 0f;
    // Start is called before the first frame update    
    void Start()
    {   
        //wd = Instantiate(wd);
        wd.attackReleased = true;
        wd.isReloading = false;
        attackPoint = transform.Find("AttackPoint").gameObject;
        parent = transform.parent.gameObject;
    }

    private bool CanAttack(){
        return ( !wd.isReloading && ((wd.attackReleased && wd.roundsPerSecond<0) || (timeSinceShoot > 1f/wd.roundsPerSecond && wd.roundsPerSecond>0)));
    }

    public void Attack(){
        if (CanAttack()){
            if (wd.loadedAmmo > 0){                
                wd.attackReleased = false;
                //invocar bullet
                switch (wd.type){
                    case 0:
                        Shoot0();
                        break;
                    case 1:
                        Shoot1();
                        break;
                    case 2:
                        break;
                }
                
                
                wd.loadedAmmo --;
                timeSinceShoot = 0;
                DoAfterAttack();
            }else {
                StartReload();
            }
        }
        
    }

    private void Shoot0(){
        GameObject newBullet = Instantiate(bulletDefault, attackPoint.transform.position, Quaternion.identity);
        newBullet.GetComponent<Attack>().SetAttackData(wd,parent);
        newBullet.transform.right = transform.right*parent.transform.localScale.x;
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.right*wd.velocity*parent.transform.localScale.x;  
        AudioManager.instance.Play(wd.soundName);
    }

    private void Shoot1(){        
        for(int i=0; i<wd.fieldOfAtack; i++){
            float disp = Random.Range(-wd.fieldOfAtack*0.025f,wd.fieldOfAtack*0.025f);
            GameObject newBullet = Instantiate(bulletDefault, attackPoint.transform.position, Quaternion.identity);
            newBullet.GetComponent<Attack>().SetAttackData(wd,parent);
            Vector2 temp = transform.right*parent.transform.localScale.x ;            
            temp.x = temp.x + (disp*Mathf.Sign(temp.x));
            temp.y = temp.y - (disp*Mathf.Sign(temp.y));            
            newBullet.transform.right = temp;                             
            newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.right*wd.velocity ;                  
        }
        AudioManager.instance.Play(wd.soundName);
    }

    private void DoAfterAttack(){
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        rb.velocity = rb.velocity + (Vector2)(transform.right*wd.recoil*parent.transform.localScale.x*-1);
    }

    public void StartReload(){
        if((wd.ammo > 0 || wd.maxAmmo < 0) && !wd.isReloading){
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload(){
        wd.isReloading = true;
        yield return new WaitForSeconds(wd.reloadTime);
        if(wd.maxAmmo >= 0){
            if(wd.ammo < wd.reloadSize){
                wd.loadedAmmo = wd.ammo;
                wd.ammo = 0;
            }else {
                wd.ammo -= wd.reloadSize;            
                wd.loadedAmmo = wd.reloadSize;
            }            
        }else {
            wd.loadedAmmo = wd.reloadSize;
        }            
        wd.isReloading = false;
    }

    public void AttackReleased(){
        wd.attackReleased = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceShoot += Time.deltaTime;
    }
}
