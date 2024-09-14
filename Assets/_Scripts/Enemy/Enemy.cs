using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyLoadSave))]
public abstract class Enemy : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] protected ParticleSystem bloodSplatter;

    protected float health;
    [SerializeField] protected float MAX_HEALTH;

    [SerializeField] protected GameObject drop;

    protected bool isDead = false;
    protected bool multipleDeaths = false;



    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();

        EnemyLoadSave enemyLoadSave = GetComponent<EnemyLoadSave>();

        if (MAX_HEALTH <= 0) MAX_HEALTH = 4;

        health = MAX_HEALTH;
    }

    // Update is called once per frame
    public virtual void hit(int damage, string damageType)
    {
        health -= damage;
    }

    public virtual void hitPlayer(Collider player)
    {

    }

    public virtual void SaveEnemy()
    {

    }

    public virtual void LoadEnemy() 
    {

    }
}
