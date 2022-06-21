using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Special;

public class DungeonGenerator : MonoBehaviour
{   
    [SerializeField] RoomGeneration[] startRooms;
    [SerializeField] RoomGeneration[] endRooms;
    [SerializeField] RoomGeneration[] chestRooms;
    [SerializeField] RoomGeneration[] specialRooms;
    [SerializeField] RoomGeneration[] normalRooms; 
    [SerializeField] private Vector3Int gridSize;
    private List<Vector3> positionsList = new List<Vector3>();
    private Grid<int> grid;
    private Vector3 startRoomPos, endRoomPos, specialRoomPos, chestRoomPos;
    private bool control = true;
    private void Awake() {
        grid = new Grid<int>(gridSize.x , gridSize.y, gridSize.z, new Vector3(transform.position.x - gridSize.x * gridSize.z /2, transform.position.y - gridSize.y * gridSize.z/2, 0));
        startRoomPos = GetRandomGridPos();
        endRoomPos = GetRandomGridPos();
        specialRoomPos = GetRandomGridPos();
        chestRoomPos = GetRandomGridPos();
        Instantiate(MyRandom.GetObject<RoomGeneration>(startRooms).gameObject, startRoomPos, Quaternion.identity);
        Instantiate(MyRandom.GetObject<RoomGeneration>(endRooms).gameObject, endRoomPos, Quaternion.identity);
        Instantiate(MyRandom.GetObject<RoomGeneration>(specialRooms).gameObject, specialRoomPos, Quaternion.identity);
        Instantiate(MyRandom.GetObject<RoomGeneration>(chestRooms).gameObject, chestRoomPos, Quaternion.identity);
        for(int x = 2; x <= 4; x += 2){
            for(int y = 2; y <= 4; y+= 2){
                Instantiate(MyRandom.GetObject<RoomGeneration>(normalRooms), grid.GetWorldPosition(x,y,gridSize.z), Quaternion.identity);
            }
        }

    }






    private int[] i = new int[]{2,4};
    private Vector3 GetRandomGridPos(){
        int x,y;
        bool control = true;
        Vector3 pos = Vector3.zero;
        while(control){
            if(MyRandom.GetRandomBool()){
                x = Special.MyRandom.GetObject<int>(i);
                if(MyRandom.GetRandomBool()){
                    y = 0;
                } 
                else{
                    y = gridSize.y - 1;
                } 
            }
            else{
                y = Special.MyRandom.GetObject<int>(i);
                if(MyRandom.GetRandomBool()){
                    x = 0;
                } 
                else{
                    x = gridSize.x - 1;
                }  
            }  
            pos = grid.GetWorldPosition(x, y, gridSize.z);
            control = false;
            foreach(Vector3 positions in positionsList){
                if(pos == positions){
                    control = true;
                    break;
                } 
            }            
        }
        positionsList.Add(pos);
        return pos;
    }
}
