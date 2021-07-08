﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public script_BlockSpawner blockSpawner;

    public script_MapElitesGenerator mapElites;

    public GameObject playerCatPrefab;
    public GameObject mainCamera;
    public Canvas canvas;

    private BuildingGenerator buildingGenerator;

    private enum Direction{
        north,
        south,
        west,
        east
    }

    public void GenerateCity()
    {
        StartCoroutine(GenerateCityCoroutine());
    }


    IEnumerator GenerateCityCoroutine()
    {
        mapElitesTown[,] townGrid = mapElites.runMapElites();

        //Spawn blocks in MAP Elites style grid formation
        spawnBlocksGridForm(townGrid, true);

        //Spawn them in a spiral
        //spawnBlocksSpiralForm(townGrid, true);

        //Thomas, your chunk should hopefully be able to plug in here
        //This method generates all of the buildings in the right formation, but passing in false stops it generating my ugly blocks, leaving the space clear for WFC cleverness
        //Comment out the above spawnBlocksSpiralForm() method when you're ready to hook into this
        
        ArrayList buildingList = spawnBlocksSpiralForm(townGrid, false);

        buildingGenerator = gameObject.GetComponent<BuildingGenerator>();
        buildingGenerator.Init();

        int test_iter=0;
        foreach  (building b in buildingList){

            int height = b.getHeight();
            int[] originCorner = b.getAbsNWCorner();
            Debug.Log("originCorner[0]:" + originCorner[0] + " originCorner[1]:" + originCorner[1]);

            buildingGenerator.Generate(originCorner[0]*5, originCorner[1]*5);

            Debug.Log("foreach success");

            if (test_iter==1) {
                break;
            }
            test_iter ++;
        }

        mainCamera.SetActive(false);
        canvas.enabled = false;
        Instantiate(playerCatPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        yield return null;
    }

    private ArrayList spawnBlocksGridForm(mapElitesTown[,] meGrid, bool spawnBlocks){
        int xoffset = 0;
        int zoffset = 0;

        ArrayList allBuildings = new ArrayList();

        //Generate block representations in same form as MAP Elite grid
        for (int x = 0; x<meGrid.GetLength(0); x++){
            for (int y = 0; y<meGrid.GetLength(1); y++){
                
                //Check map elites grid cell has a town in it
                if (meGrid[x,y]!=null){
                    if (spawnBlocks){
                        blockSpawner.spawnBlocks2D(meGrid[x,y].getRepresentation(), xoffset, zoffset);
                    }
                    meGrid[x,y].updateBuildingOffset(new int[]{xoffset, zoffset});
                    allBuildings.AddRange(meGrid[x,y].getBuildingList());
                }
                else {
                    //Debug.Log("Final cell empty");
                }
                xoffset += 11;
            }
            zoffset+= 11;
            xoffset = 0;
        }

        return allBuildings;

    }

    private ArrayList spawnBlocksSpiralForm(mapElitesTown[,] meGrid, bool spawnBlocks){
        int xoffset = 0;
        int zoffset = 0;

        ArrayList allBuildings = new ArrayList();

        Direction currDirection = Direction.east;
        //Number of times we have done each side (we need to do each length twice for a spiral)
        int sideCount = 0;
        //Store the current side length of the spiral we're drawning
        int currentSideLength=1;
        //Store the current number of steps we have done of current side
        int stepCount = 0;
        
        //Amount we separate each chunk spawn by
        int offsetAmount = 12;

        for (int x = meGrid.GetLength(0)-1; x>0; x--){
            for (int y = meGrid.GetLength(1)-1; y>0; y--){
                
                //Check map elites grid cell has a town in it
                if (meGrid[x,y]!=null){
                    if (spawnBlocks){
                        blockSpawner.spawnBlocks2D(meGrid[x,y].getRepresentation(), xoffset, zoffset);
                    }
                    meGrid[x,y].updateBuildingOffset(new int[]{xoffset, zoffset});
                    allBuildings.AddRange(meGrid[x,y].getBuildingList());

                    if(currDirection == Direction.east){
                        xoffset+=offsetAmount;
                    }
                    else if(currDirection == Direction.north){
                        zoffset+=offsetAmount;
                    }
                    else if(currDirection == Direction.west){
                        xoffset-=offsetAmount;
                    }
                    else if(currDirection == Direction.south){
                        zoffset-=offsetAmount;
                    }

                    //If we still have progress to make on this side, keep going
                    if (stepCount<currentSideLength){
                        stepCount+=1;
                    }
                    //If were at the end of the side, change direction
                    else {
                        stepCount = 0;
                        sideCount+=1;

                        if(currDirection == Direction.east){
                            currDirection=Direction.north;
                        }
                        else if(currDirection == Direction.north){
                            currDirection=Direction.west;
                        }
                        else if(currDirection == Direction.west){
                            currDirection=Direction.south;
                        }
                        else if(currDirection == Direction.south){
                            currDirection=Direction.east;
                        }

                        //If we've done this side length twice, increase target length
                        if (sideCount==2){
                            currentSideLength+=1;
                            sideCount =0;
                        }
                    }
                }
            }
        }

        return allBuildings;

    }


}
