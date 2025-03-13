using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using StarterAssets;

internal class MenuButtonsClick : MonoBehaviour
{
    [SerializeField]
    private List<Image> LoadScreens;

    [SerializeField]
    private Image[] HtP;

    [SerializeField]
    private Image Titles;
    int I_now = 0, I_prev = 0;

    [SerializeField]
    private GameObject Close;
    [SerializeField]
    private GameObject Right;
    [SerializeField]
    private GameObject Left;

    [SerializeField]
    private GameObject CloseT;
    [SerializeField]
    private GameEvents GE;

    public void PlayGame()
    {
        StartCoroutine(LoadAndDelay());
        //SceneManager.LoadScene("Startovaya");
    }

    public void Start()
    {
        for (int i = 0; i < HtP.Length; i++)
        {
            HtP[i].enabled = false;
        }
        Close.SetActive(false);
        Right.SetActive(false);
        Left.SetActive(false);
        CloseT.SetActive(false);
    }


    //IEnumerator LoadScreen()
    //{
    //    Image image = LoadScreens[Random.Range(0, LoadScreens.Count)];
    //    Color color = image.color;
    //    color.a = 1f;
    //    image.color = color;
    //    image.enabled = true;
    //    float timer = 0.0f;
    //    while (timer < 3.0f)
    //        timer += Time.deltaTime;
    //    {
    //        color.a -= 0.35f * Time.deltaTime; //Mathf.Lerp(0f, 1f, timer / 5f);
    //        image.color = color;
    //    }
    //    image.enabled = false;
    //    yield return null;
    //}

    IEnumerator LoadAndDelay()
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Startovaya");
        asyncLoad.allowSceneActivation = false;

        Image loadingImage = LoadScreens[UnityEngine.Random.Range(0, LoadScreens.Count)];
        //loadingImage.enabled = true;
        float timer = 1f;
        Color imageColor = loadingImage.color;
        loadingImage.color = imageColor;
        loadingImage.enabled = true;
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

    public void Continue()
    {

    }

    public void ShowHtP()
    {
        HtP[(int)Math.Abs(I_prev % HtP.Length)].enabled = false;
        HtP[(int)Math.Abs(I_now % HtP.Length)].enabled = true;
        Close.SetActive(true);
        Right.SetActive(true);
        Left.SetActive(true);
        //Close.image.enabled = true;
        //HtP[i_prev % HtP.Length].enabled = false;
    }

    public void CloseHtP()
    {
        HtP[(int)Math.Abs(I_now % HtP.Length)].enabled = false;
        Close.SetActive(false);
        Right.SetActive(false);
        Left.SetActive(false);
    }

    public void MoveForward()
    {
        I_prev = I_now;
        I_now++;
        ShowHtP();
    }

    public void MoveBack()
    {
        //if (I_now > 0)
        //{
            I_prev = I_now;
            I_now--;

            ShowHtP();
        //}
    }


    public void ExitGame()
    {
        GameEvents.ResetLevel();
        Application.Quit();
    }

    public void ShowTitles()
    {
        Titles.enabled = true;
        CloseT.SetActive(true);
    }

    public void HideTitles()
    {
        Titles.enabled = false;
        CloseT.SetActive(false);
    }

    public void Delete() 
    {
        PlayerPrefs.DeleteAll();
    }

}
