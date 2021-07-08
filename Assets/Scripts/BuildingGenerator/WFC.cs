using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFC : MonoBehaviour
{
    public class Vocabulary {
        public string Code { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<string>> Allowed = new Dictionary<string, List<string>>();

        List<string> dimensions = new List<string>{"x", "y", "z"};
        List<string> directions = new List<string>{"p", "n"};

        public Vocabulary (string code, string name){
            Code = code;
            Name = name;
            foreach(var dim in dimensions) {
                foreach(var dir in directions) {
                    Allowed.Add( dim+dir, new List<string>() );
                }
            }
        }
    }

    // TODO: move this outside of this class and improve extendibility
    private Vocabulary G;
    private Vocabulary N;
    private Vocabulary X;
    private Vocabulary T;
    private Vocabulary A1;
    private Vocabulary A2;
    private Vocabulary B1;
    private Vocabulary B2;
    private Vocabulary B3;
    private Vocabulary B4;
    private Vocabulary C1;
    private Vocabulary C2;
    private Vocabulary C3;
    private Dictionary<string, Vocabulary> dictionary;
    private Dictionary<(int,int,int), List<string>> site;
    private List<(int,int,int)> coordCollapseable;
    private List<(int,int,int)> coordVisitable;
    private int maxH;
    private int planX;
    private int planY;
    private int planXPadded;
    private int planYPadded;
    private int planZPadded;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void ResetSite(int h, int x, int y) {
        site = new Dictionary<(int,int,int), List<string>>();
        coordCollapseable = new List<(int,int,int)>();
        coordVisitable = new List<(int,int,int)>();
        maxH = h;
        planX = x;
        planY = y;
        planXPadded = planX+2;
        planYPadded = planY+2;
        planZPadded = maxH+2;
        
        for (int i = 0; i < planXPadded; i++) {
            for (int j = 0; j < planYPadded; j++) {
                for (int k = 0; k < planZPadded; k++) {
                    if ( (i==0) || (i==planXPadded-1) || (j==0) || (j==planYPadded-1) ) {
                        site.Add( (i,j,k), new List<string>() );
                    }/*
                    else if () {

                    }
                    else if () {

                    }
                    else {

                    }*/
                }
            }
        }

        // PropagateFrom( (1,1,0) );
    }

    // Initialize the dictionary and the adjacency rules among structural pieces. TODO: improve extendibility by providing a interface for adding rules
    void Initialize()
    {
        G  = new Vocabulary ("G",  "Ground");
        N  = new Vocabulary ("N",  "Empty");
        X  = new Vocabulary ("X",  "Padding-side-forbidden");
        T  = new Vocabulary ("T",  "Padding-top-forbidden");
        A1 = new Vocabulary ("A1", "Ground-column");
        A2 = new Vocabulary ("A2", "Ground-1room");
        B1 = new Vocabulary ("B1", "Mid-column");
        B2 = new Vocabulary ("B2", "Mid-1room");
        B3 = new Vocabulary ("B3", "Mid-2roomx-n");
        B4 = new Vocabulary ("B4", "Mid-2roomx-p");
        C1 = new Vocabulary ("C1", "Roof-1room");
        C2 = new Vocabulary ("C2", "Roof-2roomx-n");
        C3 = new Vocabulary ("C3", "Roof-2roomx-p");

        dictionary = new Dictionary<string, Vocabulary>();
        dictionary.Add( "G",  G );
        dictionary.Add( "N",  N );
        dictionary.Add( "X",  X );
        dictionary.Add( "T",  T );
        dictionary.Add( "A1", A1 );
        dictionary.Add( "A2", A2 );
        dictionary.Add( "B1", B1 );
        dictionary.Add( "B2", B2 );
        dictionary.Add( "B3", B3 );
        dictionary.Add( "B4", B4 );
        dictionary.Add( "C1", C1 );
        dictionary.Add( "C2", C2 );
        dictionary.Add( "C3", C3 );

        G.Allowed["zp"] = new List<string>() {"A1", "A2", "N"};
        G.Allowed["xp"] = G.Allowed["xn"] = G.Allowed["yp"] = G.Allowed["yn"] = new List<string>() {"G"};

        N.Allowed["zp"] = new List<string>() {"N"};
        N.Allowed["zn"] = new List<string>() {"C1", "C2", "C3", "G", "N"};
        N.Allowed["xp"] = N.Allowed["xn"] = N.Allowed["yp"] = N.Allowed["yn"] = new List<string>() {"A1","A2","B1","B2","B3","B4","C1","C2","C3","N","X"};

        X.Allowed["zp"] = X.Allowed["zn"] = new List<string>() {"X"};
        X.Allowed["xp"] = X.Allowed["xn"] = X.Allowed["yp"] = X.Allowed["yn"] = new List<string>() {"A1","A2","B1","B2","B3","B4","C1","C2","C3","N","X"};

        T.Allowed["zn"] = new List<string>() {"C1", "C2", "C3", "N"};
        T.Allowed["xp"] = T.Allowed["xn"] = T.Allowed["yp"] = T.Allowed["yn"] = new List<string>() {"T"};

        A1.Allowed["zp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3"};
        A1.Allowed["zn"] = new List<string>() {"G"};
        A1.Allowed["xp"] = A1.Allowed["xn"] = A1.Allowed["yp"] = A1.Allowed["yn"] = new List<string>() {"A1", "A2", "N", "X"};

        A2.Allowed["zp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3"};
        A2.Allowed["zn"] = new List<string>() {"G"};
        A2.Allowed["xp"] = A2.Allowed["xn"] = A2.Allowed["yp"] = A2.Allowed["yn"] = new List<string>() {"A1", "A2", "N", "X"};

        B1.Allowed["zn"] = new List<string>() {"A1", "B1"};
        B1.Allowed["zp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3"};
        B1.Allowed["xp"] = B1.Allowed["xn"] = B1.Allowed["yp"] = B1.Allowed["yn"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        B2.Allowed["zn"] = new List<string>() {"A1", "A2", "B1", "B2", "B3", "B4"};
        B2.Allowed["zp"] = new List<string>() {"B2", "B3", "B4", "C1", "C2", "C3"};
        B2.Allowed["xn"] = new List<string>() {"B1", "B2", "B4", "C1", "C2", "C3", "N", "X"};
        B2.Allowed["xp"] = new List<string>() {"B1", "B2", "B3", "C1", "C2", "C3", "N", "X"};
        B2.Allowed["yn"] = B2.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        B3.Allowed["zn"] = new List<string>() {"A1", "A2", "B1", "B2", "B3", "B4"};
        B3.Allowed["zp"] = new List<string>() {"B2", "B3", "B4", "C1", "C2", "C3"};
        B3.Allowed["xn"] = new List<string>() {"B1", "B2", "B4", "C1", "C2", "C3", "N", "X"};
        B3.Allowed["xp"] = new List<string>() {"B4"};
        B3.Allowed["yn"] = B3.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        B4.Allowed["zn"] = new List<string>() {"A1", "A2", "B1", "B2", "B3", "B4"};
        B4.Allowed["zp"] = new List<string>() {"B2", "B3", "B4", "C1", "C2", "C3"};
        B4.Allowed["xn"] = new List<string>() {"B3"};
        B4.Allowed["xp"] = new List<string>() {"B1", "B2", "B3", "C1", "C2", "C3", "N", "X"};
        B4.Allowed["yn"] = B4.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        C1.Allowed["zn"] = new List<string>() {"A1", "A2", "B1", "B2"};
        C1.Allowed["zp"] = new List<string>() {"N"};
        C1.Allowed["xn"] = new List<string>() {"B1", "B2", "B4", "C1", "C2", "C3", "N", "X"};
        C1.Allowed["xp"] = new List<string>() {"B1", "B2", "B3", "C1", "C2", "C3", "N", "X"};
        C1.Allowed["yn"] = C1.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        C2.Allowed["zn"] = new List<string>() {"B3"};
        C2.Allowed["zp"] = new List<string>() {"N"};
        C2.Allowed["xn"] = new List<string>() {"B1", "B2", "B4", "C1", "C2", "C3", "N", "X"};
        C2.Allowed["xp"] = new List<string>() {"B1", "B2", "B3", "C1", "C2", "C3", "N", "X"};
        C2.Allowed["yn"] = C2.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};

        C3.Allowed["zn"] = new List<string>() {"B4"};
        C3.Allowed["zp"] = new List<string>() {"N"};
        C3.Allowed["xn"] = new List<string>() {"B1", "B2", "B4", "C1", "C2", "C3", "N", "X"};
        C3.Allowed["xp"] = new List<string>() {"B1", "B2", "B3", "C1", "C2", "C3", "N", "X"};
        C3.Allowed["yn"] = C3.Allowed["yp"] = new List<string>() {"B1", "B2", "B3", "B4", "C1", "C2", "C3", "N", "X"};
    }
}
