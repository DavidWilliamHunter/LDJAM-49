using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class CharacterMaterial : MonoBehaviour
    {
        public BlockColour blockColour;

        public Material[] blockColourMaterials;

        public TrailRenderer trail;

        public Renderer rendererComponent;

        public void Start()
        {
            if (!trail)
                trail = GetComponent<TrailRenderer>();
            if (!rendererComponent)
                rendererComponent = GetComponent<Renderer>();
        }

        public void SetBlockColour (BlockColour blockColour)
        {
            this.blockColour = blockColour;

            int index = (int)blockColour;

            rendererComponent.material = blockColourMaterials[index];
        }

        public BlockColour GetBlockColour() { return blockColour;  }
    }
}