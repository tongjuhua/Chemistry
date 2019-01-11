using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCE;

public class PipeAndPlugFinishTrigger : FinishTrigger {
    [Header("Air Transmit Connection")]
    public AirTransmit fromAirTransmit;
    public AirTransmit toAirTransmit;

    //[Header("Temporarily Tracking Disable")]
    //public OptitrackObject trackingObject;

    public override void OnFinished(Collider other) {
        base.OnFinished(other);

        if (fromAirTransmit != null && toAirTransmit != null) {
            AirTransmit.Connect(fromAirTransmit, toAirTransmit);
        }

        TipBoard.Progress(0, 0);
        if (TipBoard.Progress(1, 4)) {
          //  trackingObject.isTracking = false;
        }
    }

    public override void OnRestarted(Collider other) {
        base.OnRestarted(other);


        if (fromAirTransmit != null && toAirTransmit != null) {
            AirTransmit.Disconnect(fromAirTransmit, toAirTransmit);
        }
        
        TipBoard.Progress(1, 0);
        TipBoard.Progress(4, 0);
    }
}
