using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using DG.Tweening;




public class CameraMovement : MonoBehaviour
{

    public StreetViewCamera cameraFreeRotations;
    

    public void MoveCamera(bool isActivePlayerWhite)
    {
        Vector3 targetPosition = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            -Camera.main.transform.position.z
        );

        Camera.main.transform.DOMove(targetPosition, 1);
        cameraFreeRotations.UpdateOriginalRotation();

    }
}
