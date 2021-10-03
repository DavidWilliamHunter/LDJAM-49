using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LDJam49
{
    public enum BlockColour
    {
        Red = 0,
        Blue = 1,
        Green = 2,
        Black = 3,
    };

    public class TrackComponent : TrackPiece
    {
        public BlockColour blockColour;

        public float rotationTime = 0.5f;
        public float floor = -10.0f;
        public float fallSpeed = 1.0f;

        public double FallTime = Mathf.Infinity;

        public bool IsFlipping = false;

        protected float originalHeight = 0.0f;

        //public UnityEvent<BlockColour, BlockColour> OnColourChanged;

        public override bool IsGround() { return true; }

        public void SetBlockColour(BlockColour blockColour)
        {
            //if(OnColourChanged!=null)
            //    OnColourChanged.Invoke(this.blockColour, blockColour);
            this.blockColour = blockColour;            
        }

        public BlockColour GetBlockColour() { return blockColour; }

        public virtual void SetupBlockColour(BlockColour blockColour)
        {

        }

        public void Flip(BlockColour targetColour)
        {
            StartCoroutine(FlipCoroutine(targetColour));
        }

        public IEnumerator FlipCoroutine(BlockColour targetColour)
        {
            //if (!IsFlipping)
            {
                IsFlipping = true;
                Vector3 angles = transform.rotation.eulerAngles;
                angles.x = Mathf.Floor(angles.x);
                float targetAngle = targetColour == BlockColour.Red ? 0.0f : 180.0f;
                float rotationSpeed = 180.0f / rotationTime;
                //Debug.Log("Name: " + gameObject.name + " Colour: " + targetColour + " currentAngle :" + angles.x + " targetAngle : " + targetAngle);
                if (Mathf.Abs(angles.x - targetAngle) > 1)
                {
                    if (angles.x > targetAngle)
                    {
                        for (float r = angles.x; r >= targetAngle; r -= (rotationSpeed * Time.deltaTime))
                        {
                            transform.rotation = Quaternion.Euler(new Vector3(r, angles.y, angles.z));
                            //Debug.Log("Name: " + gameObject.name + " Colour: " + targetColour + " currentAngle :" + r + " targetAngle : " + targetAngle);
                            yield return new WaitForFixedUpdate();
                        }
                    }
                    else
                    {
                        for (float r = angles.x; r <= targetAngle; r += (rotationSpeed * Time.deltaTime))
                        {
                            transform.rotation = Quaternion.Euler(new Vector3(r, angles.y, angles.z));
                            //Debug.Log("Name: " + gameObject.name + " Colour: " + targetColour + " currentAngle :" + r + " targetAngle : " + targetAngle);

                            yield return new WaitForFixedUpdate();
                        }
                    }
                }
                switch (targetColour)
                {
                    case BlockColour.Red:
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case BlockColour.Blue:
                        transform.rotation = Quaternion.Euler(180, 0, 0);
                        break;
                    case BlockColour.Green:
                        break;
                }
                angles = transform.rotation.eulerAngles;
                Debug.Log("Name: " + gameObject.name + " Colour: " + targetColour + " currentAngle :" + angles.x + " targetAngle : " + targetAngle);

                IsFlipping = false;
            }
        }

        public void Fall()
        {
            StartCoroutine(FallCoroutine());
        }

        public IEnumerator FallCoroutine()
        {
            Vector3 startPosition = transform.position;
            float velDown = 0.0f;
            Debug.Log("Begin Fall");
            for (float y = startPosition.y; y > floor; y -= velDown)
            {
                velDown += fallSpeed * Time.fixedDeltaTime;
                transform.position = new Vector3(startPosition.x, y, startPosition.z);
                Debug.Log("Iterate Fall");
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("End Fall");
            enabled = false;
        }

        public void OnLevelLoad()
        {
            originalHeight = transform.position.y;
        }

        protected void OnReset()
        {
            TimeController timeController = TimeController.Instance;

            double levelPosition = (float)transform.position.x;
            Vector3 position = transform.position;
            position.y = originalHeight;
            transform.position = position;

            FallTime = levelPosition / timeController.cellsPerSecond;



            enabled = true;
        }
    }
}