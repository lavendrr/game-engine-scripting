using UnityEngine;
using TMPro;
//using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI display;
    public TextMeshProUGUI display2;

    float prevInput = 0f;

    bool clearPrevInput = true;

    private EquationType equationType;

    private void Start()
    {
        Clear();
    }

    public void AddInput(string input)
    {

        // Starts a new input string if clearPrevInput is true
        if (clearPrevInput == true){
            display.text = string.Empty;
            clearPrevInput = false;
        }


        // If the input is the change sign variable, adds or removes the negative sign to the start of the string
        if (input == "sign"){
            if (display.text.Contains("-")){
                display.text = display.text.Remove(0, 1);
            } else {
                display.text = '-' + display.text;
            }
        // If the input is a decimal, check if the display already has a decimal. If so, then don't add another decimal
        // If the input is a number, add the number to the current display number
        } else if (!(input == "." && display.text.Contains('.'))){ 
            display.text += input;
        }
    }


    public void SetEquationType(string input){
        prevInput = float.Parse(display.text);
        clearPrevInput = true;

        if (input == "+"){
            equationType = EquationType.ADD;
            display2.text = "+";
            //Image addButton = GameObject.Find("BPlus").GetComponent<Image>();
        } else if (input == "-"){
            equationType = EquationType.SUBTRACT;
            display2.text = "-";
        } else if (input == "*"){
            equationType = EquationType.MULTIPLY;
            display2.text = "*";
        } else {
            equationType = EquationType.DIVIDE;
            display2.text = "/";
        }

        Debug.Log(equationType);
    }

    public void Clear()
    {
        // Reset all values
        display.text = "0";
        display2.text = "";
        clearPrevInput = true;
        prevInput = 0f;
        equationType = EquationType.None;
    }

    public void Calculate()
    {
        float currentInput = float.Parse(display.text);

        if (equationType == EquationType.ADD){
            display.text = (prevInput + currentInput).ToString();
        } else if (equationType == EquationType.SUBTRACT){
            display.text = (prevInput - currentInput).ToString();
        } else if (equationType == EquationType.MULTIPLY){
            display.text = (prevInput * currentInput).ToString();
        } else if (equationType == EquationType.DIVIDE){
            display.text = (prevInput / currentInput).ToString();
        }
    }

    public enum EquationType
    {
        None = 0,
        ADD = 1,
        SUBTRACT = 2,
        MULTIPLY = 3,
        DIVIDE = 4
    }
}
