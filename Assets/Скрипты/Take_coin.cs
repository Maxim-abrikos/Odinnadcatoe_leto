using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Take_coin : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private TMP_Text MessagePanel;

    private bool CanPickUp = false;
    [SerializeField]
    GameEvents GE;

    [SerializeField]
    private int Nomer;


    void Start()
    {
        MessagePanel.enabled = false;
        if (PlayerPrefs.HasKey(Nomer.ToString()))
        {
            Destroy(this.gameObject);
            MessagePanel.enabled = false;
            this.gameObject.SetActive(false);
            MessagePanel.enabled = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (CanPickUp) 
        {
            StartCoroutine(Pick());
        }

    }

    IEnumerator Pick()
    {
        if ((Input.GetKeyDown(KeyCode.E)))
        {
            GE.Collect_Coins(Nomer);
            Destroy(this.gameObject);
            MessagePanel.enabled = false;
            this.gameObject.SetActive(false);
            MessagePanel.enabled = false;
            PlayerPrefs.SetString(Nomer.ToString(), Nomer.ToString());
            PlayerPrefs.Save();
        }
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanPickUp = true;
            MessagePanel.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanPickUp = false;
            MessagePanel.enabled = false;
        }
    }
}
