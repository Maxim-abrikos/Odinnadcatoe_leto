using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using StarterAssets;

internal class Note_T_1_1 : MonoBehaviour
{
    [SerializeField]
    private Image NoteImage;

    [SerializeField]
    private Image[] Correct_Answer_Image;

    [SerializeField]
    private int A1, A2;

    [SerializeField]
    private GameObject Star;

    [SerializeField]
    private TMP_Text MessagePanel;
    [SerializeField]
    private string NameOfLoad;

    private bool Action = false;
    private bool Costyl = false;
    private KeyCode[,] Answers = new KeyCode[2, 5] { { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 }, { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 } };
    private bool[,] Check = new bool[2, 5] { { false, false, false, false, false }, { false, false, false, false, false } };
    private int Index = 0;
    private int Max = 2;

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
    private Image[] WrongAnswer;
    

    //private bool HowToLoad = false;

    void Start()
    {
        if (PlayerPrefs.HasKey(NameOfLoad))
        {
            Index = PlayerPrefs.GetInt(NameOfLoad);
            if (Index == 100)
            {
                { Destroy(Star); Portal.Stars += 1; }
            }
        }
        MessagePanel.enabled = false;
            Check[0, A1] = true;
            Check[1, A2] = true;
            this.gameObject.SetActive(Is_Active);

    }

    void Update()
    {
        if (Index < 2 && CanSolve)
        {
            StartCoroutine(Show_Task());
            StartCoroutine(Check_Input());
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
                for (int i = 0; i < 2; i++)
                {
                    Correct_Answer_Image[i].enabled = false;
                }
            }

        }
        if (CanSolve == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
                WrongAnswer[Index].enabled = false;
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
                for (int i = 0; i < Index; i++)
                {
                    Correct_Answer_Image[i].enabled = true;
                }
                WrongAnswer[Index].enabled = false;
                //GE.Open_Close_Book();
            }
            else if (Action && Costyl == false)
            {
                MessagePanel.enabled = true;
                Action = true;
                NoteImage.enabled = false;
                Costyl = !Costyl;
                for (int i = 0; i < Index; i++)
                {
                    Correct_Answer_Image[i].enabled = false;
                }
                WrongAnswer[Index].enabled = false;
                //GE.Open_Close_Book();
            }
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    yield return null;
        //    MessagePanel.enabled = true;
        //    Action = true;
        //    NoteImage.enabled = false;
        //    Costyl = !Costyl;
        //}
        yield return null;
    }


    IEnumerator Check_Input() //вот тут решение задачи низкого уровня сложности
    {
        if ((Action == true) && (Costyl == false) && (Index < 2))
        if (Input.anyKeyDown) 
        {
            for (int i = 0; i < Answers.GetLength(1); i++) 
            {
                if (Index <2)
                if (Input.GetKeyDown(Answers[Index, i])) 
                {
                        if (Check[Index, i])
                        {
                            Index++;
                            if (Index == 2)
                            { Destroy(Star); Correct_Answer_Image[Index - 1].enabled = true; GE.Collect_Stars(1, T_or_G, Number_of_Discipline); Portal.Stars += 1; MessagePanel.enabled = false; Index = 100; }// MessagePanel.text = "Задание уже выполнено"; }

                            else
                            {
                                Correct_Answer_Image[Index - 1].enabled = true;
                            }
                        }
                        else 
                        {
                                WrongAnswer[Index].enabled = true;
                                TimeLeft = 20.0f;
                                StartCoroutine(Countdown());
                        }
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.E))
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
            { WrongAnswer[Index].enabled = false; }
        }
        MessagePanel.text = "Нажмите Е для чтения";
        CanSolve = true;
    }




    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Index != 100) {
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
            MessagePanel.enabled = false;
            Action = false;
            NoteImage.enabled = false;
            for (int i = 0; i < Index; i++)
            {
                Correct_Answer_Image[i].enabled = false;
            }
            Costyl = false;
            WrongAnswer[Index].enabled = false;
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
