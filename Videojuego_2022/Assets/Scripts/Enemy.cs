using UnityEngine;

public class Enemy : MonoBehaviour, I_Damagable
{
    public static Enemy obj;
    public float hp = 3f;
    public float speed = 2f; 
    public float movHor; //Se moverá sólo (cambiar el valor de 1 a -1 )
    public int scoreGive = 50;
    public bool mustTurn = false;

    public bool isGroundedFloor = false;
    public bool isGroundFront = false;
    public LayerMask groundLayer; 
    public Transform groundCheckPos;
    public Collider2D bodyCollider;

    protected Rigidbody2D rb; 
    public Animator anim; 
    protected SpriteRenderer spr;

    protected GameObject player;
    protected Player playerScript;
    
    public float lineOfSite;
    

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        playerScript = (Player) player.GetComponent(typeof(Player));
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerScript.Damage(1);
            Flip();
            movHor = movHor * -1;
            //anim.SetFloat("Horizontal", movHor);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
            movHor = movHor * -1;
            //anim.SetFloat("Horizontal", movHor);
        }
        /**if(collision.collider.CompareTag("Border"))
        {
            Debug.Log("Colisión con con algún objeto");
            movHor = movHor * -1;
            anim.SetFloat("Horizontal", movHor);
            //hasCollision = true; //Sí hay una colisión con alguna plataforma
        }*/
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false); //El antangonista desaparece 
            //fx_manager.obj.showPop(transform.position);
        }
    }
    
    public virtual void Damage(float damage){
        hp = hp - damage;        
        if(hp <= 0){            
            Die();
        }
    }

    public virtual void Die(){
        Destroy(this.gameObject);       
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
        mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }

    void Update()
    {
        if(mustTurn == true || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
            movHor = movHor * -1;
        }

        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if(distanceFromPlayer < lineOfSite)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed*Time.deltaTime);
            speed = 6f; 
            movHor = 0;
        }
        else{
            if(movHor == 0)
            {
                Start();
            }
        }
    }

}