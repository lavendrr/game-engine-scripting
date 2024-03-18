using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateController : MonoBehaviour
{
    State currentState;

    BladeManager bladeScript;

    public DealState DealState;
    public DrawState DrawState;
    public PlayerActionState PlayerActionState;
    public PCActionState PCActionState;
    public GameEndState GameEndState;
    [SerializeField]
    GameObject endDialog;

    private void Start()
    {
        bladeScript = gameObject.GetComponent<BladeManager>();

        DealState = new DealState(bladeScript, this);
        DrawState = new DrawState(bladeScript, this);
        PlayerActionState = new PlayerActionState(bladeScript, this);
        PCActionState = new PCActionState(bladeScript, this);
        GameEndState = new GameEndState(bladeScript, this, endDialog);

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

    public State GetCurrentState()
    {
        return currentState;
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

    private GameObject deck1, hand1;

    public override void OnEnter()
    {
        Debug.Log("Entered Draw state!");
        base.OnEnter();
        deck1 = GameObject.Find("Deck1");
        hand1 = GameObject.Find("Hand1");

        blade.Redraw();
        
        if (deck1 != null)
        {
            deck1.GetComponent<Button>().interactable = true;
        }
        else if (hand1 != null)
        {
            hand1.GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        if (deck1 != null)
        {
            deck1.GetComponent<Button>().interactable = false;
        }
        else if (hand1 != null)
        {
            hand1.GetComponent<CanvasGroup>().interactable = false;
        }

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

        blade.CheckPlayerCardsLeft();

        GameObject.Find("Hand1").GetComponent<CanvasGroup>().interactable = true;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting Player Action State!");
        base.OnExit();
        GameObject.Find("Hand1").GetComponent<CanvasGroup>().interactable = false;
    }

}

public class PCActionState : State
{
    public PCActionState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        Debug.Log("Entering PC Action State!");
        base.OnEnter();
        blade.PCPlayCard();
    }

    public override void OnExit()
    {
        Debug.Log("Exiting PC Action State!");
        base.OnExit();
    }

}

public class GameEndState : State
{
    internal GameObject endDialog;
    public GameEndState(BladeManager bladeScript, StateController stateController, GameObject endDialog) : base(bladeScript, stateController){this.endDialog = endDialog;}
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered game end state!");
        endDialog.SetActive(true);
        if (blade.GetWinState())
        {
            endDialog.GetComponent<Image>().color = new Color(0.525f, 0.952f, 0.498f, 0.5f);
            TextMeshProUGUI endText = GameObject.Find("EndText").GetComponent<TextMeshProUGUI>();
            endText.text = "Game won!";
        }
        else
        {
            endDialog.GetComponent<Image>().color = new Color(0.898f, 0.333f, 0.267f, 0.5f);
            TextMeshProUGUI endText = GameObject.Find("EndText").GetComponent<TextMeshProUGUI>();
            endText.text = "Game lost!";
        }

    }

}