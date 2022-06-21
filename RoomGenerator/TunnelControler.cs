using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelControler : MonoBehaviour
{
    [SerializeField] LayerMask roomLayerMask;
    [SerializeField] GameObject Tunnel;
    [SerializeField] bool castCollision;
    [SerializeField] int direction;
    private GameObject gameObjectCollider;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "NonCastCollider"){
            gameObjectCollider = other.gameObject;
            if(castCollision){
                TunnelControler controler = other.GetComponent<TunnelControler>();
                GameObject tunnel = controler.GenerateTunnel(false);
                ChangeParentOpenDirection(tunnel, false);
                Destroy(gameObject);
            }
            else ChangeParentOpenDirection();
            
        }
        if(other.tag == "NormalRoom"){
            gameObjectCollider = other.gameObject;
        }            
        

        
    }
    public GameObject GenerateTunnel(){
        GameObject tunnel =  Instantiate(Tunnel, transform.position, Quaternion.identity);
        ChangeParentOpenDirection(tunnel);
        Destroy(gameObject);
        return tunnel;
    }
    public GameObject  GenerateTunnel(bool canBeDestroyed){
        GameObject tunnel = Instantiate(Tunnel, transform.position, Quaternion.identity);
        ChangeParentOpenDirection(tunnel , canBeDestroyed);
        Destroy(gameObject);
        return tunnel;
    }

    private void ChangeParentOpenDirection(){
        RoomGeneration roomGeneration= GetComponentInParent<RoomGeneration>();
        switch(direction){
            case 1:{
                roomGeneration.ChangeOpenDirectionX(true);
                break;
            }
            case 2:{
                roomGeneration.ChangeOpenDirectionY(true);
                break;
            }
            case 3:{
                roomGeneration.ChangeOpenDirectionZ(true);
                break;
            }
            case 4:{
                roomGeneration.ChangeOpenDirectionW(true);
                break;
            }
            default: break;
        }
    }
    private void ChangeParentOpenDirection(GameObject tunnel){
        RoomGeneration roomGeneration= GetComponentInParent<RoomGeneration>();
        roomGeneration.AddTunnel(tunnel);
        switch(direction){
            case 1:{
                roomGeneration.ChangeOpenDirectionX(true);
                break;
            }
            case 2:{
                roomGeneration.ChangeOpenDirectionY(true);
                break;
            }
            case 3:{
                roomGeneration.ChangeOpenDirectionZ(true);
                break;
            }
            case 4:{
                roomGeneration.ChangeOpenDirectionW(true);
                break;
            }
            default: break;
        }
    }
    private void ChangeParentOpenDirection(GameObject tunnel , bool canBeDestroyed){
        RoomGeneration roomGeneration= GetComponentInParent<RoomGeneration>();
        roomGeneration.AddTunnel(tunnel);
        roomGeneration.canBeDestroyed = canBeDestroyed;
        switch(direction){
            case 1:{
                roomGeneration.ChangeOpenDirectionX(true);
                break;
            }
            case 2:{
                roomGeneration.ChangeOpenDirectionY(true);
                break;
            }
            case 3:{
                roomGeneration.ChangeOpenDirectionZ(true);
                break;
            }
            case 4:{
                roomGeneration.ChangeOpenDirectionW(true);
                break;
            }
            default: break;
        }
    }
    public void ChangeColliderDirection(){
        RoomGeneration roomGeneration;
        roomGeneration = Physics2D.OverlapCircle(transform.position , 1, roomLayerMask).GetComponent<RoomGeneration>();            
        switch(direction){
            case 1:{
                roomGeneration.ChangeOpenDirectionX(false);
                break;
            }
            case 2:{
                roomGeneration.ChangeOpenDirectionY(false);
                break;
            }
            case 3:{
                roomGeneration.ChangeOpenDirectionZ(false);
                break;
            }
            case 4:{
                roomGeneration.ChangeOpenDirectionW(false);
                break;
            }
            default: break;
        }
    }
}
