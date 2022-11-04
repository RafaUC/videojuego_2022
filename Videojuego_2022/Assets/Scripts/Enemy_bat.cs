using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_bat : MonoBehaviour
{
    

    public static Enemy_bat obj;
    public float inercia = 0f;
    public float inerciaX = 0f;
    public Vector2 initPosition;
    public float ineAceleracion = 0.3f;
    public float range = 3f;
    public float speed = 8f;
    public Vector2 direction = Vector2.up;
    public float MovVer = 1f;
    public float MovHor = 0f;
    
    private GameObject player;
    private Player playerScript;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initPosition = transform.position;
        player = GameObject.FindWithTag("Player");
        playerScript = (Player) player.GetComponent(typeof(Player));
    }

    void awake()
    {
        obj = this;
    }

    void OnCollisionEnter2D(Collision2D collission){
        if (collission.gameObject.CompareTag("Player")){
            Debug.Log("Daño Player");
            playerScript.recibirDano(1);
        }
    }

    public void flip(float _xValue){
        Vector3 theScale = transform.localScale;
        if(_xValue > 0){
            theScale.x = Mathf.Abs(theScale.x)*-1;
        }else {
            if (_xValue < 0){
                theScale.x = Mathf.Abs(theScale.x);
            }
        }
        transform.localScale = theScale;
    }

    public void caclMovVer(){
        if(transform.position.y < initPosition.y){
            MovVer = 1f;
        }else if(transform.position.y > initPosition.y+range){
            MovVer = -1f;
        }
    }

    public void caclMovHor(){
        if(transform.position.x < initPosition.x){
            MovHor = 1f;
        }else if(transform.position.x > initPosition.x){
            MovHor = -1f;
        }
    }

    public void actualizarInercia(){     
        caclMovVer();  
        caclMovHor(); 
        float tempSpeed = MovVer * speed;
        float tempSpeedX = MovHor * speed;
        if (tempSpeed == 0){
            if(inercia<0){
                inercia = inercia+ineAceleracion;
            }else if (inercia>0){
                inercia = inercia-ineAceleracion;
            }
        }else if ((tempSpeed < 0 && inercia > tempSpeed) || (tempSpeed > 0 && inercia < tempSpeed)){
            inercia = inercia + (MovVer * ineAceleracion);
        }
        if (tempSpeedX == 0){
            if(inerciaX<0){
                inerciaX = inerciaX+ineAceleracion;
            }else if (inerciaX>0){
                inerciaX = inerciaX-ineAceleracion;
            }
        }else if ((tempSpeedX < 0 && inerciaX > tempSpeedX) || (tempSpeedX > 0 && inerciaX < tempSpeedX)){
            inerciaX = inerciaX + (MovHor * ineAceleracion);
        }
    }

    public void setPlayerPos(){
         initPosition = player.transform.position;
    }

    void FixedUpdate(){        
        setPlayerPos();
        actualizarInercia();
        rb.velocity = new Vector2(rb.velocity.x, inercia);
        rb.velocity = new Vector2(inerciaX, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collission){
        if (collission.gameObject.CompareTag("Player")){
            AudioManager.obj.playMuerte();    
            gameObject.SetActive(false);
            FXManager.obj.showPop(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        flip(MovHor);
    }
}
