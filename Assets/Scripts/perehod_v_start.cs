using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class perehod_v_start : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.position = new Vector3(1.92055f, 0.2519306f, 38.03358f);
        SceneManager.LoadScene("Startovaya");
        transform.position = new Vector3(1.92055f, 0.2519306f, 38.03358f);
    }
}