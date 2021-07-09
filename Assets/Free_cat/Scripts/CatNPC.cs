using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNPC : MonoBehaviour
{
    new public Renderer renderer;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = renderer.material;
        material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
