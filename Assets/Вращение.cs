using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Вращение : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float abobus = 0.1f;
        transform.Rotate(0, abobus, 0);
    }
}
