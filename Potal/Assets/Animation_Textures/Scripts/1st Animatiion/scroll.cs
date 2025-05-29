//TPN GAMES 2024 All Right Reseved.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TPNGAMES
{
    


    


 public class scroll : MonoBehaviour
 {    

    public float ScrollX =  0.5f;
    public float ScrollY = 0.5f;
    private string nameCom = "TPNGAMES";
    


    

    void Update()
    {
        if (nameCom ==  ("TPNGAMES")){

            
            float OffsetY = Time.time * ScrollY;
            float OffsetX = Time.time * ScrollX;
            GetComponent<Renderer> (). material.mainTextureOffset = new Vector2 (OffsetX,OffsetY);
            

        }
        else
        {
            Debug.Log("Error. YouChange Original OwnerName in the Scipt!");
        }

    }

    
 }

}