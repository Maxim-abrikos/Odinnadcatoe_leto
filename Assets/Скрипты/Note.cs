using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Note : MonoBehaviour
{
    [SerializeField]
    private Image NoteImage;
    [SerializeField]
    private TMP_Text MessagePanel;
    //public TextMeshProUGUI MessagePanel;
    private bool Action = false;
    private bool Costyl = false;

    void Start()
    {
        //MessagePanel.SetActive(false);
        MessagePanel.enabled = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Costyl = !Costyl;
            if ((Action == true) && (Costyl == true))
            {
                //MessagePanel.SetActive(false);
                MessagePanel.enabled = false;
                Action = true;//false;
                NoteImage.enabled = true;
                Costyl = false;
                //Time.timeScale = 0;
            }
            else if (Action && Costyl == false)
            {
                //MessagePanel.SetActive(true);
                MessagePanel.enabled = (true);
                Action = true;
                NoteImage.enabled = false;
                Costyl = !Costyl;
                //Time.timeScale = 1;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MessagePanel.SetActive(true);
            MessagePanel.enabled = (true);
            Action= true;
            Costyl= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //MessagePanel.SetActive(false);
            MessagePanel.enabled= (false);
            Action = false;
            NoteImage.enabled = false;
            Costyl = false;
        }
    }
}
