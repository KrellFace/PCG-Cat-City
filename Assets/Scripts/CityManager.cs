using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public script_BlockSpawner blockSpawner;


    private int[, ,] testMatrix = new int[,,] {{{1, 0, 1 , 1}, { 1, 0, 1 , 1}, { 2, 0, 1 , 1} }, 
                                 { { 0, 0, 1 , 1}, { 0, 0, 1 , 1},{ 0, 0, 1 , 1},},
                                 { { 0, 0, 1 , 1}, { 0, 0, 0 , 0},{ 0, 0, 0 , 0},},
                                 { { 0, 0, 1 , 1}, { 0, 0, 1 , 1},{ 0, 0, 2 , 2},}};

    // Start is called before the first frame update
    void Start()
    {

        blockSpawner.spawnBlocks(testMatrix);
    }

}
