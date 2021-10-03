using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class CharacterController : MonoBehaviour
    {
        //private Rigidbody rb;

        public float jumpForce = 5.0f;
        public float sideSpeed = 10.0f;
        public float maxForwardSpeed = 10.0f;



        public float maxSideSpeed = 5.0f;
        public float sideStepAmount = 1.0f;
        public float maxAcc = 5.0f;
        public float maxBreak = 5.0f;
        public float fallSpeed = 50.0f;
        public float fallDepth = -10.0f;

        public float forwardSpeed = 0.0f;

        public float sideStepTarget = 0.0f;
        public float currentLane = 0.0f;
        public bool isStepping = false;

        public bool isGrounded = false;
        public bool isJumping = false;
        public bool isFatalFall = false;
        public Vector3 velocity = Vector3.zero;

        public bool IsPaused = false;

        public LevelController levelController;

        private CharacterMaterial characterMaterial;

        public bool isAlive = true;

        // Start is called before the first frame update
        void Start()
        {
            if (!characterMaterial)
                characterMaterial = GetComponent<CharacterMaterial>();
            //if (!rb)
            //   rb = GetComponent<Rigidbody>();

            //currentLane = Mathf.Floor(transform.position.z / sideStepAmount) * sideStepAmount;
            //Vector3 pos = transform.position;
            //pos.z = currentLane;
            //transform.position = pos;

            levelController = LevelController.Instance;

            levelController.OnLevelLoad.AddListener(OnLevelLoaded);
            levelController.OnResetLevel.AddListener(OnLevelReset);

            TimeController timeController = TimeController.Instance;
            //timeController.OnColourChangeEvent.AddListener(OnColourChangeEvent);
        }

        private Vector3 startPosition;
        private BlockColour startBlockColour;

        public void OnLevelLoaded()
        {
            startPosition = transform.position;

            currentLane = Mathf.Floor(transform.position.z / sideStepAmount) * sideStepAmount;

            startBlockColour = characterMaterial.GetBlockColour();
        }

        public void OnLevelReset()
        {
            Debug.Log("CharacterController: LevelReset");
            transform.position = startPosition;

            currentLane = Mathf.Floor(transform.position.z / sideStepAmount) * sideStepAmount;
            Vector3 pos = transform.position;
            pos.z = currentLane;
            transform.position = pos;

            isAlive = true;

            characterMaterial.SetBlockColour(startBlockColour);
        }

        public void OnColourChangeEvent(BlockColour targetColour)
        {
            if (targetColour != characterMaterial.blockColour)
            {
                characterMaterial.SetBlockColour(targetColour);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (levelController.currentState == LevelController.State.Running)
            {
                float h = Input.GetAxis("Horizontal");
                float v = 1.0f; Input.GetAxis("Vertical");
                bool jump = Input.GetButtonDown("Jump");

                bool leftKeyPress = h < 0;
                bool rightKeyPress = h > 0; // Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.B);

                if (transform.position.y < fallDepth)
                {
                    Kill();
                }

                if (!isFatalFall)
                {
                    if (v > 0.0f)
                    {
                        AccelerateForward(maxAcc, maxForwardSpeed);
                    }
                    else
                    {
                        BreakForward(maxBreak, 0.0f);
                    }

                    //if (h < 0.0f && !isStepping)
                    if (leftKeyPress && !isStepping)
                        SideStep(true);
                    else if (rightKeyPress && !isStepping)
                        SideStep(false);
                }

                DoSideStep();
                CheckGround();

                if (jump && isGrounded && !isJumping && !isFatalFall)
                {
                    velocity.y = jumpForce;
                    isJumping = true;
                }
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (isFatalFall)
                {
                    if (isGrounded)  // our fall is broken lucky us
                        isFatalFall = false;
                    else
                    {
                        velocity.y = -fallSpeed;        // fatal falling as we are not on a surface
                        //BreakForward(maxBreak, 0.0f);
                    }

                }
                else
                {
                    if (velocity.y > 0.0f) // moving upward
                    {
                        Debug.Log(velocity.y);
                        velocity.y -= fallSpeed * Time.deltaTime;    // accelerate downward
                    }
                    else
                    {
                        if (isGrounded)   // we hit the ground in the down direction
                        {
                            velocity.y = 0.0f;
                            isJumping = false;
                        }
                        else
                        {
                            if (!isStepping && !isJumping)
                            {
                                isFatalFall = true;
                            }
                            else if (isJumping)
                            {
                                Debug.Log(velocity.y);
                                velocity.y -= fallSpeed * Time.deltaTime;    // accelerate downward
                            }
                        }
                    }
                }

                transform.position += velocity * Time.deltaTime;
            }
        }

        public void AccelerateForward(float amount, float targetSpeed)
        {
            //float vel = rb.velocity.magnitude;
            float vel = velocity.magnitude;
            //float currentForwardSpeed = rb.velocity.x;
            float currentForwardSpeed = velocity.x;
            float newVel = currentForwardSpeed + amount;
            if (newVel > targetSpeed)
                newVel = targetSpeed;
            //Vector3 newForwardVelocity = rb.velocity;
            Vector3 newForwardVelocity = velocity;
            newForwardVelocity.x = newVel;
            //rb.velocity = newForwardVelocity;
            velocity = newForwardVelocity;
            forwardSpeed = newVel;
        }

        public void BreakForward(float amount, float targetSpeed)
        {
            //float vel = rb.velocity.magnitude;
            //float currentForwardSpeed = rb.velocity.x;
            float vel = velocity.magnitude;
            float currentForwardSpeed = velocity.x;
            float newVel = currentForwardSpeed - amount;
            if (newVel < targetSpeed)
                newVel = targetSpeed;
            //Vector3 newForwardVelocity = rb.velocity;
            Vector3 newForwardVelocity = velocity;
            newForwardVelocity.x = newVel;
            //rb.velocity = newForwardVelocity;
            velocity = newForwardVelocity;
            forwardSpeed = newVel;
        }

        public void SideStep(bool direction)
        {
            if (!isStepping)
            {
                currentLane = Mathf.Round(transform.position.z / sideStepAmount) * sideStepAmount;
                sideStepTarget = currentLane + (direction ? sideStepAmount : -sideStepAmount);
                Debug.Log("SideStep: currentLane : " + currentLane + " sideStepTarget : " + sideStepTarget);

                isStepping = true;
            }
        }

        public void DoSideStep()
        {
            //float actualLane = Mathf.Floor(transform.position.z / sideStepAmount) * sideStepAmount;
            if (isStepping)
            {
                if (sideStepTarget < currentLane)        // moving left
                {
                    if (transform.position.z > sideStepTarget)
                    {
                        //Vector3 vel = rb.velocity;
                        //vel.z = -sideSpeed;
                        //rb.velocity = vel;
                        velocity.z = -sideSpeed;
                    }
                    else
                    {
                        //Vector3 vel = rb.velocity;
                        //vel.z = 0;
                        //rb.velocity = vel;
                        velocity.z = 0;
                        isStepping = false;
                    }
                }
                else
                {
                    if (transform.position.z < sideStepTarget)
                    {
                        //Vector3 vel = rb.velocity;
                        //vel.z = sideSpeed;
                        //rb.velocity = vel;
                        velocity.z = sideSpeed;
                    }
                    else
                    {
                        //Vector3 vel = rb.velocity;
                        //vel.z = 0;
                        //rb.velocity = vel;
                        velocity.z = 0;
                        isStepping = false;
                    }
                }
            }


        }

        protected HashSet<TrackPiece> currentlyCollidingWith = new HashSet<TrackPiece>();

        public void CheckGround()
        {
            Collider[] results = Physics.OverlapSphere(transform.position, 1.0f);
            bool localGrounded = false;
            isGrounded = false;
            HashSet<TrackPiece> foundThisIteration = new HashSet<TrackPiece>();
            foreach (Collider result in results)
            {
                TrackPiece tc = result.GetComponent<TrackPiece>();
                if (tc)
                {
                    if (tc.IsGround())
                        isGrounded = true;

                    foundThisIteration.Add(tc);

                    if (currentlyCollidingWith.Contains(tc))
                        tc.CharacterCollisionStay(this, result);
                    else
                    {
                        currentlyCollidingWith.Add(tc);
                        tc.CharacterCollisionEntered(this, result);
                    }
                }
            }

            List<TrackPiece> removeList = new List<TrackPiece>();
            foreach (var tc in currentlyCollidingWith)
                if (!foundThisIteration.Contains(tc))
                {
                    tc.CharacterCollisionExit(this);
                    removeList.Add(tc);
                }

            foreach (var tc in removeList)
                currentlyCollidingWith.Remove(tc);

        }

        public void Kill()
        {
            characterMaterial.SetBlockColour(BlockColour.Black);
            isAlive = false;
            levelController.Killed();
        }

        public void Win()
        {
            StartCoroutine(WinCoroutine());
        }

        public IEnumerator WinCoroutine()
        {
            IsPaused = true; // use the pause functionality to disable input.
            Vector3 currentAngle = transform.rotation.eulerAngles;
            Vector3 startPosition = transform.position;

            float skidAmount = 15.0f;
            float spinAmount = 180.0f;

            Vector3 currentPosition = startPosition;
            for (; transform.position.x < startPosition.x + skidAmount;)
            {
                currentAngle.y += spinAmount * Time.fixedDeltaTime;
                currentPosition.x += maxForwardSpeed * Time.fixedDeltaTime;
                transform.position = currentPosition;
                transform.rotation = Quaternion.Euler(currentAngle);
                yield return new WaitForFixedUpdate();
            }
            IsPaused = false;
            levelController.Win();
        }
    }
}