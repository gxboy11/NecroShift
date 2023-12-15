using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryController : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        AudioManager.Instance.PlaySFX("Theme");
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
