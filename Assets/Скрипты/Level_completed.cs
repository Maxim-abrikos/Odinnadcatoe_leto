using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Level_completed : MonoBehaviour
{
    [SerializeField]
    internal int Stars = 0;
    [SerializeField]
    private bool Is_Active;
    [SerializeField]
    private List<GameObject> NextLevel = new List<GameObject>();

    [SerializeField]
    private string Name_to_save;

    void Start()
    {
        this.gameObject.SetActive(Is_Active);
        if (PlayerPrefs.HasKey(Name_to_save))
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Stars >= 3)
        {
            //foreach (GameObject go in NextLevel) 
            //{
            //    go.SetActive(true);
            //}
            Destroy(this.gameObject);
            PlayerPrefs.SetInt(Name_to_save, 1);
            PlayerPrefs.Save();
        }
    }
}
