using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuFuncs : MonoBehaviour
{
    private void Start()
    {
    }
    private void Update()
    {
    }

    public void OnStart(Button btn)
    {
        btn.interactable = false;
        SceneCutter.Instance.LoadScene("BeatAdjust", 2f);
        //load next scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
