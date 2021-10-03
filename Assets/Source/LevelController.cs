using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LDJam49
{
    public class LevelController : MonoBehaviour
    {
        private static LevelController _instance;

        public static LevelController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<LevelController>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        public UnityEvent OnLevelLoad;

        public void LevelLoaded()
        {
            if (OnLevelLoad != null)
                OnLevelLoad.Invoke();
            currentState = State.LevelLoaded;
        }

        public UnityEvent OnResetLevel;

        public void ResetLevel()
        {
            TimeController timeController = TimeController.Instance;
            timeController.ResetTimer();
            currentState = State.LevelReady;
            if (OnResetLevel != null)
                OnResetLevel.Invoke();
        }

        public UnityEvent OnStartLevel;


        public void StartLevel()
        {
            TimeController timeController = TimeController.Instance;
            timeController.StartTimer();
            currentState = State.Running;
            if (OnStartLevel != null)
                OnStartLevel.Invoke();
        }

        public UnityEvent OnPauseLevel;

        public void PauseLevel()
        {
            TimeController timeController = TimeController.Instance;
            timeController.PauseTimer();
            currentState = State.Paused;
            if(OnPauseLevel!=null)
                OnPauseLevel.Invoke();
        }

        public UnityEvent OnUnPauseLevel;

        public void UnPauseLevel()
        {
            TimeController timeController = TimeController.Instance;
            timeController.UnPauseTimer();
            currentState = State.Running;
            if (OnUnPauseLevel != null)
                OnUnPauseLevel.Invoke();
        }

        public UnityEvent OnFinishedAny;
        public UnityEvent OnFailed;

        public void Killed()
        {
            currentState = State.Failed;
            if (OnFailed != null)
                OnFailed.Invoke();
            if (OnFinishedAny != null)
                OnFinishedAny.Invoke();
        }

        public UnityEvent OnSuccess;

        public void Win()
        {
            currentState = State.Success;
            if (OnSuccess != null)
                OnSuccess.Invoke();
            if (OnFinishedAny != null)
                OnFinishedAny.Invoke();
        }

        public enum State
        {
            PreLoading,
            LevelLoaded,
            LevelReady,
            Running,
            Paused,
            Failed,
            Success
        }

        public State currentState = State.PreLoading;

        public void Update()
        {
            switch (currentState)
            {
                case State.PreLoading:
                    LevelLoaded();
                    break;
                case State.LevelLoaded:
                    ResetLevel();
                    break;
                case State.LevelReady:
                    if(Input.GetButtonUp("Start"))
                    {
                        StartLevel();
                    }
                    if (Input.GetButtonUp("Cancel"))
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    break;
                case State.Running:
                    if (Input.GetButtonUp("Pause") || Input.GetButtonUp("Cancel"))
                        PauseLevel();
                    break;
                case State.Paused:
                    if (Input.GetButtonUp("Pause"))
                        UnPauseLevel();
                    if (Input.GetButtonUp("Cancel"))
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    break;
                case State.Failed:
                    if (Input.GetButtonUp("Start"))
                    {
                        ResetLevel();
                    }
                    if (Input.GetButtonUp("Cancel"))
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    break;
                case State.Success:
                    if (Input.GetButtonUp("Start"))
                    {
                        ResetLevel();
                    }
                    if(Input.GetButtonUp("Cancel"))
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    break;
            }
            
        }
    }
}