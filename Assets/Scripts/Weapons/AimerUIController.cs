using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimerUIController : MonoBehaviour
{

    public void AimScope()
    {
        gameObject.SetActive(true);
    }

    public void HipAim()
    {
        gameObject.SetActive(false);
    }
}
