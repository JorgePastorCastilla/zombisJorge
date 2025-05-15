using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{

    public enum STATE { IDLE, PURSUE, ATTACK, RUNAWAY };
    
    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    protected GameObject zombi;
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Transform playerTransform;
    protected State nextState;

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float attackDist = 7.0f;

    public State(GameObject zombi, Animator animator, NavMeshAgent agent, Transform playerTransform)
    {
        this.zombi = zombi;
        this.animator = animator;
        this.agent = agent;
        this.playerTransform = playerTransform;
        stage = EVENT.ENTER;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = EVENT.EXIT;
    }

    public State Process()
    {
        if (stage == EVENT.ENTER)
        {
            Enter();
        }
        if (stage == EVENT.UPDATE)
        {
            Update();
        }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }


}
