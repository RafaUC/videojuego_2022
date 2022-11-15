using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public bool xInfinitetiling = false;
    public bool yInfiniteTiling = false;
    public CinemachineVirtualCamera cam;
    public Transform subject;

    public Vector2 startPos;
    public Vector2 originalSize;
    public Vector2 size => GetComponent<SpriteRenderer>().size;

    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPos;
    float distanceFromsubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromsubject > 0? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane));

    float parallaxFactor => Mathf.Abs(distanceFromsubject) / clippingPlane;
    

    public void Start(){
        originalSize = GetComponent<SpriteRenderer>().size;
        if(xInfinitetiling){
            GetComponent<SpriteRenderer>().size = new Vector2(originalSize.x*3,originalSize.y);
        }
        startPos = transform.position;
        startZ = transform.position.z;
    }

    public void Update(){
        
        Vector2 newPos = startPos + (travel * parallaxFactor);
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

        if(xInfinitetiling){
            if(Mathf.Abs(travel.x) > size.x/6*2){
                startPos = new Vector2(transform.position.x+(size.x/3*Mathf.Sign(travel.x)), startPos.y);
                transform.position = new Vector3(startPos.x,transform.position.y,transform.position.z);
                //startPos = transform.position;
            }
        }
    }
}
