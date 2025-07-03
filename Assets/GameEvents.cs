 using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace StarterAssets
{
    internal class GameEvents : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private GameObject[] PauseMenu;
        private bool Pause = true;

        [SerializeField]
        private FirstPersonController _FirstPersonController;

        internal bool Costyl;

        [SerializeField]
        private AudioClip[] Sounds;

        private AudioSource _Source => GetComponent<AudioSource>();

        internal int Current_level = 0;


        void Awake()
        {
            Application.targetFrameRate = 120;        // ��� -1, ���� ������ ��������� ����� �����������
            QualitySettings.vSyncCount = 0;           // ��������� VSync
            Application.runInBackground = true;
        }

        //public int[] _Stars= new int[2] { 0,0};
        void Start()
        {
            Cursor.visible = false;
            Statistics.enabled = false;
            if (PlayerPrefs.HasKey("Level"))
            {
                Current_level = PlayerPrefs.GetInt("Level");
            }
        }

        private void Update()
        {
            Get_Paused();
        }


        internal void PlaySound(int i)
        {
            _Source.PlayOneShot(Sounds[i]);
        }

        public void StrangePause()
        {
            for (int i = 0; i < PauseMenu.Length; i++)
            {
                PauseMenu[i].SetActive(false);
            }
            Time.timeScale = 1.0f;
            this._FirstPersonController.ChangeRS(Pause);
            //_FirstPersonController.RotationSpeed = 1.0f;
            Pause = !Pause;
            AudioListener.pause = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Statistics.enabled = false;
        }

        private void Get_Paused()
        {
            if ((Input.GetKeyDown(KeyCode.Escape)))
            {
                if (Pause)
                {
                    for (int i = 0; i < PauseMenu.Length; i++)
                    {
                        PauseMenu[i].SetActive(true);
                    }
                    Time.timeScale = 0.0f;
                    this._FirstPersonController.ChangeRS(Pause);
                    //_FirstPersonController.GetComponents = 0.0f;
                    Pause = !Pause;
                    AudioListener.pause = true;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    for (int i = 0; i < PauseMenu.Length; i++)
                    {
                        PauseMenu[i].SetActive(false);
                    }
                    Time.timeScale = 1.0f;
                    this._FirstPersonController.ChangeRS(Pause);
                    //_FirstPersonController.RotationSpeed = 1.0f;
                    Pause = !Pause;
                    AudioListener.pause = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Statistics.enabled = false;
                }
            }
        }

        internal void Collect_Stars(int Stars, bool T_of_G, int index)
        {
            if (T_of_G)
            {
                _FirstPersonController.Collected_Stars_T[Current_level] += Stars;
            }
            else
            {
                _FirstPersonController.Collected_Stars_G[Current_level] += Stars;
            }
            _FirstPersonController.Collected_Stars_In_Disciplines[Current_level, index] += Stars;
        }
        internal void Collect_Coins(int Nomer)
        {
            _FirstPersonController.Collected_Coins[Current_level]++;
        }

        internal bool Use_Coins()
        {
            if (_FirstPersonController.Collected_Coins[Current_level] >= 3)
            {
                _FirstPersonController.Collected_Coins[Current_level] -= 3;
                return true;
            }
            else
                return false;
        }


        public void ExitGame()
        {
            Application.Quit();
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("Menushka");
        }

        [SerializeField]
        private TMP_Text Statistics;
        public void ShowStats()
        {
            Statistics.enabled = true;
            if (Current_level == 0)
            {
                Statistics.text = "���� �������:" + "\n����� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 0] + "\n����� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 1] + "\n���. ������ " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 2] + "\n������ " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 3] + "\n���������������� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 4] + "\n������� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 5] + "\n���������� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 6] + "\n��������� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 7] + "\n��������� ������������ " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 8] + "\n���� " + _FirstPersonController.Collected_Stars_In_Disciplines[0, 9];
            }
            if (Current_level == 1)
            {
                Statistics.text = "���� �������:" + "\n�������� ������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 0] + "\n������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 1] + "\n���������� ���������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 2] + "\n������ " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 3] + "\n���������������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 4] + "\n������� ������ " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 5] + "\n������� ���� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 6] + "\n��������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 7] + "\n����� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 8] + "\n���������� " + _FirstPersonController.Collected_Stars_In_Disciplines[1, 9];
            }
            if (Current_level == 2)
            {
                Statistics.text = "���� �������:" + "\n���. ������ " + (_FirstPersonController.Collected_Stars_In_Disciplines[2, 0] + _FirstPersonController.Collected_Stars_In_Disciplines[2, 1]) + "\n������� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 2] + "\n������ " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 3] + "\n���������������� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 4] + "\n������� ������ " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 5] + "\n������� ���� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 6] + "\n��������� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 7] + "\n����� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 8] + "\n���������� " + _FirstPersonController.Collected_Stars_In_Disciplines[2, 9];
            }
            if (Current_level == 3)
            {
                Statistics.text = "���� �������:" + "\n������: " + (_FirstPersonController.Collected_Stars_In_Disciplines[3, 0] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 1] +_FirstPersonController.Collected_Stars_In_Disciplines[3, 2] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 3] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 4]) + "\n�����+�������� : " + (_FirstPersonController.Collected_Stars_In_Disciplines[3, 5] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 6] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 7] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 8] + _FirstPersonController.Collected_Stars_In_Disciplines[3, 9]);
            }
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetInt("Level", Current_level);
            PlayerPrefs.Save();
        }

        public static void ResetLevel()
        {
            PlayerPrefs.DeleteKey("Level");
        }
        }
}