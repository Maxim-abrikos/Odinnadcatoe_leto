using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen: MonoBehaviour
{
    // Start is called before the first frame update

    public List<Image> LoadScreens;

    void Start()
    {
        StartCoroutine(StartScreen_());
    }

    IEnumerator StartScreen_()
    {
        Image image = LoadScreens[Random.Range(0, LoadScreens.Count)];
        Color color = image.color;
        color.a = 1f;
        image.color = color;
        image.enabled = true;
        float timer = 0.0f;
        while (timer < 3.0f)
            timer += Time.deltaTime;
        {
            color.a -= 0.35f * Time.deltaTime; //Mathf.Lerp(0f, 1f, timer / 5f);
            image.color = color;
        }
        image.enabled = false;
        yield return null;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
