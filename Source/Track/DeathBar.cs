using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LDJam49
{
    public class DeathBar : TrackComponent
    {
        public override void CharacterCollisionEntered(CharacterController characterController, Collider collider)
        {
            characterController.Kill();
        }
    }
}