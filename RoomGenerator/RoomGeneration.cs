using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Special;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] wallSprites;
    public Vector3Int roomSize;
    #region SpriteRendererLists
    List<SpriteRenderer> floorSprites = new List<SpriteRenderer>(); 
    //full edges
    List<SpriteRenderer> downEdgesSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> topEdgesSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> leftEdgesSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> rightEdgesSprite = new List<SpriteRenderer>();
    //edges without door
    List<SpriteRenderer> downEdgesWithoutDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> topEdgesWithoutDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> leftEdgesWithoutDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> rightEdgesWithoutDoorSprite = new List<SpriteRenderer>();
    //only door
    List<SpriteRenderer> downEdgesDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> topEdgesDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> leftEdgesDoorSprite = new List<SpriteRenderer>();
    List<SpriteRenderer> rightEdgesDoorSprite = new List<SpriteRenderer>();
    #endregion
    public Sprite[] wall;
    public GameObject[] floorSpritesObject;
    private Grid<SpriteRenderer> grid;
    public Vector4 openDirection;
    
    void Start()
    {
        grid = new Grid<SpriteRenderer>(roomSize.x, roomSize.y, roomSize.z, new Vector3(transform.position.x - roomSize.x * roomSize.z /2, transform.position.y - roomSize.y * roomSize.z/2, 0));
        for(int x = 1; x < roomSize.x - 1; x++){
            for(int y = 1; y < roomSize.y - 1; y++){
                GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(floorSpritesObject), grid.GetWorldPosition(x , y, roomSize.z), Quaternion.identity);
                wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
                wall.transform.parent = transform;
                floorSprites.Add(wall.GetComponent<SpriteRenderer>());
            }
        }
        //down edge SetUp
        for(int x = 0; x < roomSize.x; x++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(x , 0, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            downEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(x == roomSize.x/2 || x == roomSize.x/2-1) downEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else downEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
        ChangeDownEdgeSprite(wall[0]);
        //top edge SetUp
        for(int x = 0; x < roomSize.x; x++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(x , roomSize.y - 1, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            topEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(x == roomSize.x/2 || x == roomSize.x/2-1) topEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else topEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
        ChangeTopEdgeSprite(wall[0]);
        //left edge SetUp
        for(int y = 0; y < roomSize.y; y++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(0 , y, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            leftEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(y == roomSize.y/2 || y == roomSize.y/2-1) leftEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else leftEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
        ChangeLeftEdgeSprite(wall[0]);
        //right edge SetUp
        for(int y = 0; y < roomSize.y; y++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(roomSize.x-1 , y, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            rightEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(y == roomSize.y/2 || y == roomSize.y/2-1) rightEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else rightEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
        ChangeRightEdgeSprite(wall[0]);

        RefreashOpenDirection();
        //ChangeFloorSprite(floors);
    }

    public void RefreashOpenDirection(){
        if(openDirection.x == 1){
            ChangeTopEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeTopEdgeDoorSprite(null);
        }
        if(openDirection.y == 1){
            ChangeRightEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeRightEdgeDoorSprite(null);
        }
        if(openDirection.z == 1){
            ChangeDownEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeDownEdgeDoorSprite(null);
        }
        if(openDirection.w == 1){
            ChangeLeftEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeLeftEdgeDoorSprite(null);
        }

    }
    public void RefreashOpenDirection(Vector4 newDirection){
        openDirection = newDirection;
        RefreashOpenDirection();
    }

































































    public void ChangeFloorSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in floorSprites){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeFloorSprite(Sprite[] sprites){
        foreach(SpriteRenderer spriteRenderer in floorSprites){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = MyRandom.GetObject<Sprite>(sprites);
        }
    }
    public void ChangeDownEdgeSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in downEdgesSprite){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeTopEdgeSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in topEdgesSprite){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeLeftEdgeSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in leftEdgesSprite){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeRightEdgeSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in rightEdgesSprite){
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeDownEdgeWithoutDoorSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in downEdgesWithoutDoorSprite){
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeTopEdgeWithoutDoorSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in topEdgesWithoutDoorSprite){
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeLeftEdgeWithoutDoorSprite(Sprite sprite){
        foreach(SpriteRenderer spriteRenderer in leftEdgesWithoutDoorSprite){
            spriteRenderer.sprite = sprite;
        }
    }
    public void ChangeRightEdgeWithoutDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in rightEdgesWithoutDoorSprite){
                spriteRenderer.sprite = sprite;
            }            
        }
        else{
            foreach(SpriteRenderer spriteRenderer in rightEdgesWithoutDoorSprite){
                spriteRenderer.gameObject.SetActive(false);
            }   
        }

    }
    public void ChangeDownEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in downEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(true);
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in downEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(false);
            }   
        }

    }
    public void ChangeTopEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in topEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(true);
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in topEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(false);
            }  
        }
    }
    public void ChangeLeftEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in leftEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(true);
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in leftEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(false);
            }  
        }
    }
    public void ChangeRightEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in rightEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(true);
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in rightEdgesDoorSprite){
                spriteRenderer.gameObject.SetActive(false);
            }  
        }
    }

}
