using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Wall : MonoBehaviour
{

    public GameObject SELF = new GameObject();
    bool Is_Real = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Is_Real == false)
        {
            Destroy(SELF);
            Is_Real = false;
        }
    }
}
