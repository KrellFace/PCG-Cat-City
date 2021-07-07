using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public script_BlockSpawner blockSpawner;

    public script_MapElitesGenerator mapElites;


    /*
    private int[, ,] testMatrix = new int[,,] {{{1, 0, 1 , 1}, { 1, 0, 1 , 1}, { 2, 0, 1 , 1} }, 
                                 { { 0, 0, 1 , 1}, { 0, 0, 1 , 1},{ 0, 0, 1 , 1},},
                                 { { 0, 0, 1 , 1}, { 0, 0, 0 , 0},{ 0, 0, 0 , 0},},
                                 { { 0, 0, 1 , 1}, { 0, 0, 1 , 1},{ 0, 0, 2 , 2},}};
    */

    //private int[,] twoDtestMatrix = new int[,]{ { 1, 2, 3}, { 1, 2, 3}, { 1, 2, 3}, { 1, 2, 3} };

    // Start is called before the first frame update
    void Start()
    {

        //blockSpawner.spawnBlocks(testMatrix);
        //blockSpawner.spawnBlocks2D(twoDtestMatrix);

        //Test method, loop through the full contents of a map elites grid, generate each town in series with an offset between
        mapElitesTown[,] townGrid = mapElites.runMapElites();

        int xoffset = 0;
        int zoffset = 0;

        for (int x = 0; x<townGrid.GetLength(0); x++){
            for (int y = 0; y<townGrid.GetLength(1); y++){
                
                //Check map elites grid cell has a town in it
                if (townGrid[x,y]!=null){
                    //Debug.Log("Generating final town chunk with stats:");
                    //Debug.Log("Fitness: " + townGrid[x,y].getFitness());
                    //Debug.Log("TotalHeight: " + townGrid[x,y].getTotalHeight());
                    //Debug.Log("Street Count: " + townGrid[x,y].getStreetCount());

                    blockSpawner.spawnBlocks2D(townGrid[x,y].getRepresentation(), xoffset, zoffset);
                }
                else {
                    //Debug.Log("Final cell empty");
                }
                xoffset += 11;

            }
            //Generate the chunks 
            zoffset+= 11;
            xoffset = 0;
        }

    }

}
