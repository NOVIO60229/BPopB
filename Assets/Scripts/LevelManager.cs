using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static Action OnLevelStart;
    public static Action OnLevelEnd;
    public static Action OnLevelLeave;

    BPM_Counter Bpm;
    public Image Edge;

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
        Timer(1f, OnLevelStart);
    }
    private void Update()
    {
        CheckEvents();
    }

    private void CheckEvents()
    {
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
