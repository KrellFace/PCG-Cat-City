﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingGenerator : MonoBehaviour
{
    public WFC scriptWFC;
    public NavMeshSurface surface;
    
    public GameObject piece_A1;
    public GameObject piece_A2;
    public GameObject piece_B1;
    public GameObject piece_B2;
    public GameObject piece_B3;
    public GameObject piece_B4;
    public GameObject piece_C1;
    public GameObject piece_C2;
    public GameObject piece_C3;

    //public int xOffset = 0;
    //public int zOffset = 0;
    public float yOffset = 2f;
    public float grid_spacing = 2f;

    Dictionary<(int,int,int), string> structureDict = new Dictionary<(int,int,int), string>(); 
    Dictionary<string, GameObject> pieceDict = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    /*
    IEnumerator Start()
    {
        Generate();

        yield return null;
    }*/
    
    public void Init () {
        TestInitializeStructure();
        InitializeDictionary();
    }
    
    public void Generate(int xOffset, int zOffset) // xOffset and zOffset provided by CityManager per building site
    {
        WFC WFC = new WFC();
        WFC.Initialize(5,1,1);
        WFC.RunWFC();
        //WFC.PrintVisitableSpace();
        Dictionary<(int,int,int), string> structureDictFromWFC = WFC.GetFinalStructure();

        Vector3 defaultSpawnPosition = this.transform.position;
        foreach(KeyValuePair<(int,int,int), string> entry in structureDict)
        //foreach(KeyValuePair<(int,int,int), string> entry in structureDictFromWFC)
        {
            if ( (entry.Value!="G") && (entry.Value!="N") && (entry.Value!="X") && (entry.Value!="T") ) {
                //Debug.Log( string.Format("{0},{1},{2}: {3}", entry.Key.Item1, entry.Key.Item2, entry.Key.Item3, entry.Value) );

                int piece_X = entry.Key.Item1;
                int piece_Y = entry.Key.Item3 - 1;
                int piece_Z = entry.Key.Item2;

                Vector3 spawnPos = new Vector3(defaultSpawnPosition.x + piece_X*grid_spacing, 
                                            defaultSpawnPosition.y + piece_Y*grid_spacing + yOffset,
                                            defaultSpawnPosition.z + piece_Z*grid_spacing);
                Instantiate(pieceDict[entry.Value], spawnPos, this.transform.rotation, this.transform);
            }
            
        }

        surface.BuildNavMesh();
    }

    void TestInitializeStructure()
    {
        /*
        structureDict.Add( (1,1,1), "A2" );
        structureDict.Add( (1,1,2), "C1" );
        structureDict.Add( (1,2,1), "A1" );
        structureDict.Add( (1,2,2), "C1" );
        structureDict.Add( (2,1,1), "A2" );
        structureDict.Add( (2,1,2), "C3" );
        structureDict.Add( (2,2,1), "A1" );
        structureDict.Add( (2,2,2), "C1" );
        */

        structureDict.Add( (1,1,1), "A1" );
        structureDict.Add( (1,1,2), "B1" );
        structureDict.Add( (1,1,3), "C1" );

        structureDict.Add( (1,4,1), "A1" );
        structureDict.Add( (1,4,2), "B4" );
        structureDict.Add( (1,4,3), "C3" );

        structureDict.Add( (2,1,1), "A1" );
        structureDict.Add( (2,1,2), "B2" );
        structureDict.Add( (2,1,3), "B2" );
        structureDict.Add( (2,1,4), "C1" );

        structureDict.Add( (3,1,1), "A1" );
        structureDict.Add( (3,1,2), "B3" );
        structureDict.Add( (3,1,3), "B4" );
        structureDict.Add( (3,1,4), "C3" );

        structureDict.Add( (3,2,1), "A1" );
        structureDict.Add( (3,2,2), "B3" );
        structureDict.Add( (3,2,3), "C2" );

        structureDict.Add( (3,3,1), "A1" );
        structureDict.Add( (3,3,2), "C2" );
        
        structureDict.Add( (3,4,1), "A2" );
        structureDict.Add( (3,4,2), "B2" );
        structureDict.Add( (3,4,3), "C1" );

        structureDict.Add( (4,1,1), "A1" );
        structureDict.Add( (4,1,2), "B4" );
        structureDict.Add( (4,1,3), "B4" );
        structureDict.Add( (4,1,4), "C3" );
    }
    
    void InitializeDictionary()
    {
        pieceDict.Add( "A1", piece_A1 );
        pieceDict.Add( "A2", piece_A2 );
        pieceDict.Add( "B1", piece_B1 );
        pieceDict.Add( "B2", piece_B2 );
        pieceDict.Add( "B3", piece_B3 );
        pieceDict.Add( "B4", piece_B4 );
        pieceDict.Add( "C1", piece_C1 );
        pieceDict.Add( "C2", piece_C2 );
        pieceDict.Add( "C3", piece_C3 );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
