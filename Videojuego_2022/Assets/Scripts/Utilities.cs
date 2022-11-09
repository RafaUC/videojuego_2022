using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static void ChangeVelocityByAceleration(Rigidbody2D rgb, Vector2 finalVelocity, float inercia){       
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
        rgb.velocity = new Vector2(rgb.velocity.x + xDiference, rgb.velocity.y + yDiference);
    
    }  
}
