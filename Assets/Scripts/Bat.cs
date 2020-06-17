using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Bat : HurtArea
{
    float interval;
    private void Start()
    {
        interval = BPM_Counter.instance._beatIntervalSec;
        Attack();
    }
    private void Update()
    {
    }

    public void Attack()
    {
        StartCoroutine(DoAttack());
    }

    IEnumerator DoAttack()
    {
        var player = Player.Instance;
        var oriY = transform.position.y;
        var playerPos = Player.Instance.transform.position;
        var endPos = transform.position + (2f * new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z));

        transform.DOMoveY(transform.position.y + 1f, interval * 2);

        yield return new WaitForSeconds(interval * 2f);

        transform.DOMoveX(endPos.x, interval);
        transform.DOMoveZ(endPos.z, interval);
        transform.DOMoveY(playerPos.y, interval * 0.5f);

        yield return new WaitForSeconds(interval * 0.5f);

        transform.DOMoveY(oriY, interval * 0.5f);
        transform.DOScale(0, interval).onComplete += () => Destroy(gameObject); ;
    }
}
