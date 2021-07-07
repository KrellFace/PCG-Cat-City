using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public script_BlockSpawner blockSpawner;

    public script_MapElitesGenerator mapElites;

    void Start(){
        GenerateCity();
    }

    private void GenerateCity()
    {

        //blockSpawner.spawnBlocks(testMatrix);
        //blockSpawner.spawnBlocks2D(twoDtestMatrix);

        //Test method, loop through the full contents of a map elites grid, generate each town in series with an offset between
        
        mapElitesTown[,] townGrid = mapElites.runMapElites();

        int xoffset = 0;
        int zoffset = 0;

        ArrayList allBuildings = new ArrayList();

        //Generate block representations in same form as MAP Elite grid
        for (int x = 0; x<townGrid.GetLength(0); x++){
            for (int y = 0; y<townGrid.GetLength(1); y++){
                
                //Check map elites grid cell has a town in it
                if (townGrid[x,y]!=null){

                    blockSpawner.spawnBlocks2D(townGrid[x,y].getRepresentation(), xoffset, zoffset);
                    townGrid[x,y].updateBuildingOffset(new int[]{xoffset, zoffset});
                    allBuildings.AddRange(townGrid[x,y].getBuildingList());
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




        //Testing stuff to ignore
        /*
        mapElitesTown[] testDuo = mapElites.runMapElites();

        blockSpawner.spawnBlocks2D(testDuo[0].getRepresentation(), 0, 0);
        blockSpawner.spawnBlocks2D(testDuo[1].getRepresentation(), 0, 20);
        ArrayList testBuildingList = testDuo[1].getBuildingList();

        foreach(building building in testBuildingList){
            building.printInfo();
        }
        */


    }


}
