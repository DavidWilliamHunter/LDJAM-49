using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LDJam49
{
    public class MainMenuSystem : MonoBehaviour
    {
        public void RunInstructions()
        {
            SceneManager.LoadScene("Instructions");
        }

        public void RunLevel1()
        {
            SceneManager.LoadScene("Level1");
        }

        public void Quit()
        {
            Application.Quit();

        }
    }
}