// Ricky Moctezuma - Game Engine Scripting Spring 2024
using UnityEngine;
using TMPro;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI display;
    public TextMeshProUGUI operatorDisplay;

    float operand1 = 0f;
    float operand2 = 0f;

    bool clearPrevInput = true;

    // Variable to keep track of whether the operation has been changed since the last calculation, for use in consecutive equals button calculations and automatic calculations when inputting consecutive operations without using equals
    bool operatorChanged = false;

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

        // If the input is the word "sign" (as sent by the +/- button), adds or removes the negative sign to the start of the string
        if (input == "sign"){
            if (display.text.Contains("-")){
                display.text = display.text.Remove(0, 1);
            } else {
                display.text = '-' + display.text;
            }
        // If the input is a number, add the number to the current display text
        // If the input is a decimal, check if the display already has a decimal. If so, then don't add another decimal, otherwise add it like normal
        } else if (!(input == "." && display.text.Contains('.'))){ 
            display.text += input;
        }
    }


    public void SetEquationType(string input)
    {
        // Check if a new operation is being entered consecutively without hitting equals - if so, automatically calculate the previous operation
        if (operatorChanged == true){
            Calculate();
            // operatorChanged is set to false by Calculate(), but since we are calculating as part of entering a new operation, we'll set it back to true
            operatorChanged = true;
        } else {
            operatorChanged = true;
        }

        // Stores the current value in operand1 and makes it so the next number will start a new input string
        operand1 = float.Parse(display.text);
        clearPrevInput = true;

        // Sets the internal operation type and updates the user-facing operation display according to input
        if (input == "+"){
            equationType = EquationType.ADD;
            operatorDisplay.text = "+";
        } else if (input == "-"){
            equationType = EquationType.SUBTRACT;
            operatorDisplay.text = "-";
        } else if (input == "*"){
            equationType = EquationType.MULTIPLY;
            operatorDisplay.text = "*";
        } else {
            equationType = EquationType.DIVIDE;
            operatorDisplay.text = "/";
        }

        Debug.Log(equationType);
    }

    public void Clear()
    {
        // Reset all values
        display.text = "0";
        operatorDisplay.text = "";
        clearPrevInput = true;
        operand1 = 0f;
        operand2 = 0f;
        equationType = EquationType.None;
    }

    public void Calculate()
    {
        // Check if the same operation is being repeated (hitting equals without entering a new operation), and if so, use the current value as operand1 instead of operand2 to preserve the previous operation
        if (operatorChanged == true){
            operand2 = float.Parse(display.text);
            operatorChanged = false;
        } else {
            operand1 = float.Parse(display.text);
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
