using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    Vector2 Dir;
    public MoveCommand(Vector2 dir)
    {
        Dir = dir;
    }

    public void Excute()
    {
        Player.Instance.Move(Dir, BPM_Counter.instance._beatIntervalD8Sec * 2);
    }

    public void Undo()
    {
        Player.Instance.Move(-Dir, BPM_Counter.instance._beatIntervalD8Sec * 2);
    }
}
