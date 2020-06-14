using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trap : MonoBehaviour
{
    [HeaderAttribute("Excute beats")]
    public bool isBeatFull = false;
    public List<CheckBeats> checkBeatList;

    protected BPM_Counter Bpm;
    protected Animation Anim;
    private void Start()
    {
        Anim = GetComponent<Animation>();
        Anim[Anim.clip.name].speed = BPM_Counter.instance.AnimPlaySpeed;
        Bpm = BPM_Counter.instance;
    }


    public bool Check()
    {
        if (isBeatFull)
        {
            return Bpm._isBeatFull;
        }
        foreach (CheckBeats check in checkBeatList)
        {
            if (check.full != 0 && check.D8 != 0)
            {
                if (Bpm.CheckBeatD8(check.full, check.D8))
                {
                    return true;
                }
            }
            else if (check.full != 0)
            {
                if (Bpm.CheckBeatFull(check.full))
                {
                    return true;
                }
            }
            else if (check.D8 != 0)
            {
                if (Bpm.CheckBeatD8(check.D8))
                {
                    return true;
                }
            }
            else
            {
                Debug.Log(this + "not set any checkBeats");
            }
        }
        return false;
    }

    public virtual void Excute()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Hurt(10);
        }
    }


    [System.Serializable]
    public class CheckBeats
    {
        public int full = 0;
        public int D8 = 0;
    }
}
