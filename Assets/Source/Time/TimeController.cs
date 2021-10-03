using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LDJam49
{
    public class TimeController : MonoBehaviour
    {
        private static TimeController _instance;

        public static TimeController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<TimeController>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        public double currentTime;
        public double bpm;
        public double levelStartTime = 0.0;

        private double secondsPerBeat;

        public long currentBeat = 0;
        public double locationInBeat = 0.0f;

        public CharacterController characterController;
        public double characterSpeed;
        public double cellsPerSecond;
        public double cellsPerBeats;

        public UnityEvent<long> OnBeat;

        public UnityEvent<BlockColour> OnColourChangeEvent;

        public UnityEvent OnStart;
        public UnityEvent OnReset;

        public bool IsPaused = true;
        public double PauseStartTime;
        public double PauseTime;
        public double CumulativePauseTimeExcludingThisPauseEvent;
        public double CumulativeRunTime;

        private void Start()
        {
            secondsPerBeat = 60.0 / bpm;

            characterSpeed = characterController.maxForwardSpeed;
            cellsPerSecond = characterSpeed;

        }

        private void FixedUpdate()
        {
            double newTime = Time.fixedTimeAsDouble - levelStartTime - PauseTime - CumulativePauseTimeExcludingThisPauseEvent;
            if (!IsPaused)
            {
                long newBeat = (int)System.Math.Floor(currentTime / secondsPerBeat);
                double newlocationInBeat = newTime - (newBeat * secondsPerBeat);

                if (newBeat > currentBeat) // we've started a new Beat
                {
                    OnBeat.Invoke(newBeat);
                }

                currentTime = newTime;
                currentBeat = newBeat;
                locationInBeat = newlocationInBeat;

            }
            else
            {

                PauseTime = newTime - PauseStartTime;

            }
        }

        public void ResetTimer()
        {
            currentTime = 0;
            currentBeat = 0;
            locationInBeat = 0;
            CumulativeRunTime = 0;
            CumulativePauseTimeExcludingThisPauseEvent = 0;
            PauseTime = 0;
            PauseStartTime = Time.fixedTimeAsDouble;
            IsPaused = true;

            levelStartTime = Time.fixedTimeAsDouble;

            if(OnReset!=null)
                OnReset.Invoke();
        }

        public void StartTimer()
        {
            CumulativeRunTime = 0;
            CumulativePauseTimeExcludingThisPauseEvent = 0;
            PauseTime = 0;
            PauseStartTime = Time.fixedTimeAsDouble;
            levelStartTime = Time.fixedTimeAsDouble;
            IsPaused = false;
            if(OnStart!=null)
                OnStart.Invoke();
        }

        public void PauseTimer()
        {
            if(!IsPaused)
            {
                CumulativePauseTimeExcludingThisPauseEvent += PauseTime;
                PauseTime = 0;
                PauseStartTime = Time.fixedTimeAsDouble;
                IsPaused = true;
            }
        }

        public void UnPauseTimer()
        {
            if(IsPaused)
            {

                IsPaused = false;
            }
        }

        public void DoColourChangeEvent(BlockColour colour)
        {
            if(OnColourChangeEvent!=null)
                OnColourChangeEvent.Invoke(colour);
        }
    }
}