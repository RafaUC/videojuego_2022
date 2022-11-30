using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected GameObject player;
    protected Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = (Player) player.GetComponent(typeof(Player));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Destroy(this.gameObject);
        if(collider.gameObject.CompareTag("Player"))
        {   
            playerScript.Damage(1);
        }
    }
}
