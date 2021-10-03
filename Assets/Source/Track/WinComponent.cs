using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class WinComponent : TrackComponent
    {
        public override void CharacterCollisionEntered(CharacterController characterController, Collider collider)
        {
            characterController.Win();
        }
    }
}