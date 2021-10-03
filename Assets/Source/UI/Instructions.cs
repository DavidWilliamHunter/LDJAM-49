using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class Instructions : MonoBehaviour
    {
        public CharacterController playerPawn;
        public Renderer render;

        public float range = 50.0f;

        private void Start()
        {
            if(!playerPawn)
            {
                playerPawn = Transform.FindObjectOfType<CharacterController>();
            }

            if (!render)
                render = GetComponent<Renderer>();

            render.enabled = false;

        }

        private void Update()
        {
            if (playerPawn.transform.position.x > transform.position.x)
            {
                render.enabled = false;
                gameObject.SetActive(false);
                return;
            }
            if(playerPawn.transform.position.x > (transform.position.x -range ))
            {
                render.enabled = true;
            }

        }
    }
}