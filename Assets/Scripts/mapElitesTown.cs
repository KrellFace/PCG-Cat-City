using System.Collections;
using System.Collections.Generic;


//Class for storing a town in 2D matrix form, along with details about it
public class mapElitesTown
{
    private int[,] townRepresentation;

    private int fitness;
    private int streetCount;
    private int totalHeight;


    public mapElitesTown(int[,] townRep){
        townRepresentation =(int[,]) townRep.Clone();

        calculateMetrics(townRep);
    }

    //Fitness is based on cells which are next to other cells with the same value
    //Should push the search towards buildings with flat tops, and contiguous streets
    private void calculateMetrics(int[,] townRep){
        int localFitness = 0;
        int localStreetCount = 0;
        int localTotalHeight = 0;
        for (int x = 0; x<townRep.GetLength(0); x++){
            for (int y = 0; y<townRep.GetLength(1); y++){
                int cellValue = townRep[x,y];

                if (cellValue == 0){
                    localStreetCount +=1;
                }
                else{
                    localTotalHeight+=cellValue;
                }

                //Check each of the four adjacent cells (if they arent outside our grid)
                //If they are the same, increase fitness
                if ((x-1)>=0){
                    if (townRep[x-1,y]==cellValue){
                        localFitness+=1;
                    }
                }
                if ((x+1)<townRep.GetLength(0)){
                    if (townRep[x+1,y]==cellValue){
                        localFitness+=1;
                    }
                }
                if ((y-1)>=0){
                    if (townRep[x,y-1]==cellValue){
                        localFitness+=1;
                    }
                }
                if ((y+1)<townRep.GetLength(1)){
                    if (townRep[x,y+1]==cellValue){
                        localFitness+=1;
                    }
                }                                                
            }
        }

        this.fitness = localFitness;
        this.streetCount = localStreetCount;
        this.totalHeight = localTotalHeight;

    }

    public mapElitesTown Clone(){
        return new mapElitesTown(townRepresentation);
    }

    public int[,] getRepresentation(){
        return townRepresentation;
    }
    
    public int getFitness(){
        return fitness;
    }

    public int getStreetCount(){
        return streetCount;
    }

    public int getTotalHeight(){
        return totalHeight;
    }


}
