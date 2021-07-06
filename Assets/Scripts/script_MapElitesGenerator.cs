using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_MapElitesGenerator : MonoBehaviour
{
    //Parameters

    // How many generative steps we do before termination
    private int stepCount = 50000;
    //Size of Map elites grid. Always a square n by n grid
    private int gridSize = 10;

    //Size of generated towns. Always a square n by n grid
    private int townSize = 10;

    //Behavioral Metric Parameters
    private int minStreeTiles = 20;
    private int maxStreetTiles = 80;

    private int minTotalHeight = 100;
    private int maxTotalHeight = 400;

    //Maximum allowable building height
    private int maxBuildingHeight = 8;
    private float tileMutateChance = 0.5f;


    //Main method for generating the output town
    public mapElitesTown[,] runMapElites(){

        mapElitesTown[,] mapElitesGrid = generateStartingGrid();

        //MAP Elites core loop#
        
        for (int i = 0; i <stepCount; i++){

            Debug.Log("Step count: " + i + ". Current populated cells: " + getPopulatedCellCount(mapElitesGrid));

            mapElitesTown currTown = getRandomLevelFromGrid(mapElitesGrid);
            //Mutate it
            mapElitesTown newTown = tileMutate(currTown);
            //Add it back to the grid
            addToGrid(mapElitesGrid, newTown);


        }
        return mapElitesGrid;
    }

    //Generate random starting town
    private mapElitesTown generateRandomTown(){

        int[,] town = new int[townSize,townSize];

        //Random rnd = new Random();

        for (int x = 0; x<town.GetLength(0); x++){
            for (int z = 0; z<town.GetLength(1); z++){
                //50% of being street, 50% chance of random height building
                if (Random.Range(1, 100)>50){
                    town[x,z] = Random.Range(1, maxBuildingHeight);
                }
                else{
                    town[x,z] = 0;
                }
            }
        }

        return new mapElitesTown((int[,]) town.Clone());
            
    }

    //Generate starting Map Elites grid
    private mapElitesTown[,] generateStartingGrid(){

        mapElitesTown[,] mapElitesGrid = new mapElitesTown[gridSize,gridSize];

        mapElitesTown[] startingPop = generateRandomPopulation(20);

        //Loop through each population
        for (int popLoc = 0; popLoc<startingPop.GetLength(0); popLoc++){
            //Loop through each cell in map elites map to see if it belongs in specified cell
            //If it does, check fitness of current entrant, replace it if its less fit
            mapElitesTown currTown = startingPop[popLoc];

            addToGrid(mapElitesGrid, currTown);
        }
        
        return mapElitesGrid;
    }

    private mapElitesTown[] generateRandomPopulation(int popSize){

        mapElitesTown[] returnPop = new mapElitesTown[popSize];

        for (int i = 0; i < returnPop.GetLength(0); i++){
            returnPop[i] = generateRandomTown();
        }

        return returnPop;

    }

    private void addToGrid(mapElitesTown[,] mapElitesGrid, mapElitesTown townToAdd){

        for (int x = 0; x<mapElitesGrid.GetLength(0); x++){
            for (int y = 0; y<mapElitesGrid.GetLength(1); y++){
                if(checkBelongs(townToAdd, x, y)){
                    if (mapElitesGrid[x,y] == null || mapElitesGrid[x,y].getFitness() < townToAdd.getFitness()){
                        mapElitesGrid[x,y] = townToAdd;
                        Debug.Log("Town added to map elites grid at location: " + x + "," + y);
                    }
                }
            }
        }        
    }

    private bool checkBelongs(mapElitesTown townToCheck, int xLoc, int yLoc){

        int streetCountRange = maxStreetTiles - minStreeTiles;
        int totalHeightRange = maxTotalHeight - minTotalHeight;

        int townSC = townToCheck.getStreetCount();
        int townTH = townToCheck.getTotalHeight();

        float localSCmin = ((streetCountRange/gridSize)*xLoc) + minStreeTiles;
        float localSCmax = ((streetCountRange/gridSize)*(xLoc+1)) + minStreeTiles;

        float localTHmin = ((totalHeightRange/gridSize)*yLoc) + minTotalHeight;
        float localTHmax = ((totalHeightRange/gridSize)*(yLoc+1)) + minTotalHeight;

        //Debug.Log("Local SC range: " + localSCmin + "," + localSCmax + " and town SC: " + townSC);
        //Debug.Log("Local TH range: " + localTHmin + "," + localTHmax + " and town th: " + townTH);

        if(townSC>localSCmin&&townSC < localSCmax && townTH > localTHmin && townTH < localTHmax){
            //Debug.Log("Local SC range: " + localSCmin + "," + localSCmax + " and town SC: " + townSC);
            //Debug.Log("Local TH range: " + localTHmin + "," + localTHmax + " and town th: " + townTH);
            return true;
        }
        else{
            return false;
        }
    }

    private mapElitesTown getRandomLevelFromGrid(mapElitesTown[,] inputMap){

        bool selected =false ;

        mapElitesTown returnLevel = null;

        int checkCount = 0;
        int checkLimit = 100;
        //Added the check count so it doesnt loop forever and crash when i do something wrong 
        while (!selected&&checkCount < checkLimit){
            int randx = Random.Range(0, gridSize);
            int randy = Random.Range(0, gridSize);

            if (inputMap[randx, randy] != null){
                returnLevel = inputMap[randx, randy].Clone();
                selected = true;
            }

            checkCount+=1;
        }

        if (checkCount == checkLimit){
            Debug.Log("Failed to find a random level in grid. Returning null");
        }

        return returnLevel;


    }

    //Basic mutation method. Takes in a town and has a chance of increasing or decreasing the height of each cell
    private mapElitesTown tileMutate(mapElitesTown input){
        int[,] townRep = (int[,]) input.getRepresentation().Clone();

        for (int x = 0; x<townRep.GetLength(0); x++){
            for (int y = 0; y<townRep.GetLength(1); y++){

                if (Random.Range(0, 1) < tileMutateChance){
                    
                    //50 50 chance of it getting higher or lower
                     if (Random.Range(0,10)<5){
                         if (townRep[x,y]>0){
                            //Debug.Log("Reducing height");
                            townRep[x,y] -=1;
                         }
                     }
                     else {
                        if (townRep[x,y]<maxBuildingHeight){
                            //Debug.Log("Increasing height");
                            townRep[x,y] +=1;
                        }
                     }
                }
            }
        }
        return new mapElitesTown(townRep);
    }

    private int getPopulatedCellCount(mapElitesTown[,] inputGrid){
        int count = 0;

        for (int x = 0; x<inputGrid.GetLength(0); x++){
            for (int y = 0; y<inputGrid.GetLength(1); y++){
                if(inputGrid[x,y]!= null){
                    count+=1;
                }
            }
        }

        return count;
    }

}
