using UnityEngine;
using UCE;

public class PCPipeAndPlugHintTrigger : HintTrigger {
    [Header("Snap Drop")]
    public Transform snapDropObject;
    public Transform snapDropPoint;

    [Header("Air Transmit Connection")]
    public AirTransmit fromAirTransmit;
    public AirTransmit toAirTransmit;

    private bool isConnected = false;

    public void GraspBegin() {
        OpenTrigger();

        if (isConnected && fromAirTransmit != null && toAirTransmit != null) {
            AirTransmit.Disconnect(fromAirTransmit, toAirTransmit);
            isConnected = false;
        }

        TipBoard.Progress(1, 0);
        TipBoard.Progress(4, 0);
    }

    public void GraspEnd() {
        if (hintObject.activeInHierarchy) {
            snapDropObject.position = snapDropPoint.position;
            snapDropObject.rotation = snapDropPoint.rotation;

            if (fromAirTransmit != null && toAirTransmit != null) {
                AirTransmit.Connect(fromAirTransmit, toAirTransmit);
                isConnected = true;
            }

            TipBoard.Progress(0, 0);
            TipBoard.Progress(1, 4);
        }

        CloseTrigger();
    }

}

