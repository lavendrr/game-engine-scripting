using UnityEngine;
using TMPro;
//using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI display;
    public TextMeshProUGUI display2;

    float operand1 = 0f;
    float operand2 = 0f;

    bool clearPrevInput = true;
    bool opChanged = false;

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
        operand1 = float.Parse(display.text);
        clearPrevInput = true;

        if (input == "+"){
            equationType = EquationType.ADD;
            display2.text = "+";
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

        opChanged = true;

        Debug.Log(equationType);
    }

    public void Clear()
    {
        // Reset all values
        display.text = "0";
        display2.text = "";
        clearPrevInput = true;
        operand1 = 0f;
        operand2 = 0f;
        equationType = EquationType.None;
    }

    public void Calculate()
    {
        // Check if the same operation is being repeated (hitting equals without entering a new operation), and if so, use the current value as operand1 instead of operand2 to preserve the operation
        if (opChanged == false){
            operand1 = float.Parse(display.text);
        } else {
            operand2 = float.Parse(display.text);
            opChanged = false;
        }
        
        float result = 0f;

        // Compute the operation according to current equation type
        if (equationType == EquationType.ADD){
            result = operand1 + operand2;
        } else if (equationType == EquationType.SUBTRACT){
            result = operand1 - operand2;
        } else if (equationType == EquationType.MULTIPLY){
            result = operand1 * operand2;
        } else if (equationType == EquationType.DIVIDE){
            result = operand1 / operand2;
        }

        display.text = result.ToString();
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
