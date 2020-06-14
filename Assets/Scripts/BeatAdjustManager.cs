using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class BeatAdjustManager : MonoBehaviour
{
    public Text RespondText;
    public Image Edge;

    public static int AdjustAmount = 0;
    BPM_Counter Bpm;


    int Beat7Count = 0;
    int Beat4Count = 0;
    int BeatNormal = 0;

    private void OnEnable()
    {
        BPM_Counter.OnBeatFull += EdgeShine;
    }
    private void OnDisable()
    {
        BPM_Counter.OnBeatFull -= EdgeShine;
    }
    private void Start()
    {
        Bpm = BPM_Counter.instance;
        Timer(1f, BPM_Counter.instance.StartCount);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RecordInput();
        }
    }
    public void RecordInput()
    {
        if (Bpm.CheckBeatD8(7))
        {
            Beat7Count++;
            RespondText.text = null;
            RespondText.text = "too fast";
            TextBounce(Color.yellow);
        }
        else if (Bpm.CheckBeatD8(4))
        {
            Beat4Count++;
            RespondText.text = null;
            RespondText.text = "too slow";
            TextBounce(Color.yellow);
        }
        else if (Bpm.CheckBeatD8(new int[] { 1, 2, 3, 8 }))
        {
            BeatNormal++;
            RespondText.text = null;
            RespondText.text = "perfect";
            TextBounce(Color.green);
        }
        else
        {
            RespondText.text = "miss";
            TextBounce(Color.red);
        }

        if (Beat7Count + Beat4Count + BeatNormal > 24)
        {
            AdjustEnd();
        }
    }
    public void TextBounce(Color color)
    {
        RespondText.DOColor(Color.white, Bpm._beatIntervalSec).From(color, true);
    }
    public void AdjustEnd()
    {
        if (Beat7Count + Beat4Count >= BeatNormal)
        {
            AdjustAmount = Beat7Count > Beat4Count ? -1 : 1;
        }
        else
        {
            AdjustAmount = 0;
        }

        SceneCutter.Instance.LoadScene("Main", 1f);
    }
    public void EdgeShine()
    {
        Edge.DOFade(0, Bpm._beatIntervalSec).From(0.8f, true);
    }


    public void Timer(float time, Action EndOperation)
    {
        StartCoroutine(TimerCoroutine(time, EndOperation));
    }
    public IEnumerator TimerCoroutine(float time, Action EndOperation)
    {
        yield return new WaitForSeconds(time);
        EndOperation?.Invoke();
    }
}
