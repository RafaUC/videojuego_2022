using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager obj{get; private set;}
    
    public GameObject pop;

    private void Awake(){
        obj = this;
    }

    public void showPop(Vector2 pos){
        //pop.gameObject.GetComponent<pop>().show(pos);
    }
}
