using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public IdleState(GameObject zombi, Animator animator, NavMeshAgent agent, Transform playerTransform) : base(zombi, animator, agent, playerTransform)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (false)
        {
            nextState = new PursueState(GameObject zombi, Animator animator, NavMeshAgent agent, Transform playerTransform);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
