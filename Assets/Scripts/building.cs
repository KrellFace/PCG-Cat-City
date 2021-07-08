using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class building
{
    private int[] northWestCorner = new int[2];
    private int[] southEastCorner = new int[2];
    private int height;
    private int[] xyOffset = new int[2];

    public building(int[] NWCorner, int[] SECorner, int height){
        this.northWestCorner = NWCorner;
        this.southEastCorner = SECorner;
        this.height = height;
    }

    public void printInfo(){
        Debug.Log("Buidling NWC: " + northWestCorner[0] + "," + northWestCorner[1] + ". Buidling SEC: " + southEastCorner[0] + "," + southEastCorner[1] +
            " Building height: " + height);
    }

    public int getHeight(){
        return height;
    }

    public int[] getNWCorner(){
        return northWestCorner;
    }

    public int[] getSECorner(){
        return southEastCorner;
    }

    public void setxyOffset(int[] offset){
        xyOffset = offset;
    }

    public int[] getxyOffset(){
        return xyOffset;
    }
}
