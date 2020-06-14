using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Minion : MonoBehaviour
{

    public ParticleSystem DeathParticle;
    private void OnEnable()
    {
        BPM_Counter.OnBeatFull += ScalePop;
        BPM_Counter.OnBeatFull += Attack;
    }
    private void OnDisable()
    {
        BPM_Counter.OnBeatFull -= ScalePop;
        BPM_Counter.OnBeatFull -= Attack;
    }
    public virtual void Attack()
    {
        transform.DOMove(transform.position + transform.forward, BPM_Counter.instance._beatIntervalD8Sec * 2);
    }

    public void ScalePop()
    {
        transform.DOScale(0.5f, BPM_Counter.instance._beatIntervalD8Sec).From(0.7f, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.Hurt(10);
            Die();
        }
    }
    public void Die()
    {
        StartCoroutine(DoDie());
    }

    IEnumerator DoDie()
    {
        DeathParticle.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private void OnParticleCollision(GameObject other)
    {
        Die();
    }

}
