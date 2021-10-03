using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class ColourChange : TrackPiece
    {
        public BlockColour blockColour;
        public BlockColour originalColour;

        public Material[] blockColourMaterials;

        public Renderer rendererComponent;


        // Start is called before the first frame update
        public void Start()
        {
            if (!rendererComponent)
                rendererComponent = GetComponent<Renderer>();

            LevelController levelController = LevelController.Instance;
            levelController.OnResetLevel.AddListener(OnReset);
            levelController.OnLevelLoad.AddListener(OnLoadLevel);
            TimeController timeController = TimeController.Instance;
            timeController.OnColourChangeEvent.AddListener(OnColourChangeEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLoadLevel()
        {
            originalColour = blockColour;
        }

        public void OnReset()
        {
            blockColour = originalColour;
            SetBlockColour(blockColour);
        }

        public void OnColourChangeEvent(BlockColour blockColour)
        {
            BlockColour newBlockColour = BlockColour.Red;
            switch (this.blockColour)
            {
                case BlockColour.Red:
                    newBlockColour = BlockColour.Blue;
                    break;
                case BlockColour.Blue:
                    newBlockColour = BlockColour.Red;
                    break;
                case BlockColour.Green:
                    break;
            }

            SetBlockColour(newBlockColour);
        }

        public void SetBlockColour(BlockColour blockColour)
        {
            this.blockColour = blockColour;

            int index = (int)blockColour;

            rendererComponent.material = blockColourMaterials[index];
        }

        public BlockColour GetBlockColour() { return blockColour; }

        public override void CharacterCollisionEntered(CharacterController characterController, Collider collider)
        {
            Debug.Log("CharacterCollisionEntered");
            BlockColour newBlockColour = BlockColour.Red;
            switch (blockColour)
            {
                case BlockColour.Red:
                    newBlockColour = BlockColour.Blue;
                    break;
                case BlockColour.Blue:
                    newBlockColour = BlockColour.Red;
                    break;
                case BlockColour.Green:
                    return;
                    break;
            }
            Debug.Log("blockColour: " + blockColour);
            CharacterMaterial characterMaterial = characterController.GetComponent<CharacterMaterial>();

            characterMaterial.SetBlockColour(blockColour);
        }

    }
}