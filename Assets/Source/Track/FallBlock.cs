using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDJam49
{
    public class FallBlock : TrackComponent
    {
 

        protected void Start()
        {
            blockColour = BlockColour.Red;
            TimeController timeController = TimeController.Instance;

            timeController.OnReset.AddListener(OnReset);
        }

        private new void OnReset()
        {
            base.OnReset();

            TimeController timeController = TimeController.Instance;

            double levelPosition = (float) transform.position.x;

            FallTime = levelPosition / timeController.cellsPerSecond;

            // setup the colour flipping system
            timeController.OnColourChangeEvent.AddListener(Flip); 
        }

        private void FixedUpdate()
        {
            if(TimeController.Instance.currentTime > FallTime)
            {
                Fall();
            }
        }


    }
}