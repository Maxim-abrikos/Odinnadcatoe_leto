using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;
using System;
using Unity.VisualScripting;
using UnityEngine.Windows.WebCam;

namespace StarterAssets
{
    internal class Note_T_1_3 : MonoBehaviour
    {

        [SerializeField]
        public Image NoteImage;

        [SerializeField]
        public Image Correct_Answer_Image;
        [SerializeField]
        private GameEvents GE;


        private bool Action = false;
        private bool Costyl = false;
        private int Index = 0;
        [SerializeField]
        private TMP_Text MessagePanel;
        //[SerializeField]
        private string[] PartsOfAnswers;
        [SerializeField]
        private string[] FullAnswer;
        [SerializeField]
        private bool Is_Active;
        [SerializeField]
        private TMP_InputField WrittenAnswer;
        //[SerializeField]
        //private Button Enter;
        [SerializeField]
        private FirstPersonController _FirstPersonController;
        [SerializeField]
        private UnityEngine.UI.Button CheckButton;

        private string[] PlayersAnswer;

        [SerializeField]
        private Level_completed Portal;

        [SerializeField]
        private GameObject[] Stars;


        public bool T_or_G;

        [SerializeField]
        private int Number_of_Discipline;
        [SerializeField]
        private string NameOfLoad;

        private float TimeLeft = 20.0f;
        private bool CanSolve = true;

        [SerializeField]
        private Image WrongAnswer;


        void Start()
        {
            MessagePanel.enabled = false;
            this.gameObject.SetActive(Is_Active);
            WrittenAnswer.gameObject.SetActive(false);
            CheckButton.onClick.AddListener(() => OnClick());
            CheckButton.gameObject.SetActive(false);
            //PartsOfAnswers = FullAnswer.Split(' ');
            Correct_Answer_Image.enabled = false;
            if (PlayerPrefs.HasKey(NameOfLoad))
            {
                Index = PlayerPrefs.GetInt(NameOfLoad);
                if (Index == 100)
                {
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Destroy(Stars[i]);
                        }
                        Portal.Stars += 3;
                    }
                }
            }
        }


        internal void OnClick()
        {
            float similarityRatios = 0;
            float comparing = -1.0f;
            PlayersAnswer = WrittenAnswer.text.Split(' ');
            for (int j = 0; j < FullAnswer.Length; j++)
            {
                PartsOfAnswers = FullAnswer[j].Split(' ');
                similarityRatios = 0;
                for (int i = 0; i < PartsOfAnswers.Length; i++)
                {
                    if (i < PlayersAnswer.Length)
                    {
                        similarityRatios += CalculateSimilarity(PartsOfAnswers[i], PlayersAnswer[i]);
                    }
                    else
                    {
                        similarityRatios = 0;
                    }
                }
                if (similarityRatios >= comparing)
                    comparing = similarityRatios;
            }
            if (comparing / PartsOfAnswers.Length >= 0.7f)
            {
                Index = 100;
                Correct_Answer_Image.enabled = true;
                GE.Collect_Stars(3, T_or_G, Number_of_Discipline);
                Portal.Stars += 3;
                CheckButton.enabled = false;
                CheckButton.gameObject.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    Destroy(Stars[i]);
                }
            }
            else
            {
                WrongAnswer.enabled = true;
                TimeLeft = 20.0f;
                StartCoroutine(Countdown());
            }
            }
        


        static float CalculateSimilarity(string str1, string str2)
        {
            int minLength = Math.Min(str1.Length, str2.Length);
            int matches = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (str1[i] == str2[i])
                {
                    matches++;
                }
            }
            return (float)matches / Math.Max(str1.Length, str2.Length);
        }


        public void Update()
        {
            if (Index < 1 && CanSolve)
            {
                StartCoroutine(Show_Task());
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
                    Correct_Answer_Image.enabled = false;
                    WrittenAnswer.gameObject.SetActive(false);
                    CheckButton.gameObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    this._FirstPersonController.ChangeRS(false);
                }
            }
            if (CanSolve == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WrongAnswer.enabled = false;
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
                    WrittenAnswer.enabled = true;
                    Cursor.visible = true;
                    Costyl = false;
                    Time.timeScale = 0;
                    WrittenAnswer.gameObject.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    this._FirstPersonController.ChangeRS(true);
                    CheckButton.gameObject.SetActive(true);
                    if (WrittenAnswer.gameObject.activeSelf)
                    {
                        WrittenAnswer.Select();
                        WrittenAnswer.ActivateInputField();
                    }
                }
                else if (Action && Costyl == false)
                {
                    MessagePanel.enabled = true;
                    Action = true;
                    NoteImage.enabled = false;
                    WrittenAnswer.enabled = false;
                    Costyl = !Costyl;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    WrittenAnswer.gameObject.SetActive(false);
                    this._FirstPersonController.ChangeRS(false);
                    CheckButton.gameObject.SetActive(false);
                    if (WrittenAnswer.gameObject.activeSelf)
                    {
                        WrittenAnswer.Select();
                        WrittenAnswer.ActivateInputField();
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
            yield break;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Index != 100)
                    MessagePanel.text = "Нажмите Е для чтения";
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
                WrittenAnswer.enabled = false;
                Costyl = false;
                Cursor.visible = false;

                WrittenAnswer.gameObject.SetActive(false);
                CheckButton.gameObject.SetActive(false);
                //MessagePanel.text = "";
                if (WrittenAnswer.gameObject.activeSelf)
                {
                    // Set focus on the input field
                    WrittenAnswer.Select();
                    WrittenAnswer.ActivateInputField();
                }
                WrongAnswer.enabled = false;
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
}
