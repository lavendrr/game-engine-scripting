// Ricky Moctezuma - Game Engine Scripting Assignment 10
using System.IO;
using UnityEngine;
using UnityEditor;
using TMPro;

public class LevelEditor : MonoBehaviour
{
    public LevelData levelData;
    private TextMeshProUGUI errorTextObj;
    private string errorText;
    private bool succeeded;

    void Awake()
    {
        errorTextObj = GameObject.Find("errorText").GetComponent<TextMeshProUGUI>();
    }

    public void SaveLevel()
    {
        // Reset error text and success state
        errorTextObj.text = "";
        errorText = "";
        succeeded = true;

        // Tries to parse the input field into an int and assigns it to the class attribute. If this fails, or if the resulting int is negative, the check fails and the data will not be saved.
        if (!int.TryParse(GameObject.Find("lvlNumber").GetComponent<TMP_InputField>().text, out levelData.lvlNumber) || levelData.lvlNumber < 0)
        {
            errorText += "Please input a valid level number (positive int).\n";
            succeeded = false;
        }

        // Assigns the input field to the name attribute. If the name is empty, the check fails and the data will not be saved.
        levelData.lvlName = GameObject.Find("lvlName").GetComponent<TMP_InputField>().text;
        if (levelData.lvlName == "")
        {
            errorText += "Please input a level name.\n";
            succeeded = false;
        }

        // Tries to parse the input field into an int and assigns it to the class attribute. If this fails, or if the resulting int is negative, the check fails and the data will not be saved.
        if (!int.TryParse(GameObject.Find("reward").GetComponent<TMP_InputField>().text, out levelData.reward) || levelData.reward < 0)
        {
            errorText += "Please input a valid number of reward coins (positive int).\n";
            succeeded = false;
        }

        // Tries to parse the input field into an int and assigns it to the class attribute. If this fails, or if the resulting int is negative, the check fails and the data will not be saved.
        if (!int.TryParse(GameObject.Find("enemies").GetComponent<TMP_InputField>().text, out levelData.enemies) || levelData.enemies < 0)
        {
            errorText += "Please input a valid number of enemies (positive int).\n";
            succeeded = false;
        }

        if (succeeded)
        {
            // Create the file path and appropriate stream and writer objects
            string path = $"Assets/Assignment-10/Level Data/Level_{levelData.lvlNumber}.txt";
            FileStream stream = new FileStream(path, FileMode.Create); // The Create mode allows the program to overwrite the existing file with the same name if it exists
            StreamWriter writer = new StreamWriter(stream);

            // Write the data and close the objects
            writer.WriteLine(JsonUtility.ToJson(levelData));
            writer.Close();
            stream.Close();

            // Import the new txt file
            AssetDatabase.ImportAsset(path);
            errorTextObj.text = $"Successfully saved level {levelData.lvlNumber}!";
        }
        else
        {
            errorTextObj.text = errorText;
        }
    }
}
