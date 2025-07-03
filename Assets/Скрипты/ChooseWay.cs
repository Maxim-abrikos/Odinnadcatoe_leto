
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace StarterAssets
{
    public class ChooseWay : MonoBehaviour
    {
        [SerializeField]
        private Image NoteImage;
        [SerializeField]
        private TMP_Text MessagePanel;

        [SerializeField]
        private Button[] Buttons;
        //public TextMeshProUGUI MessagePanel;
        private bool Action = false;
        private bool Costyl = false;
        internal string RoomName = "GlavnZal";
        [SerializeField]
        private FirstPersonController _FPS;
        [SerializeField]
        private perehod BigDoor;
        [SerializeField]
        private GameEvents GE;

        void Start()
        {
            //MessagePanel.SetActive(false);
            MessagePanel.enabled = false;
            Buttons[0].onClick.AddListener(() => ChangeWay("GlavnZal", 0));
            Buttons[1].onClick.AddListener(() => ChangeWay("GlavnZal1", 1));
            Buttons[2].onClick.AddListener(() => ChangeWay("GlavnZal2", 2));
            Buttons[3].onClick.AddListener(() => ChangeWay("GlavnZal3", 3));
        }

        private void ChangeWay(string way, int level)
        {
            RoomName = way;
            BigDoor.Location = way;
            GE.Current_level = level;
            _FPS.LEVEL = level;
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
                    for (int i = 0; i < Buttons.Length; i++)
                    {
                        Buttons[i].gameObject.SetActive(true); ;
                    }
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    this._FPS.ChangeRS(true);
                }
                else if (Action && Costyl == false)
                {
                    //MessagePanel.SetActive(true);
                    MessagePanel.enabled = (true);
                    Action = true;
                    NoteImage.enabled = false;
                    Costyl = !Costyl;
                    //Time.timeScale = 1;
                    for (int i = 0; i < Buttons.Length; i++)
                    {
                        Buttons[i].gameObject.SetActive(false);
                    }
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    this._FPS.ChangeRS(false);
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //MessagePanel.SetActive(true);
                MessagePanel.enabled = (true);
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
                NoteImage.enabled = false;
                Costyl = false;
                for (int i = 0; i < Buttons.Length; i++)
                {
                    Buttons[i].gameObject.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                this._FPS.ChangeRS(false);
            }
        }
    }

}