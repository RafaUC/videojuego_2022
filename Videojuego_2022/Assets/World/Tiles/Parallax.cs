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

    public Vector2 ofSet;
    public Vector2 pivotOfSet;

    public float startZ;

    Vector2 travel;
    float distanceFromsubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromsubject > 0? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane));

    float parallaxFactor => Mathf.Abs(distanceFromsubject) / clippingPlane;
    
    public float parallasx;
    

    public void Start(){
        originalSize = GetComponent<SpriteRenderer>().size;
        if(xInfinitetiling){
            GetComponent<SpriteRenderer>().size = new Vector2(originalSize.x*2,originalSize.y);
            pivotOfSet = new Vector2(size.x/2,size.y/2);
        }
        
        startPos = transform.position;
        
        ofSet = (Vector2)cam.transform.position - startPos;
        startZ = transform.position.z;
    }

    

    public void Update(){
        parallasx = parallaxFactor;
        travel = (Vector2)cam.transform.position - startPos - ofSet;

        Vector2 newPos = startPos + (travel * parallaxFactor);
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

        if(xInfinitetiling){
            if(cam.transform.position.x - (transform.position.x + pivotOfSet.x) > size.x/4){
                startPos = new Vector2(transform.position.x+(size.x/2), startPos.y);
                ofSet.x = ((Vector2)cam.transform.position).x - startPos.x;
                transform.position = new Vector3(startPos.x,transform.position.y,transform.position.z);
                //startPos = transform.position;
            }else if(cam.transform.position.x - (transform.position.x + pivotOfSet.x) < -size.x/4){
                startPos = new Vector2(transform.position.x-(size.x/2), startPos.y);
                ofSet.x = ((Vector2)cam.transform.position).x - startPos.x;
                transform.position = new Vector3(startPos.x,transform.position.y,transform.position.z);
            }
        }
    }
}
