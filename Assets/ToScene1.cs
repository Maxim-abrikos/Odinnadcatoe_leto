using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToScene1 : MonoBehaviour
{
    private bool collided = false;


    public List<Image> LoadScreens;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collided = true;
            StartCoroutine(LoadAndDelay());
        }
    }

    IEnumerator LoadAndDelay()
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Startovaya");
        asyncLoad.allowSceneActivation = false;

        Image loadingImage = LoadScreens[Random.Range(0, LoadScreens.Count)];
        loadingImage.enabled = true;
        float timer = 0f;
        Color imageColor = loadingImage.color;

        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;

            if (timer >= 5f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            imageColor.a += 0.35f * Time.deltaTime; //Mathf.Lerp(0f, 1f, timer / 5f);
            loadingImage.color = imageColor;

            yield return null;
        }
    }

    private void ScreenLoad()
    {
        Image I = LoadScreens[Random.Range(0, LoadScreens.Count)];
        Color C = I.color;
        I.enabled = true;
        float fade_speed = 1f;
        while (C.a < 1.0f)
        {
            C.a += fade_speed * Time.deltaTime;
            I.color = C;
        }
    }
}
