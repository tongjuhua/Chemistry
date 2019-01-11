﻿namespace UCE
{
    using UnityEngine;
    using VRTK;

    public class Dropper : VRTK_InteractableObject
    {
        public UCE_Engine.ChemicalType tubeType;

        [HideInInspector]
        public bool inTube = false;
        [HideInInspector]
        public bool inBeaker = false;
        [HideInInspector]
        public GameObject tubeWaterGo;
        [HideInInspector]
        public GameObject beakerWaterGo;

        private bool hasWater = false;
        private string newName;
        private UCE_Engine.ChemicalType type;
        
        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);

            if (inTube)
            {
                hasWater = true;
                newName = tubeWaterGo.GetComponent<ShowNameOnTouch>().showName;
                type = tubeWaterGo.GetComponent<TubeWater>().type;

                GameObject go = transform.Find("water").gameObject;
                go.SetActive(true);
                go.GetComponent<Renderer>().material = tubeWaterGo.GetComponent<Renderer>().material;
            }
            else if (hasWater)
            {
                GameObject waterGo = transform.Find("water").gameObject;
                waterGo.SetActive(false);
                hasWater = false;

                if (inBeaker)
                {
                    // The beakerWaterGo has to be set false and then set true again 
                    // beacuase this is the only way to call UCE_Chemical.OnTriggerEnter() 
                    // again to check if reaction is possible now
                    beakerWaterGo.SetActive(false);

                    UCE_Chemical chemical = beakerWaterGo.GetComponent<UCE_Chemical>();
                    chemical.chemicalAmount.type = type;
                    chemical.chemicalAmount.amount = 1;
                    beakerWaterGo.GetComponent<Renderer>().material = waterGo.GetComponent<Renderer>().material;

                    ShowNameOnTouch snot = beakerWaterGo.GetComponent<ShowNameOnTouch>();
                    snot.ChangeName(newName);

                    beakerWaterGo.SetActive(true);
                }
            }
        }
    }
}
