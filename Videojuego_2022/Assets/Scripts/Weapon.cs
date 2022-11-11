using System.Collections;
using System.Collections.Generic;
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
                GameObject newBullet = Instantiate(bulletDefault, attackPoint.transform.position, Quaternion.identity);
                newBullet.GetComponent<Attack>().SetAttackData(wd,parent);
                newBullet.transform.right = transform.right*parent.transform.localScale.x;
                newBullet.GetComponent<Rigidbody2D>().velocity = transform.right*wd.velocity*parent.transform.localScale.x;                
                
                
                wd.loadedAmmo --;
                timeSinceShoot = 0;
                DoAfterAttack();
            }else {
                StartReload();
            }
        }
        
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
