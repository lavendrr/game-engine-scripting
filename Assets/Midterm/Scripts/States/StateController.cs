using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateController : MonoBehaviour
{
    State currentState;

    BladeManager bladeScript;

    public DrawState DrawState;
    public PlayerActionState PlayerActionState;
    public PCActionState PCActionState;
    public GameEndState GameEndState;


    private void Start()
    {
        bladeScript = gameObject.GetComponent<BladeManager>();

        DrawState = new DrawState(bladeScript, this);
        PlayerActionState = new PlayerActionState(bladeScript, this);
        PCActionState = new PCActionState(bladeScript, this);
        GameEndState = new GameEndState(bladeScript, this);

        ChangeState(DrawState);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
    }
}

public abstract class State
{
    internal StateController sc;
    internal BladeManager blade;
    
    public State(BladeManager bladeScript, StateController stateController)
    {
        blade = bladeScript;
        sc = stateController;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}

public class DrawState : State
{
    public DrawState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        Debug.Log("Entering Draw State");
        base.OnEnter();
        blade.AssignCards();
        blade.StartCoroutine(blade.CreateCards(done => {
            Debug.Log("Changing state after coroutine");
            sc.ChangeState(sc.PlayerActionState);
        }));
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting draw state!");
    }

}

public class PlayerActionState : State
{
    public PlayerActionState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        Debug.Log("Entering Player Action State!");
        base.OnEnter();
        GameObject.Find("Hand1").GetComponent<CanvasGroup>().interactable = true;
    }

}

public class PCActionState : State
{
    public PCActionState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}
    
}

public class GameEndState : State
{
    public GameEndState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}
    
}