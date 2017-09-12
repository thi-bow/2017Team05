using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステートマシンクラス
public class StateMachine<T> {

    private State<T> m_currentState;
   
    public StateMachine()
    {
        m_currentState = null;
    }

    public State<T> GetCurrentState()
    {
        return m_currentState;
    }
	
    public void ChengeState(State<T> state)
    {
        if(m_currentState != null)
        {
            m_currentState.OnExit();
        }

        m_currentState = state;
        m_currentState.OnEnter();
    }

	public void Update () {
		if(m_currentState != null)
        {
            m_currentState.OnExecute();
        }
	}
}


// ステートの基底クラス
public class State<T>
{
    protected T _base;

    public State(T _base)
    {
        this._base = _base;
    }

    public virtual void OnEnter() { }

    public virtual void OnExecute() { }

    public virtual void OnExit() { }
}
