using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinyCube : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer colour;
    private int curColour = 1;
    void Start()
    {
        colour = GetComponentInChildren<Renderer>();
        colour.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetColor()
    {
        return curColour;
    }

    public void ChangeColor()
    {
        if (curColour == 1)
        {
            colour.material.color = Color.green;
            curColour = 2;
        }
        else if(curColour == 2)
        {
            colour.material.color = Color.blue;
            curColour = 3;
        }
        else if(curColour == 3)
        {
            colour.material.color = Color.red;
            curColour = 1;
        }

    }

    public void Shot()
    {
        Destroy(gameObject);
    }
}
