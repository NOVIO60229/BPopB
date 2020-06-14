using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public abstract class AttackPattern : ScriptableObject
{
    //How many beats the Update will persist
    [HeaderAttribute("Persist beats")]
    public int beatFull = 0;
    public int beatD8 = 0;
    [HeaderAttribute("Excute beats")]
    public bool isBeatFull = false;
    public bool isBeatD8 = false;
    public List<CheckBeats> checkBeatList;

    protected BPM_Counter bpm;

    public virtual void Initialize()
    {
        bpm = BPM_Counter.instance;
    }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void End() { }
    public bool Check()
    {
        if (isBeatD8)
        {
            return bpm._isBeatD8;
        }
        if (isBeatFull)
        {
            return bpm._isBeatFull;
        }
        foreach (CheckBeats check in checkBeatList)
        {
            if (check.full != 0 && check.D8 != 0)
            {
                if (bpm.CheckBeatD8(check.full, check.D8))
                {
                    return true;
                }
            }
            else if (check.full != 0)
            {
                if (bpm.CheckBeatFull(check.full))
                {
                    return true;
                }
            }
            else if (check.D8 != 0)
            {
                if (bpm.CheckBeatD8(check.D8))
                {
                    return true;
                }
            }
            else
            {
                Debug.Log("not set any checkBeats");
            }
        }
        return false;
    }
}


[System.Serializable]
public class CheckBeats
{
    public int full = 0;
    public int D8 = 0;
}

