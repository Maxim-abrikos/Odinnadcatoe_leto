using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace StarterAssets
{
    public class FinalText : MonoBehaviour
    {
        [SerializeField]
        private Image NoteImage_def;

        [SerializeField]
        private Image NoteImage_max;

        [SerializeField]
        FirstPersonController _FPC;

        private Image ToShow;

        [SerializeField]
        private TMP_Text MessagePanel;
        //public TextMeshProUGUI MessagePanel;
        private bool Action = false;
        private bool Costyl = false;

        public bool T_or_G;

        [SerializeField]
        private GameObject Door;

        [SerializeField]
        private GameEvents GE;

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
                    if (T_or_G)
                    {
                        if (_FPC.Collected_Stars_T[GE.Current_level] == 30)
                            ToShow = NoteImage_max;
                        else
                            ToShow = NoteImage_def;
                    }
                    else
                    {
                        if (_FPC.Collected_Stars_G[GE.Current_level] == 30)
                            ToShow = NoteImage_max;
                        else
                            ToShow = NoteImage_def;
                    }
                    ToShow.enabled = true;
                    Costyl = false;
                    Door.SetActive(false);
                    //Time.timeScale = 0;
                }
                else if (Action && Costyl == false)
                {
                    //MessagePanel.SetActive(true);
                    MessagePanel.enabled = (true);
                    Action = true;
                    ToShow.enabled = false;
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
                ToShow.enabled = false;
                Costyl = false;
            }
        }
    }
}
