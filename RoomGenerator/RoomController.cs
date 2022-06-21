using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] TunnelControler tunnelControler;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "NormalRoom"){
            other.GetComponent<RoomGeneration>().AddTunnel(GenerateTunnel());
        }            
        

        
    }
    public GameObject GenerateTunnel(){
        Destroy(gameObject);
        return tunnelControler.GenerateTunnel();
        
    }

}
