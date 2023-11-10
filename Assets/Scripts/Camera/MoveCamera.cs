using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    Transform cameraPosition;

    private void Update()
    {
        //Pone la camara en el Player
        transform.position = cameraPosition.position;
    }
}
