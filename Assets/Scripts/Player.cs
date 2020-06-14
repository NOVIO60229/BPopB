using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance;
    private BPM_Counter Bpm;
    private PlayerAttack PlayerAttack;

    //Move
    Vector2 InputDir = Vector2.zero;
    private bool IsMoving = false;
    private bool IsRewinding = false;
    public static Stack<MoveCommand> MoveRecord = new Stack<MoveCommand>();
    private int[] CheckZone = new int[] { 1, 2, 3, 8 };
    private bool IsComboZone;
    private int CurrentComboBeat = 0;
    public LayerMask GroundLayer;

    //status
    public int HP { get; set; } = 100;
    private int Combo = 0;

    //Events
    public static Action InputStart;
    public static Action InputEnd;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Bpm = BPM_Counter.instance;
        CheckZoneAdjust(BeatAdjustManager.AdjustAmount);
        PlayerAttack = GetComponent<PlayerAttack>();
    }
    public void CheckZoneAdjust(int adjustAmount)
    {
        for (int i = 0; i < CheckZone.Length; i++)
        {
            CheckZone[i] += adjustAmount;
            CheckZone[i] = CheckZone[i] % 8;
            if (CheckZone[i] == 0)
            {
                CheckZone[i] = 8;
            }
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InputDir = context.ReadValue<Vector2>();

            Excute();
        }
    }
    private void OnEnable()
    {
        BPM_Counter.OnBeatFull += ScalePop;
        InputStart += ClearInput;
        InputEnd += AddEmpty;
    }
    private void OnDisable()
    {
        BPM_Counter.OnBeatFull -= ScalePop;
        InputStart -= ClearInput;
        InputEnd -= AddEmpty;
    }

    private void Update()
    {
        if (Bpm.CheckBeatD8(CheckZone[3]))
        {
            InputStart?.Invoke();
        }
        else if (Bpm.CheckBeatD8(CheckZone[2] + 1))
        {
            InputEnd?.Invoke();
        }
    }
    private void Excute()
    {
        TryMove();
        Attack();
    }

    public void TryMove()
    {
        if (Bpm.CheckBeatD8(CheckZone))
        {
            int beat;
            if (Bpm.CheckBeatD8(new int[] { 7, 8 }))
            {
                beat = Bpm._beatFullCount + 1;
            }
            else
            {
                beat = Bpm._beatFullCount;
            }

            if (CurrentComboBeat != beat)
            {
                AddCombo();
            }
            else
            {
                LoseCombo();
            }

            CurrentComboBeat = beat;
        }
        else
        {
            LoseCombo();
        }
        if (!IsMoving && !IsRewinding)
        {
            MoveCommand m = new MoveCommand(InputDir);
            m.Excute();
            MoveRecord.Push(m);
        }
    }
    public void Move(Vector2 dir, float moveSec)
    {
        IsMoving = true;

        if (Physics.Raycast(transform.position + new Vector3(dir.x, 0, dir.y), Vector3.down, 100f, GroundLayer))
        {
            transform.DOMove(transform.position + new Vector3(dir.x, 0, dir.y), moveSec).SetEase(Ease.InOutQuad).onComplete += () =>
                Timer(Bpm._beatIntervalD8Sec * 3, () => IsMoving = false);
        }
        else
        {
            Vector3 pos = transform.position;
            transform.DOMove(transform.position + new Vector3(dir.x, 0, dir.y) * 0.5f, moveSec * 0.5f).SetEase(Ease.InOutQuad).onComplete += () =>
            transform.DOMove(pos, moveSec * 0.5f).SetEase(Ease.InOutQuad).onComplete += () =>
                Timer(Bpm._beatIntervalD8Sec * 3, () => IsMoving = false);
        }
        if (dir != Vector2.zero)
        {
            transform.DOLookAt(transform.position + new Vector3(dir.x, 0, dir.y), moveSec * 0.5f);
        }
        //Play Jump Animation
    }
    public void AddCombo()
    {
        Combo++;
    }
    public void LoseCombo()
    {
        Combo = 0;
    }
    public void Attack()
    {
        if (Combo != 0)
        {
            PlayerAttack.Attack();
        }
    }
    private void ClearInput()
    {
        InputDir = Vector2.zero;
    }
    private void AddEmpty()
    {
        //if no input, record this beat as empty move
        if (InputDir == Vector2.zero)
        {
            MoveCommand m = new MoveCommand(InputDir);
            m.Excute();
            MoveRecord.Push(m);
        }
    }

    public void ScalePop()
    {
        transform.DOScale(0.8f, Bpm._beatIntervalD8Sec).From(0.9f, true);
    }

    public void Rewind()
    {
        StartCoroutine(DoRewind());
    }

    IEnumerator DoRewind()
    {

        IsRewinding = true;
        while (MoveRecord.Count > 0)
        {
            MoveRecord.Pop().Undo();
            yield return new WaitForSeconds(Bpm._beatIntervalSec / 5f);
        }

        //Rewind CoolDown
        yield return new WaitForSeconds(Bpm._beatIntervalSec);

        IsRewinding = false;
    }

    public void Hurt(int damage)
    {
        HP -= damage;
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
