using UnityEngine.UI;
using UnityEngine;
using System;

public class ShowHide : MonoBehaviour
{
    
    [SerializeField]
    private Image[] HtP;
    int I_now = 0, I_prev = 0;

    [SerializeField]
    private Button Close;


    public void ShowHtP()
    {
        HtP[(int)Math.Abs(I_prev % HtP.Length)].enabled = false;
        HtP[(int)Math.Abs(I_now % HtP.Length)].enabled = true;
        Close.enabled = true;
        //Close.image.enabled = true;
        //HtP[i_prev % HtP.Length].enabled = false;
    }

    public void CloseHtP()
    {
        HtP[I_now % HtP.Length].enabled = false;
        Close.enabled = false;
    }

    public void MoveForward()
    {
        if (I_now != HtP.Length)
        {
            I_prev = I_now;
            I_now++;
            ShowHtP();
        }
    }

    public void MoveBack()
    {
        //if (I_now >= 1)
        //{
            I_prev = I_now;
            I_now--;
            ShowHtP();
        //}
    }
}
