using Dance.Utils.Curves;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class MoveController : MonoBehaviour
    {
        private BezierCurve bezierCurve;
        private float endTime;
        private float startTime;
        private bool moving = false;
        public Vector3 targetLocation;

        private Rigidbody rb;

        public float jumpForce = 1.0f;
        public float moveSpeed = 10.0f;

        public Vector3 currentLocation;
        public Quaternion direction;

        // Start is called before the first frame update
        void Start()
        {
            currentLocation = transform.position;
        }

        public void Update()
        {
            if (moving)
            {
                float proportion = Time.time - startTime;
                float range = endTime - startTime;
                proportion /= range;
                if (proportion < 1.0f && proportion > 0.0f)
                {
                    Vector3 position = bezierCurve.GetPoint(proportion);
                    Vector3 velocity = bezierCurve.GetVelocity(proportion);
                    position.y += 1.5f;
                    transform.position = position;
                    //transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
                }
                else if (proportion > 1.0f)
                {
                    Debug.Log("End Move");
                    moving = false;
                    currentLocation = targetLocation;
                }
            }
        }

        public void DoMove(Vector3 target, float timeToDestination)
        {
            Vector3[] curveControls = new Vector3[4];
            curveControls[0] = transform.position;
            curveControls[1] = transform.position + Vector3.up * jumpForce;
            curveControls[2] = target + Vector3.up * jumpForce;
            curveControls[3] = target;
            bezierCurve = new BezierCurve(curveControls);

            endTime = Time.time + timeToDestination;
            startTime = Time.time;

            targetLocation = target;
            moving = true;
        }
    }
}