using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy obj;
    public float speed = 2f; 
    public float movHor; //Se moverá sólo (cambiar el valor de 1 a -1 )
    public int scoreGive = 50;
    public bool mustTurn = false;

    public bool isGroundedFloor = false;
    public bool isGroundFront = false;
    public LayerMask groundLayer; 
    public Transform groundCheckPos;
    public Collider2D bodyCollider;

    private Rigidbody2D rb; 
    public Animator anim; 
    private SpriteRenderer spr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //player.obj.getDamage();
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

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movHor * speed, rb.velocity.y);
        mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
    }

    void Update()
    {
        if(mustTurn == true || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
            movHor = movHor * -1;
        }
    }

}