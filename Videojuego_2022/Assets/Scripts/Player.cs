using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player obj;    
    public int lives = 3;
    public float maxSpeed = 13f;
    public float movInercia = 18f;
    public float movHor;        
    public float jumpForce = 25f;
    public float AnimSpeedDivisor = 6;
    public float jumpSpareTime = 0.4f;

    public bool isGrounded = false;
    public bool canJump = false;
    public LayerMask groundLayer;
    public float radius = 0.617f;
    public float groundRayDist = 0f;
    public float jumpSpareTimeCount;
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private GameObject tail;
    private int isFliped = 1;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tail = transform.Find("Tail").gameObject;
    }
    void FixedUpdate()
    {
        ChangeVelocityByAceleration(rb,new Vector2(movHor*maxSpeed,rb.velocity.y),movInercia);
        
    }

    void Awake()
    {
        obj = this;
    }

    public void jump(){
        if (!canJump) return;
        jumpSpareTimeCount = 0;
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        AudioManager.obj.playSaltar();
    }

    public void cancelJump(){
        if (rb.velocity.y < 0) return;
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/3f);
    }

    public void flip(float _xValue){
        Vector3 theScale = transform.localScale;
        if(_xValue < 0){
            theScale.x = Mathf.Abs(theScale.x)*-1;
            isFliped = -1;
        }else {
            if (_xValue > 0){
                theScale.x = Mathf.Abs(theScale.x);
                isFliped = 1;
            }
        }
        transform.localScale = theScale;
    }

    public Vector2 getPos(){
        return transform.position;
    }

    public void recibirDano(int num){
        lives = lives - num;
        AudioManager.obj.playHurt();
        if(lives <= 0){
            AudioManager.obj.playMuerte();            
            gameObject.SetActive(false);
            FXManager.obj.showPop(transform.position);
        }
    }

    public void ChangeVelocityByAceleration(Rigidbody2D rgb, Vector2 finalVelocity, float inercia){       
        if (rgb.velocity.x == finalVelocity.x && rgb.velocity.y == finalVelocity.y) return;

        
        
        float xDiference = (finalVelocity.x - rgb.velocity.x)/inercia;
        float yDiference = (finalVelocity.y - rgb.velocity.y)/inercia;                
        if(rgb.velocity.x < finalVelocity.x && xDiference < 0 || rgb.velocity.x > finalVelocity.x && xDiference > 0){ //La fuerza agregada saldria del la fuerza deseada, en eje x
            rgb.velocity = new Vector2(finalVelocity.x,rgb.velocity.y);
            xDiference = 0f;
        }
        if(rgb.velocity.y+yDiference < finalVelocity.y && yDiference < 0 || rgb.velocity.y+yDiference > finalVelocity.y && yDiference > 0){
            rgb.velocity = new Vector2(rgb.velocity.x,finalVelocity.y);
            yDiference = 0f;
        }        
        rgb.velocity = new Vector2(rb.velocity.x + xDiference, rb.velocity.y + yDiference);
    
    }

    // Update is called once per frame
    void Update()
    {        
        movHor = Input.GetAxisRaw("Horizontal");
        flip(movHor);        
        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);
        if(!isGrounded){
            jumpSpareTimeCount = jumpSpareTimeCount - Time.deltaTime;
        }else {            
            jumpSpareTimeCount = jumpSpareTime;
        }
        canJump = (jumpSpareTimeCount > 0);

        if(Input.GetKeyDown(KeyCode.Space)){
            jump();
        }
        if(Input.GetKeyUp(KeyCode.Space) && !isGrounded){
            cancelJump();
        }   


        //Anbimation updates
        tail.transform.right = (rb.velocity*isFliped).normalized;
        switch((movHor,isGrounded)){
            
            case (_, false):
                anim.SetInteger("Running", 2);
                anim.speed = Mathf.Abs(rb.velocity.x)/AnimSpeedDivisor/5;
                break;
            case var expression when (rb.velocity.x < -0.27 || rb.velocity.x > 0.27):
                anim.SetInteger("Running", 2);
                anim.speed = Mathf.Abs(rb.velocity.x)/AnimSpeedDivisor;
                break;
            case (0f, true):
                anim.SetInteger("Running", 0);
                break;
            default:
                anim.SetInteger("Running", 2);
                anim.speed = Mathf.Abs(rb.velocity.x)/AnimSpeedDivisor;
                break;

        }
        
    }
}
