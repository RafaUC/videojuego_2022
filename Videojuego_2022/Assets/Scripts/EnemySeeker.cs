using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySeeker : Enemy
{
    public float maxSpeed = 20f;
    public float movInercia = 32f;
    public Vector2 mov;
    public float timer = 0f;
    public Vector2 wonderingTarget;
    public float wonderingRange = 10f;
    public float playerDetectionRange = 35f;

    public Vector2 targetPos;
    public GameObject target;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //SetTarget(player);
        timer = Random.Range(0f, 3f);
        wonderingTarget = transform.position;
    }

    void FixedUpdate(){
        if(target != null){
            Utilities.ChangeVelocityByAceleration(rb,mov*maxSpeed,movInercia);
        }else {
            Utilities.ChangeVelocityByAceleration(rb,mov*(maxSpeed/2),movInercia);
        }
        
    }

    public void flip(float _xValue){
        Vector3 theScale = transform.localScale;
        if(_xValue < -0.2){
            theScale.x = Mathf.Abs(theScale.x)*-1;            
        }else {
            if (_xValue > 0.2){
                theScale.x = Mathf.Abs(theScale.x);                
            }
        }
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMov();
        flip(rb.velocity.x);
    }    

    public virtual void SetTarget(GameObject newTarget){
        target = newTarget;
        AudioManager.instance.Play("PufferAngry");
    }

    public virtual void searchPlayer(){
        if (target != null) return;
        Vector3 offset = player.GetComponent<Collider2D>().bounds.size;
        offset = new Vector3(0,offset.y,0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            (player.transform.position - transform.position + offset).normalized, playerDetectionRange, 
            LayerMask.GetMask("ground","Player","Default"));
        Debug.DrawRay(transform.position, (player.transform.position - transform.position + offset), Color.red);
        if(hit){            
            if (hit.transform.GetComponent<Player>() != null){
                SetTarget(player);
            }
        }
        
        

    }

    public void UpdateMov(){
        if(target != null){ //seeking
            anim.SetInteger("Chasing", 1);
            Vector3 offset = target.GetComponent<Collider2D>().bounds.size * 0.7f;
            mov = (target.transform.position + offset - transform.position).normalized;        
            if(rb.velocity.magnitude>maxSpeed/4){
                timer = 0;
            }
            if(timer > 1.5){
                target = null;
            }
        }else { //Wondering behavior
            anim.SetInteger("Chasing", 0);
            if(timer > 3){
                timer = 0;
                Utilities.RandomVector2(ref wonderingTarget, (Vector2)transform.position - (Vector2.one * wonderingRange), (Vector2)transform.position + (Vector2.one * wonderingRange));
                
            }            
            mov = (wonderingTarget - (Vector2)transform.position);               
            if(timer % 1 < 0.06){
                searchPlayer();
            }
        }
        timer += Time.deltaTime;
        if (mov.magnitude < 1){
            mov = Vector2.zero;
        }else {
            mov = mov.normalized;        
        }
                
    }

    void OnCollisionEnter2D(Collision2D collission){
        I_Damagable damagable = collission.gameObject.GetComponent<I_Damagable>();
        if (damagable != null && target != null && target.GetComponent<I_Damagable>() == damagable){            
            damagable.Damage(1);
        }
    }

    public override void Damage(float damage){
        AudioManager.instance.Play("PufferHurt");
        base.Damage(damage);        
        //rb.velocity = rb.velocity * 0.7f;
    }

    public override void Die()
    {
        AudioManager.instance.Play("PufferDie");
        base.Die();
    }

}
