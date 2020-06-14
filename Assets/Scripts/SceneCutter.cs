using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCutter : MonoBehaviour
{
    private static SceneCutter _instance;
    public static SceneCutter Instance
    {
        get
        {
            if (_instance == null)
            {
                var instance = new GameObject("SceneCutter").AddComponent<SceneCutter>();
                _instance = instance;
                DontDestroyOnLoad(instance.gameObject);
            }
            return _instance;
        }
    }

    public Image BlackPanel;

    private bool IsLoading;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(BlackPanel.transform.parent.gameObject);
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        DontDestroyOnLoad(BlackPanel.transform.parent);
        SceneManager.activeSceneChanged += FadeIn;
        SceneManager.activeSceneChanged += (current, next) => IsLoading = false;
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {

    }


    public void FadeIn(Scene current, Scene next)
    {
        StartCoroutine(DoFadeIn());
    }
    IEnumerator DoFadeIn()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            BlackPanel.color = new Color(BlackPanel.color.r, BlackPanel.color.g, BlackPanel.color.b, alpha);
            alpha -= Time.deltaTime;
            yield return null;
        }
    }
    public void FadeOut(float sec)
    {
        StartCoroutine(DoFadeOut(sec));
    }
    IEnumerator DoFadeOut(float sec)
    {
        float alpha = 0;
        while (alpha < sec)
        {
            BlackPanel.color = new Color(BlackPanel.color.r, BlackPanel.color.g, BlackPanel.color.b, alpha);
            alpha += Time.deltaTime;
            yield return null;
        }
    }

    public void LoadScene(string sceneName, float fadeSec)
    {
        StartCoroutine(DoLoadScene(sceneName, fadeSec));
    }

    IEnumerator DoLoadScene(string sceneName, float fadeSec)
    {
        if (IsLoading) yield break;
        IsLoading = true;
        StartCoroutine(DoFadeOut(fadeSec));
        yield return new WaitForSeconds(fadeSec);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
