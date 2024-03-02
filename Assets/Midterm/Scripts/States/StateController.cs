using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateController : MonoBehaviour
{
    State currentState;

    BladeManager bladeScript;

    public DealState DealState;
    public DrawState DrawState;
    public PlayerActionState PlayerActionState;
    public PCActionState PCActionState;
    public GameEndState GameEndState;


    private void Start()
    {
        bladeScript = gameObject.GetComponent<BladeManager>();

        DealState = new DealState(bladeScript, this);
        DrawState = new DrawState(bladeScript, this);
        PlayerActionState = new PlayerActionState(bladeScript, this);
        PCActionState = new PCActionState(bladeScript, this);
        GameEndState = new GameEndState(bladeScript, this);

        ChangeState(DealState, 0f);
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(State newState, float delayTime)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        StartCoroutine(Delay(delayTime, done => {
            currentState = newState;
            currentState.OnEnter();
        }));
    }

    private IEnumerator Delay(float time, System.Action<bool> done)
    {
        yield return new WaitForSeconds(time);
        done(true);
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

public class DealState : State
{
    public DealState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        Debug.Log("Entering Deal State");
        base.OnEnter();
        blade.AssignCards();
        blade.StartCoroutine(blade.CreateCards(done => {
            Debug.Log("Changing state after coroutine");
            sc.ChangeState(sc.DrawState, 0f);
        }));
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting deal state!");
    }

}

public class DrawState : State
{
    public DrawState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        Debug.Log("Entered Draw state!");
        base.OnEnter();
        blade.Redraw();
        GameObject.Find("Deck1").GetComponent<Button>().interactable = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        GameObject.Find("Deck1").GetComponent<Button>().interactable = false;
        Debug.Log("Exited Draw state!");
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

    public override void OnEnter()
    {
        Debug.Log("Entering PC Action State!");
        base.OnEnter();
    }

}

public class GameEndState : State
{
    public GameEndState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered game end state!");
    }

}