using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LDJam49
{
    public class DisplayStartText : MonoBehaviour
    {
        TMP_Text textmeshPro;

        public string StartText = "<color=\"grey\">Press <color=\"black\">[SPACE]<color=\"grey\"> or Gamepad <color=\"green\">[A] <color=\"grey\">";
        public string FailText = "Oops!\n<color=\"grey\">Press <color=\"black\">[SPACE]<color=\"grey\"> or Gamepad <color=\"green\">[A] <color=\"grey\">";
        public string SuccessText = "Yay!\n <color=\"grey\">Press <color=\"black\">[SPACE]<color=\"grey\"> or Gamepad <color=\"green\">[A] <color=\"grey\"> or \n Press <color=\"black\">[ESC]<color=\"grey\"> or Gamepad start";
        public string PauseText = "<color=\"grey\">Press <color=\"black\">[P]<color=\"grey\"> or Gamepad <color=\"green\">[start] <color=\"grey\"> to continue.";
        // Start is called before the first frame update
        void Start()
        {
            textmeshPro = GetComponent<TMP_Text>();
            LevelController levelController = LevelController.Instance;
            levelController.OnResetLevel.AddListener(PressSpaceToStart);
            levelController.OnStartLevel.AddListener(RemoveText);
            levelController.OnPauseLevel.AddListener(Pause);
            levelController.OnUnPauseLevel.AddListener(RemoveText);
            levelController.OnFailed.AddListener(Failure);
            levelController.OnSuccess.AddListener(Success);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PressSpaceToStart()
        {
            DisplayText(StartText);
        }

        public void Pause()
        {
            DisplayText(PauseText);
        }

        public void Success()
        {
            DisplayText(SuccessText);
        }

        public void Failure()
        {
            DisplayText(FailText);
        }

        public void DisplayText(string text)
        {
            textmeshPro.text = text;
            gameObject.SetActive(true);
            enabled = true;
        }

        public void RemoveText()
        {
            gameObject.SetActive(false);
            enabled = false;
        }

    }

}