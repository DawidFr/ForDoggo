using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Special;

public class Grid<TGridObject> {
    public event EventHandler<OnGridObjectChangedEventsArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventsArgs : EventArgs{
        public int x;
        public int y;
    } 
    private int width, height;
    private float cellSize;
    private TGridObject[,] gridArray;
    //TextMesh[,] debugTextArray;
    private Vector3 originalPos;
    public Grid(int width, int height, float cellSize, Vector3 originalPos/*, Func<Grid<TGridObject>, int, int, TGridObject > createGridObject*/){
        this.cellSize = cellSize;
        this.width = width;
        this.height = height;
        this.originalPos = originalPos;
        gridArray = new TGridObject[width, height];
        //debugTextArray = new TextMesh[width, height]; 
        for(int x = 0; x < gridArray.GetLength(0); x++){
            for(int y = 0; y < gridArray.GetLength(1); y++){
                //debugTextArray[x,y] = SpecialFunction.CreateWorldText(gridArray[x,y].ToString(),null, GetWorldPosition(x,y,cellSize), (int) cellSize*5, Color.white, TextAnchor.MiddleCenter );
                
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.white, 100f);

    }
    public int GetHeight(){
        return height;
    }
    public int GetWidth(){
        return width;
    }
    public float GetCellSize(){
        return cellSize;
    }
    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x ,y ) * cellSize + originalPos;
    }
    public Vector3 GetWorldPosition(int x, int y, float cellSize){
        return new Vector3(x,y) * cellSize + originalPos + new Vector3(cellSize,cellSize)*0.5f;
    }
    private void SetValue(int x, int y, TGridObject value){
        if(x >= 0 && y >= 0 && x<width && y< height){
            gridArray[x,y] = value;
            //debugTextArray[x,y].text = value.ToString(); 
            if(OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventsArgs{x = x, y = y});
        }
    }
    private void GetXY(Vector3 wolrldPos ,out int x, out int y){
        x = Mathf.FloorToInt((wolrldPos - originalPos).x / cellSize);
        y = Mathf.FloorToInt((wolrldPos - originalPos).y / cellSize);
    }   
    private void SetValue(Vector3 worldPos, TGridObject value){
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x,y, value);
    }
    public TGridObject GetGridObject(int x, int y){
        if(x >= 0 && y >= 0 && x<width && y< height){
            return gridArray[x,y];
        }
        else return default(TGridObject);
    }
    public TGridObject GetGridObject(Vector3 worldPos){
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetGridObject(x,y);
        
    }
    public void TriggerGridObjectChanged(int x, int y){
        if(OnGridObjectChanged != null)OnGridObjectChanged(this, new OnGridObjectChangedEventsArgs{x = x, y = y});
    }
}
