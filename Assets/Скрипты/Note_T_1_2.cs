using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

internal class Note_T_1_2 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image NoteImage;
    [SerializeField]
    private Image[] Right;
    [SerializeField]
    private TMP_Text MessagePanel;

    private bool Action = false;
    private bool Costyl = false;

    [SerializeField]
    private Image[] Pointers = new Image[4];
    [SerializeField]
    private string NameOfLoad;


    private KeyCode[] Answers1 = new KeyCode[4] {  KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4  };
    [SerializeField]
    private int[] R_Andwers = new int[4];

    private bool[,] True_False = new bool[4, 4] { {false,false,false,false },
                                                 {false,false,false,false },
                                                 {false,false,false,false },
                                                 {false,false,false,false } };
    private int Index = 0;

    [SerializeField]
    private List<GameObject> To_Destroy= new List<GameObject>(2);
    [SerializeField]
    private Level_completed Portal;
    [SerializeField]
    private bool Is_Active;
    [SerializeField]
    private GameEvents GE;

    public bool T_or_G;

    [SerializeField]
    private int Number_of_Discipline;

    private float TimeLeft = 20.0f;
    private bool CanSolve = true;

    [SerializeField]
    private Image WrongAnswer;

    void Start()
    {
        if (PlayerPrefs.HasKey(NameOfLoad))
        {
            Index = PlayerPrefs.GetInt(NameOfLoad);
            if (Index == 100)
            {
                foreach (GameObject G in To_Destroy)
                {
                    Destroy(G);
                }
                Portal.Stars += 2;
            }
        }
        MessagePanel.enabled = false;
        for (int i = 0; i < 4; i++)
        {
            True_False[i, R_Andwers[i]] = true;
        }
        this.gameObject.SetActive(Is_Active);
    }

    void Update()
    {
        if (Index < 4 && CanSolve)
        {
            StartCoroutine(Show_Task());
            StartCoroutine(Check_Answer());
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && ((Action == true) && (Costyl == false)))
            {
                if (CanSolve)
                    MessagePanel.text = "Задание уже выполнено";
                else
                    MessagePanel.text = "Осталось ещё " + TimeLeft;
                MessagePanel.enabled = true;
                Action = true;
                NoteImage.enabled = false;
                Costyl = !Costyl;
                for (int i = 0; i < 4 * 2; i += 2)
                {
                    Right[i].enabled = false;
                    Right[i + 1].enabled = false;
                }
                Pointers[Index].enabled = false;
            }

        }
        if (CanSolve == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Pointers[Index].enabled = false;
                //WrongAnswer.enabled = false;
                for (int i = 0; i < Index * 2; i += 2)
                {
                    Right[i].enabled = false;
                    Right[i + 1].enabled = false;

                }
                WrongAnswer.enabled = false;
                Pointers[Index].enabled = false;
                //WrongAnswer.enabled = false;
            }
        }
        return;
    }


    IEnumerator Show_Task()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ((Action == true) && (Costyl == true))
            {
                MessagePanel.enabled = false;
                Action = true;
                NoteImage.enabled = true;
                Costyl = false;
                Pointers[Index].enabled = true;
                for (int i = 0; i < Index * 2; i += 2)
                {
                    Right[i].enabled = true;
                    Right[i + 1].enabled = true;
                }
            }
            else if (Action && Costyl == false)
            {
                MessagePanel.enabled = true;
                Action = true;
                NoteImage.enabled = false;
                Costyl = !Costyl;
                for (int i = 0; i < Index * 2; i += 2)
                {
                    Right[i].enabled = false;
                    Right[i + 1].enabled = false;
                }
                Pointers[Index].enabled = false;
            }
        }
        yield return null;
    }

    IEnumerator Check_Answer()
    {
        if ((Action == true) && (Costyl == false) && (Index < 4))
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < Answers1.Length; i++)
                {
                    if (Input.GetKeyDown(Answers1[i]))
                    {
                        if (True_False[Index, i])
                        {
                            Right[Index*2].enabled= true;
                            Right[Index*2+1].enabled= true;
                            if (Index < 4)
                            {
                                Pointers[Index].enabled = false;
                                Index++;
                                if (Index < 4)
                                    Pointers[Index].enabled = true;
                            }
                            if (Index == 4)
                            {
                                Pointers[Index-1].enabled = false;
                                foreach (GameObject G in To_Destroy)
                                {
                                    Destroy(G);
                                }
                                GE.Collect_Stars(2, T_or_G, Number_of_Discipline);
                                Portal.Stars += 2;
                                MessagePanel.enabled = false;
                                Index = 100;
                            }
                        }
                        else
                        {
                            WrongAnswer.enabled = true;
                            TimeLeft = 20.0f;
                            StartCoroutine(Countdown());
                        }
                    }
                }
            }
        }
        yield return null;
    }


    IEnumerator Countdown()
    {
        CanSolve = false;
        while (TimeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            MessagePanel.text = "Осталось ещё " + TimeLeft;
            TimeLeft--;
            if (Input.GetKeyDown(KeyCode.E))
            { WrongAnswer.enabled = false; }
        }
        MessagePanel.text = "Нажмите Е для чтения";
        CanSolve = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Index != 100){
                if (CanSolve)
                {
                    MessagePanel.text = "Нажмите Е для чтения";
                }
            }
            else
            {
                if (CanSolve == false)
                    MessagePanel.text = "Осталось ещё " + TimeLeft;
                else
                    MessagePanel.text = "Задание уже выполнено";
            }
            MessagePanel.enabled = true;
            Action = true;
            Costyl = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessagePanel.enabled = (false);
            Action = false;
            NoteImage.enabled = false;
            Costyl = false;
            for (int l = 0; l < Right.Length; l++)
            {
                Right[l].enabled = false;
            }
            if (Index < 4)
            Pointers[Index].enabled= false;
            WrongAnswer.enabled = false;
            //MessagePanel.text = "";
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(NameOfLoad, Index);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(NameOfLoad, Index);
        PlayerPrefs.Save();
    }
}
