using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class TrackPiece : MonoBehaviour
    {
        public virtual void CharacterCollisionEntered(CharacterController characterController, Collider collider) { }
        public virtual void CharacterCollisionStay(CharacterController characterController, Collider collider) { }
        public virtual void CharacterCollisionExit(CharacterController characterController) { }

        public virtual bool IsGround() { return false; }
    }
}