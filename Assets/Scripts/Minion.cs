using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Minion : MonoBehaviour, IDamagable
{

    //status
    private bool IsDying;

    //attacks
    public GameObject Bat;

    public ParticleSystem DeathParticle;

    public int HP { get; set; } = 100;

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

    private void Update()
    {
        if (HP <= 0)
        {
            Die();
        }

        if (Keyboard.current.rKey.isPressed)
        {
            SpawnBats();
        }
    }
    public virtual void Attack()
    {
        //transform.DOMove(transform.position + transform.forward, BPM_Counter.instance._beatIntervalD8Sec * 2);
    }

    public void ScalePop()
    {
        transform.DOScale(2f, BPM_Counter.instance._beatIntervalD8Sec).From(2.15f, true);
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
        if (IsDying) yield break;

        IsDying = true;
        var parti = Instantiate(DeathParticle, transform.position, Quaternion.identity);
        parti.Play();
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private void OnParticleCollision(GameObject other)
    {
        Hurt(5);
    }

    public void Hurt(int damage)
    {
        HP -= damage;
    }

    public void SpawnBats()
    {
        var spawnXY = Player.Instance.transform.position - transform.position;
        var spawnPos = new Vector3(-spawnXY.z, 0, spawnXY.x).normalized;
        Instantiate(Bat, transform.position + spawnPos + Vector3.up, Quaternion.identity);
        Instantiate(Bat, transform.position - spawnPos + Vector3.up, Quaternion.identity);
    }
}
