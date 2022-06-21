using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Special;
using Vector4Bool = Special.MyStruct.Vector4Bool;

public class RoomGeneration : MonoBehaviour
{
    public GameObject[] wallSprites;
    private List<GameObject> tunnels = new List<GameObject>();
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
    public enum RoomType{
        NormalRoom,
        Tunnel,
    }
    public RoomType roomType;
    public GameObject[] floorSpritesObject;
    public Sprite[] door;
    private Grid<SpriteRenderer> grid;
    public Vector4Bool openDirection;
    public bool generateDownWall, generateTopWall, generateLeftWall, generateRightWall;
    private BoxCollider2D boxCollider;
    public bool canBeDestroyed = true;
    private static bool oneRoomDeleated;
    private void Awake()
    {
        oneRoomDeleated = false;
        boxCollider = GetComponent<BoxCollider2D>();
        grid = new Grid<SpriteRenderer>(roomSize.x, roomSize.y, roomSize.z, new Vector3(transform.position.x - roomSize.x * roomSize.z /2, transform.position.y - roomSize.y * roomSize.z/2, 0));
        if(boxCollider != null)boxCollider.size = new Vector2(roomSize.x-4 , roomSize.y-4);
        for(int x = 0; x < roomSize.x ; x++){
            for(int y = 0; y < roomSize.y; y++){
                GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(floorSpritesObject), grid.GetWorldPosition(x , y, roomSize.z), Quaternion.identity);
                wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
                wall.transform.parent = transform;
                floorSprites.Add(wall.GetComponent<SpriteRenderer>());
            }
        }
        if(generateDownWall) GenerateDownWall();
        if(generateLeftWall) GenerateLeftWall();
        if(generateRightWall) GenerateRightWall();
        if(generateTopWall) GenerateTopWall();

        RefreashOpenDirection();
        StartCoroutine(Check());
        //ChangeFloorSprite(floors);
    }

    public Vector3[] GetCellPos(){
        Vector3[] cellPos = new Vector3[(roomSize.x-2) * (roomSize.y - 2)];
        int i = 0;
        for(int x = 1; x < roomSize.x - 1; x++){
            for(int y = 1; y < roomSize.y - 1; y++){
                cellPos[i] = grid.GetWorldPosition(x , y, roomSize.z);  
                i++;
            }
        }
        return cellPos;
    }
    public void RefreashOpenDirection(){
        if(!openDirection.x){
            ChangeTopEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeTopEdgeDoorSprite(null);
        }
        if(!openDirection.y){
            ChangeRightEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeRightEdgeDoorSprite(null);
        }
        if(!openDirection.z){
            ChangeDownEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeDownEdgeDoorSprite(null);
        }
        if(!openDirection.w){
            ChangeLeftEdgeDoorSprite(wall[0]);
        }
        else{
            ChangeLeftEdgeDoorSprite(null);
        }

    }
    public void RefreashOpenDirection(Vector4Bool newDirection){
        openDirection = newDirection;
        RefreashOpenDirection();
    }
    public void ChangeOpenDirectionX(bool newBool){
        openDirection.x = newBool;
        RefreashOpenDirection();
    }
    public void ChangeOpenDirectionY(bool newBool){
        openDirection.y = newBool;
        RefreashOpenDirection();
    }
    public void ChangeOpenDirectionZ(bool newBool){
        openDirection.z = newBool;
        RefreashOpenDirection();
    }
    public void ChangeOpenDirectionW(bool newBool){
        openDirection.w = newBool;
        RefreashOpenDirection();
    }
    public void GenerateTopWall(){
        for(int x = 0; x < roomSize.x; x++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(x , roomSize.y - 1, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            topEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(x == roomSize.x/2 || x == roomSize.x/2-1 || x == roomSize.x/2+1 || x == roomSize.x/2-2) topEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else topEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
    }
    public void GenerateDownWall(){
        //down edge SetUp
        for(int x = 0; x < roomSize.x; x++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(x , 0, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            downEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(x == roomSize.x/2 || x == roomSize.x/2-1 || x == roomSize.x/2+1 || x == roomSize.x/2-2) downEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else downEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
    }
    public void GenerateLeftWall(){
        for(int y = 0; y < roomSize.y; y++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(0 , y, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            leftEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(y == roomSize.y/2 || y == roomSize.y/2-1 || y == roomSize.y/2+1 || y == roomSize.y/2-2)  leftEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else leftEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
    }
    public void GenerateRightWall(){
        for(int y = 0; y < roomSize.y; y++){
            GameObject wall = Instantiate(MyRandom.GetObject<GameObject>(wallSprites), grid.GetWorldPosition(roomSize.x-1 , y, roomSize.z), Quaternion.identity);
            wall.transform.localScale = new Vector3(roomSize.z, roomSize.z, 1);
            wall.transform.parent = transform;
            rightEdgesSprite.Add(wall.GetComponent<SpriteRenderer>());
            if(y == roomSize.y/2 || y == roomSize.y/2-1 || y == roomSize.y/2+1 || y == roomSize.y/2-2) rightEdgesDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
            else rightEdgesWithoutDoorSprite.Add(wall.GetComponent<SpriteRenderer>());
        }
    }

    public void CloseDoor(){
        if(openDirection.x){
            ChangeTopEdgeDoorSprite(door[0]);
        }
        if(openDirection.y){
            ChangeRightEdgeDoorSprite(door[1]);
        }
        if(openDirection.z){
            ChangeDownEdgeDoorSprite(door[0]);
        }
        if(openDirection.w){
            ChangeLeftEdgeDoorSprite(door[1]);
        }
    }
    public void OpenDoor(){
        if(openDirection.x){
            ChangeTopEdgeDoorSprite(null);
        }
        if(openDirection.y){
            ChangeRightEdgeDoorSprite(null);
        }

        if(openDirection.z){
            ChangeDownEdgeDoorSprite(null);

        }
        if(openDirection.w){
            ChangeLeftEdgeDoorSprite(null);
        }
    }
    private void Update() {
        if(roomType == RoomType.NormalRoom){
            if(Input.GetKeyDown(KeyCode.O)) OpenDoor();
            if(Input.GetKeyDown(KeyCode.C)) CloseDoor();            
        }

    }
    public void AddTunnel(GameObject tunnel){
        tunnels.Add(tunnel);
    }
    public void ChangeDestroyAble(bool newBool){
        canBeDestroyed = newBool;
        StartCoroutine(Check());
    }
    public IEnumerator Check() {
        yield return new WaitForEndOfFrame();
        if(canBeDestroyed){
            if(roomType == RoomType.NormalRoom && !oneRoomDeleated){
                oneRoomDeleated = true;
                foreach(GameObject tunnel in tunnels){
                    RoomGeneration roomGeneration = tunnel.GetComponent<RoomGeneration>();
                    yield return new WaitForSeconds(1);
                    roomGeneration.ChangeDestroyAble(true);
                }  
                yield return new WaitForSecondsRealtime(0.1f);
                Destroy(gameObject);              
            }
            else if(roomType == RoomType.Tunnel){
                tunnelsControlers = GetComponentsInChildren<TunnelControler>();
                foreach(TunnelControler tunnelControler in tunnelsControlers){
                    tunnelControler.ChangeColliderDirection();
                }
                yield return new WaitForSecondsRealtime(0.1f);
                Destroy(gameObject);
            }

        }
    }
    TunnelControler[] tunnelsControlers;

































































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
                spriteRenderer.enabled = true;
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = true;
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in downEdgesDoorSprite){
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = false;
                spriteRenderer.enabled = false;
            }   
        }

    }
    public void ChangeTopEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in topEdgesDoorSprite){
                spriteRenderer.enabled = true;
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = true;
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in topEdgesDoorSprite){
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = false;
                spriteRenderer.enabled = false;
            }  
        }
    }
    public void ChangeLeftEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in leftEdgesDoorSprite){
                spriteRenderer.enabled = true;
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = true;
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in leftEdgesDoorSprite){
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = false;
                spriteRenderer.enabled = false;
            }  
        }
    }
    public void ChangeRightEdgeDoorSprite(Sprite sprite){
        if(sprite != null){
            foreach(SpriteRenderer spriteRenderer in rightEdgesDoorSprite){
                spriteRenderer.enabled = true;
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = true;
                spriteRenderer.sprite = sprite;
            }
        }
        else{
            foreach(SpriteRenderer spriteRenderer in rightEdgesDoorSprite){
                spriteRenderer.enabled = false;
                BoxCollider2D collider2D = spriteRenderer.transform.GetComponent<BoxCollider2D>();
                collider2D.enabled = false;
            }  
        }
    }

}
