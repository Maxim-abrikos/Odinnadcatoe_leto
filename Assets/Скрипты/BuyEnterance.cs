using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyEnterance : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Level_completed Portal;
    [SerializeField]
    private TMP_Text MessagePanel;
    [SerializeField]
    private GameEvents gameEvents;
    private bool Action = false;
    private bool Costyl = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Action == true && Costyl == true && Input.GetKeyDown(KeyCode.E))
        {
            if (gameEvents.Use_Coins())
            {
                MessagePanel.text = "Монетки использованы";
                Portal.Stars += 2;
            }
            else
                MessagePanel.text = "Не хватает монеток";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MessagePanel.SetActive(true);
            MessagePanel.enabled = (true);
            MessagePanel.text = "Купить 2 звезды за 3 монетки?";
            Action = true;
            Costyl = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MessagePanel.SetActive(false);
            MessagePanel.enabled = (false);
            Action = false;
            //NoteImage.enabled = false;
            Costyl = false;
        }
    }
}
