using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    public bool isSelected
    {
        set
        {
            ToggleSelected(value);
        }
        get
        {
            return m_isSelected;
        }

    }
    private bool m_isSelected = false;
    
    Material material;
    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = gameObject.GetComponent<Renderer>();
        material = ren.material;
    }

    void ToggleSelected(bool curValue)
    {
        m_isSelected = curValue;
        if (curValue)
        {
            material.color = Color.cyan;
        }
        else
        {
            material.color = Color.white;
        }
    }
}