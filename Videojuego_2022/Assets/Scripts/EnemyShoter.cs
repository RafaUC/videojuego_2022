using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShoter : MonoBehaviour
{
    public float Range;
    public Transform Target;
    public bool Detected;
    Vector2 Direction;
    //public GameObject Star;
    public GameObject Bullet;
    public float FireRate;
    float nextTimetoFire = 0;
    public Transform ShootPos;
    public float force;
 
    void Start()
    {

    }

    void Update()
    {
        Vector2 targetpos = Target.position;
        Direction = targetpos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if(rayInfo)
        {
            if(rayInfo.collider.gameObject.CompareTag("Player"))
            {
                if(Detected == false)
                {
                    Detected = true;
                    Debug.Log("Player detected");
                }
            }
            else
            {
                if(Detected == true)
                {
                    Detected = false;
                    Debug.Log("Player not detected anymore");
                }
            }
        }
        if(Detected)
        {
            if(Time.time > nextTimetoFire)
            {
                nextTimetoFire = (Time.time + 1) / FireRate;
                Shoot();
            }
            //star.transform.up = Direction;
        }
    }

    void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPos.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * force);
    }


    void OnDrawGizmosSelected() //Se muestra el rango que tiene de alcance el enemigo
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
