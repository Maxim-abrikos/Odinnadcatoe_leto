using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxFPS = 60;

    private void Start()
    {
        Application.targetFrameRate = maxFPS;
    }

    public void SetFPS(int fps)
    {
        maxFPS = fps;
        Application.targetFrameRate = maxFPS;
    }
}
