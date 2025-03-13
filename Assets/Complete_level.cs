using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complete_level : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SELF = new GameObject();
    bool Is_Real = true;
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
