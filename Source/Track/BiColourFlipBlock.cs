using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class BiColourFlipBlock : TrackComponent
    {
        BlockColour originalColour;

        // Start is called before the first frame update
        void Start()
        {
            //SetBlockColour(BlockColour.Red);
            originalColour = blockColour;
            SetBlockColour(blockColour);


            TimeController timeController = TimeController.Instance;
            LevelController levelController = LevelController.Instance;
            levelController.OnLevelLoad.AddListener(OnLevelLoad);
            levelController.OnResetLevel.AddListener(OnReset);
            levelController.OnResetLevel.AddListener(ThisOnReset);

            timeController.OnColourChangeEvent.AddListener(OnColourChangeEvent);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ThisOnReset()
        {
            //if (originalColour != blockColour)
            //{
                Flip(originalColour);
            SetBlockColour(originalColour);

            SetupBlockColour(originalColour);
            //}
        }

        void OnColourChangeEvent(BlockColour targetColour)
        {
            //if(targetColour!=blockColour)
            {
                BlockColour newColour = BlockColour.Red;
                switch (blockColour)
                {
                    case BlockColour.Red:
                        newColour = BlockColour.Blue;
                        break;
                    case BlockColour.Blue:
                        newColour = BlockColour.Red;
                        break;
                    case BlockColour.Green:
                        break;
                }
                Flip(newColour);
                SetBlockColour(newColour);
            }

        }

        public override void SetupBlockColour(BlockColour blockColour)
        {
            switch (blockColour)
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
        }

        public void OnCollisionEnter(Collision collision)
        {
            var characterMaterial = collision.collider.GetComponent<CharacterMaterial>();
            if (characterMaterial)
            {
                if (blockColour != characterMaterial.blockColour)
                {
                    Fall();
                }
            }
        }

        public override void CharacterCollisionEntered(CharacterController characterController, Collider collider)
        {
            var characterMaterial = characterController.GetComponent<CharacterMaterial>();
            if (characterMaterial)
            {
                if (blockColour != characterMaterial.blockColour)
                {
                    Fall();
                }
            }
        }

    }
}