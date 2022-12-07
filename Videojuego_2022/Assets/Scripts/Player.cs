using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, I_Damagable
{
    public static Player obj;    
    public float hp = 3f;
    public float maxSpeed = 13f;
    public float movInercia = 18f;
    public float movHor;        
    public float jumpForce = 25f;
    public float AnimSpeedDivisor = 6;
    public float jumpSpareTime = 0.4f;
    public float weaponRotationLimitY = 0.9f;

    public bool isGrounded = false;
    public int canJump = 0;
    public LayerMask groundLayer;
    public Vector2 boxSize = new Vector2(1.17f,0.27f);
    public float groundRayDist = 0f;
    public float jumpSpareTimeCount;
    public Vector2 movementInput;
    public Vector2 cursorPosition;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;    
    private int isFliped = 1;

    private GameObject weapon;
    private GameObject tail;    
    private GameObject midle;
    private Weapon WeaponScript;
    

    [SerializeField] private InputActionReference movement, attack, cursorPos, jump;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tail = transform.Find("Tail").gameObject;
        weapon = transform.Find("Gun").gameObject;
        midle = transform.Find("Midle").gameObject;
        WeaponScript = weapon.GetComponent<Weapon>();
    }
    void FixedUpdate()
    {
        Utilities.ChangeVelocityByAceleration(rb,new Vector2(movHor*maxSpeed,rb.velocity.y),movInercia);
        midle.transform.position = transform.position + ((Vector3)cursorPosition - transform.position)/3;
    }

    void Awake()
    {
        obj = this;
    }

    public void Damage(float damage){
        hp = hp - damage;        
        if(hp <= 0){            
            AudioManager.instance.ReturnMenu();
            this.gameObject.SetActive(false);
            
            //Destroy(this.gameObject);       
        }
    }


    public void Jump(){
        if (canJump != 2 || jumpSpareTimeCount < 0) return;
        jumpSpareTimeCount = 0;
        canJump = 1;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //AudioManager.obj.playSaltar();
    }

    public void CancelJump(){        
        if (rb.velocity.y < 0 || isGrounded || canJump != 1) return;
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y/3f);
        canJump = 0;
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

    
    public Vector2 GetCursorPos(){
        Vector3 currentCursorPos = cursorPos.action.ReadValue<Vector2>();
        currentCursorPos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(currentCursorPos);
    }

      

    public void UpdateWeaponRotation(){
        Vector2 rotation = ((cursorPosition-(Vector2)weapon.transform.position)*isFliped).normalized;
        if(Mathf.Abs(rotation.y)< weaponRotationLimitY){
            weapon.transform.right = rotation;
        }
        
    }
    
    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position,boxSize);
    }

    // Update is called once per frame
    void Update()
    {        
        cursorPosition = GetCursorPos();
        
        movementInput = movement.action.ReadValue<Vector2>();
        movHor = movementInput.x;
        //flip(movHor);   
        Vector2 targetDif = (cursorPosition-(Vector2)weapon.transform.position);
        flip(targetDif.x);

        isGrounded = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector3.zero, groundRayDist, groundLayer);        
        if(!isGrounded){
            jumpSpareTimeCount = jumpSpareTimeCount - Time.deltaTime;
        }else {            
            jumpSpareTimeCount = jumpSpareTime;
        }
        if (jumpSpareTimeCount > 0) {
            canJump = 2;
        }else {
            //canJump = 1;
        }
        

        if(jump.action.triggered){
            Jump();
        }
        if(jump.action.ReadValue<float>() == 0){
            CancelJump();
        }

        UpdateWeaponRotation();
        if(attack.action.ReadValue<float>() == 1){
            WeaponScript.Attack();
        }
        if(attack.action.ReadValue<float>() == 0){
            WeaponScript.AttackReleased();
        }
        


        //Anbimation updates
        tail.transform.right = (rb.velocity*isFliped).normalized;        
        

        switch((movHor,isGrounded)){
            
            case (_, false):
                anim.SetInteger("Running", 2);
                anim.speed = Mathf.Abs(rb.velocity.x)/AnimSpeedDivisor/5;
                break;
            case var expression when ((rb.velocity.x < -0.27 || rb.velocity.x > 0.27)):
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

        if((targetDif.x > 0 && rb.velocity.x < -0.7) || (targetDif.x < 0  && rb.velocity.x > 0.7)){
            anim.SetInteger("Backwards", 1);
        }else {
            anim.SetInteger("Backwards", 0);
        }
        
    }
}
