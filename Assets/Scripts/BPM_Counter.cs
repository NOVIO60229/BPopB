using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BPM_Counter : MonoBehaviour
{
    public float _bpm;
    public bool detect_BPM_everyFrame = false;
    [HideInInspector] public bool _isStartCount = false;

    private float _beatTimer = 0, _beatTimerD8 = 0;
    private bool isfirstBeat = true;

    //variables for outside
    public static BPM_Counter instance;
    public float _beatIntervalSec, _beatIntervalD8Sec;
    public float AnimPlaySpeed;
    public bool _isBeatFull = true, _isBeatD8 = true;
    public int _beatFullCount = 1, _beatD8Count = 1;

    //events
    public static Action OnBeatFull;
    public static Action OnBeatD8;
    //excute onve every 8 beatFull loop
    public static Action OnBeatFullevery8;

    AudioSource _audio;
    int _beatFull = 1;
    int _beatD8 = 1;
    void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        instance = this;
        _audio = GetComponent<AudioSource>();
        DetectBPM();
    }
    private void OnEnable()
    {
        LevelManager.OnLevelStart += StartCount;
        LevelManager.OnLevelEnd += FadeMusic;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelStart -= StartCount;
        LevelManager.OnLevelEnd -= FadeMusic;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStartCount)
        {
            UpdateBeat();
        }
    }
    void UpdateBeat()
    {
        //manully trigger firstBeat since the timer can't count first beat
        if (isfirstBeat)
        {
            OnBeatFull?.Invoke();
            OnBeatD8?.Invoke();
            OnBeatFullevery8?.Invoke();
            isfirstBeat = false;
        }

        if (detect_BPM_everyFrame)
        {
            DetectBPM();
        }

        //full beat count
        _isBeatFull = false;
        _beatTimer += Time.deltaTime;
        if (_beatTimer >= _beatIntervalSec)
        {
            _beatTimer -= _beatIntervalSec;
            _isBeatFull = true;
            _beatFullCount++;

            UpdateBeatFull();

            OnBeatFull?.Invoke();
            if (CheckBeatFull(1))
            {
                OnBeatFullevery8?.Invoke();
            }
        }

        //devided beat count 
        _isBeatD8 = false;
        _beatTimerD8 += Time.deltaTime;
        if (_beatTimerD8 >= _beatIntervalD8Sec)
        {
            _beatTimerD8 -= _beatIntervalD8Sec;
            _isBeatD8 = true;
            _beatD8Count++;

            UpdateBeatD8();

            OnBeatD8?.Invoke();
        }
    }
    void DetectBPM()
    {
        _beatIntervalSec = 60 / _bpm;
        _beatIntervalD8Sec = _beatIntervalSec / 8;
        AnimPlaySpeed = _bpm / 60f;
    }
    public void StartCount()
    {
        _isStartCount = true;
        StartCoroutine(PlayMusic());
    }
    IEnumerator PlayMusic()
    {
        if (_audio.clip == null || _audio.isPlaying)
        {
            yield break;
        }

        _audio.Play();

        float volume = 0;
        while (_audio.volume < 0.2f)
        {
            volume += 0.0004f;
            _audio.volume = volume;
            yield return null;
        }
    }
    public void FadeMusic()
    {
        StartCoroutine(DoFadeMusic());
    }
    IEnumerator DoFadeMusic()
    {
        while (_audio.volume > 0)
        {
            _audio.volume -= 0.0002f;
            yield return null;
        }

        _audio.Stop();
        SceneCutter.Instance.LoadScene("Menu", 2f);
        InitializeVariables();
    }


    public bool CheckBeatFull(int beat)
    {
        return (_isBeatFull) && (_beatFull == beat);

    }
    public bool CheckBeatFull(int[] beats)
    {
        for (int i = 0; i < beats.Length; i++)
        {
            if ((_isBeatFull) && (_beatFull == beats[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckBeatD8(int beat)
    {
        return (_isBeatD8) && (_beatD8 == beat);
    }
    //remove isBeatD8 for player
    public bool CheckBeatD8(int[] beats)
    {
        for (int i = 0; i < beats.Length; i++)
        {
            if ((_beatD8 == beats[i]))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckBeatD8(int beatFull, int beatD8)
    {
        return (_isBeatD8) && (_beatFull == beatFull && _beatD8 == beatD8);
    }
    public bool CheckBeatD8(int beatFull, int[] beatD8s)
    {
        for (int i = 0; i < beatD8s.Length; i++)
        {
            if ((_isBeatD8) && (_beatFull == beatFull && _beatD8 == beatD8s[i]))
            {
                return true;
            }
        }
        return false;
    }


    private void UpdateBeatFull()
    {
        _beatFull = _beatFullCount % 8;
        if (_beatFull == 0)
        {
            _beatFull = 8;
        }
    }
    private void UpdateBeatD8()
    {
        _beatD8 = _beatD8Count % 8;
        if (_beatD8 == 0)
        {
            _beatD8 = 8;
        }
    }

    public void InitializeVariables()
    {
        OnBeatFull = null;
        OnBeatD8 = null;
        OnBeatFullevery8 = null;
        _isStartCount = false;
        _beatTimer = 0;
        _beatTimerD8 = 0;
        isfirstBeat = true;
        _isBeatFull = true;
        _isBeatD8 = true;
        _beatFullCount = 1;
        _beatD8Count = 1;
        _beatFull = 1;
        _beatD8 = 1;
    }
}
