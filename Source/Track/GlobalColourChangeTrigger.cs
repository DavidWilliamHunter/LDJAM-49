using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class GlobalColourChangeTrigger : TrackPiece
    {
        public BlockColour blockColour;
        protected BlockColour originalBlockColour;

        public Material[] blockColourMaterials;

        public Renderer [] rendererComponents;


        // Start is called before the first frame update
        public void Start()
        {
            if (rendererComponents == null || rendererComponents.Length==0)
                rendererComponents = GetComponentsInChildren<Renderer>();

            LevelController levelController = LevelController.Instance;
            levelController.OnResetLevel.AddListener(OnReset);

            levelController.OnLevelLoad.AddListener(OnLoadLevel);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLoadLevel()
        {
            originalBlockColour = blockColour;
        }

        public void OnReset()
        {
            SetBlockColour(originalBlockColour);
        }

        public void SetBlockColour(BlockColour blockColour)
        {
            this.blockColour = blockColour;

            int index = (int)blockColour;

            foreach(var render in rendererComponents)
                render.material = blockColourMaterials[index];


        }

        public BlockColour GetBlockColour() { return blockColour; }

        public override void CharacterCollisionEntered(CharacterController characterController, Collider collider)
        {
            Debug.Log("GlobalColourChangeTrigger: CharacterCollisionEntered");
            CharacterMaterial characterMaterial = characterController.GetComponent<CharacterMaterial>();
            if (characterMaterial.blockColour != blockColour)
            {
                BlockColour newBlockColour = BlockColour.Red;
                switch (characterMaterial.blockColour)
                {
                    case BlockColour.Red:
                        newBlockColour = BlockColour.Blue;
                        break;
                    case BlockColour.Blue:
                        newBlockColour = BlockColour.Red;
                        break;
                    case BlockColour.Green:
                        return;
                }

                TimeController timeController = TimeController.Instance;
                timeController.DoColourChangeEvent(newBlockColour);

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

                SetBlockColour(newBlockColour);
            }


        }
    }
}