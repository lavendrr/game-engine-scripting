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
        // Assign references and initialize states
        bladeScript = gameObject.GetComponent<BladeManager>();

        DealState = new DealState(bladeScript, this);
        DrawState = new DrawState(bladeScript, this);
        PlayerActionState = new PlayerActionState(bladeScript, this);
        PCActionState = new PCActionState(bladeScript, this);
        GameEndState = new GameEndState(bladeScript, this, endDialog);

        // Enter the deal state upon game start
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
        // Exit the current state
        if (currentState != null)
        {
            currentState.OnExit();
        }

        // Enter the new state after the delay is complete
        StartCoroutine(Delay(delayTime, done => {
            currentState = newState;
            currentState.OnEnter();
        }));
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    // Function to add a delay to any action, used when changing states
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
        // Assign and create player cards, entering the draw state after completion
        base.OnEnter();
        blade.AssignCards();
        blade.StartCoroutine(blade.CreateCards(done => {
            sc.ChangeState(sc.DrawState, 0f);
        }));
    }

    public override void OnExit()
    {
        base.OnExit();
    }

}

public class DrawState : State
{
    public DrawState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    private GameObject deck1, hand1;

    public override void OnEnter()
    {
        base.OnEnter();
        deck1 = GameObject.Find("Deck1");
        hand1 = GameObject.Find("Hand1");

        // Reset the game field
        blade.Redraw();
        // Enable the deck if it still has cards
        if (deck1 != null)
        {
            deck1.GetComponent<Button>().interactable = true;
        }
        // Enable the hand if the deck is out of cards
        else if (hand1 != null)
        {
            hand1.GetComponent<CanvasGroup>().interactable = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        // Disable the deck or hand depending on what was active
        if (deck1 != null)
        {
            deck1.GetComponent<Button>().interactable = false;
        }
        else if (hand1 != null)
        {
            hand1.GetComponent<CanvasGroup>().interactable = false;
        }

    }

}

public class PlayerActionState : State
{
    public PlayerActionState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        // Check if the player has cards left to play, and enable their hand
        base.OnEnter();

        blade.CheckPlayerCardsLeft();

        GameObject.Find("Hand1").GetComponent<CanvasGroup>().interactable = true;
    }

    public override void OnExit()
    {
        // Disable the player's hand
        base.OnExit();
        GameObject.Find("Hand1").GetComponent<CanvasGroup>().interactable = false;
    }

}

public class PCActionState : State
{
    public PCActionState(BladeManager bladeScript, StateController stateController) : base(bladeScript, stateController){}

    public override void OnEnter()
    {
        // Make the PC play a card
        base.OnEnter();
        blade.PCPlayCard();
    }

    public override void OnExit()
    {
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
        // Enable the game end dialog
        endDialog.SetActive(true);
        // Assign the end text and box color according to if the player won or lost
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