using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector2 movementInput;
    public Vector2 cursorPosition;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private GameObject tail;
    private GameObject gun;
    private int isFliped = 1;
    

    [SerializeField] private InputActionReference movement, attack, cursorPos, jump;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tail = transform.Find("Tail").gameObject;
        gun = transform.Find("Gun").gameObject;
    }
    void FixedUpdate()
    {
        Utilities.ChangeVelocityByAceleration(rb,new Vector2(movHor*maxSpeed,rb.velocity.y),movInercia);
        
    }

    void Awake()
    {
        obj = this;
    }

    public void Jump(){
        if (!canJump) return;
        jumpSpareTimeCount = 0;
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //AudioManager.obj.playSaltar();
    }

    public void CancelJump(){
        if (isGrounded) return;
        if (rb.velocity.y < 0) return;
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/3f);
    }

    private void DoAtack(){

    }

    public void flip(float _xValue){
        Vector3 theScale = transform.localScale;
        if(_xValue < -0.2){
            theScale.x = Mathf.Abs(theScale.x)*-1;
            isFliped = -1;
        }else {
            if (_xValue > 0.2){
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

    public Vector2 GetCursorPos(){
        Vector3 currentCursorPos = cursorPos.action.ReadValue<Vector2>();
        currentCursorPos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(currentCursorPos);
    }

      

    public void UpdateGunRotation(){
        Vector2 rotation = ((cursorPosition-(Vector2)gun.transform.position)*isFliped).normalized;
        if(Mathf.Abs(rotation.y)<0.9f){
            gun.transform.right = rotation;
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {        
        cursorPosition = GetCursorPos();
        movementInput = movement.action.ReadValue<Vector2>();
        movHor = movementInput.x;
        //flip(movHor);   
        Vector2 targetDif = (cursorPosition-(Vector2)gun.transform.position);
        flip(targetDif.x);

        isGrounded = Physics2D.CircleCast(transform.position, radius, Vector3.down, groundRayDist, groundLayer);
        if(!isGrounded){
            jumpSpareTimeCount = jumpSpareTimeCount - Time.deltaTime;
        }else {            
            jumpSpareTimeCount = jumpSpareTime;
        }
        canJump = (jumpSpareTimeCount > 0);     

        if(jump.action.triggered){
            Jump();
        }
        if(jump.action.ReadValue<float>() == 0){
            CancelJump();
        }
        


        //Anbimation updates
        tail.transform.right = (rb.velocity*isFliped).normalized;        
        UpdateGunRotation();

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

        if((targetDif.x > 0 && rb.velocity.x < -0.27) || (targetDif.x < 0  && rb.velocity.x > 0.27)){
            anim.SetInteger("Backwards", 1);
        }else {
            anim.SetInteger("Backwards", 0);
        }
        
    }
}
