using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public bool isSelected = false;
    
    Material material;
    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = gameObject.GetComponent<Renderer>();
        material = ren.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            material.color = Color.green;
        }
        else
        {
            material.color = Color.red;
        }
    }
}