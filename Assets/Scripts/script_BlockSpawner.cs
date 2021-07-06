using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_BlockSpawner : MonoBehaviour
{
    public GameObject basicBlock;


    public void spawnBlocks(int[,,] inputMatrix){

        Vector3 defaultSpawnPosition = this.transform.position;

        for (int x = 0; x<inputMatrix.GetLength(0); x++){
            for (int y = 0; y<inputMatrix.GetLength(1); y++){
                for (int z = 0; z<inputMatrix.GetLength(2); z++){
                    Debug.Log("Checking cell: " + x + "," + y + "," + z);
                    Debug.Log("Cell value: " + inputMatrix[x,y,z]);
                    //If we find a 1 in our matrix, spawn a block at our position, offset by its matrix position
                    if (inputMatrix[x,y,z] == 1){
                        
                        //Get spawn position
                        Vector3 spawnPos = new Vector3(defaultSpawnPosition.x+x, defaultSpawnPosition.y+y, defaultSpawnPosition.z+z);
                        Instantiate(basicBlock, spawnPos, this.transform.rotation, this.transform);
                    }

                }
            }
        }
    }

}
