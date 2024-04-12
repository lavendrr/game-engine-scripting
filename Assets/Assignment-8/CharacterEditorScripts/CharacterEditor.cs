using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CharacterEditor
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] Button nextMaterial;
        [SerializeField] Button nextBodyPart;
        [SerializeField] Button loadGame;

        [SerializeField] Character character;

        int id = 0;
        BodyTypes bodyType = BodyTypes.Head;

        private void Awake()
        {
            nextMaterial.onClick.AddListener(NextMaterial);
            nextBodyPart.onClick.AddListener(NextBodyPart);
            loadGame.onClick.AddListener(LoadGame);
        }

        void NextMaterial()
        {
            if (id < 2)
            {
                id++;
            }
            else
            {
                id = 0;
            }

            switch(bodyType)
            {
                case BodyTypes.Head:
                    PlayerPrefs.SetInt("headID", id);
                    break;
                case BodyTypes.Body:
                    PlayerPrefs.SetInt("bodyID", id);
                    break;
                case BodyTypes.Arm:
                    PlayerPrefs.SetInt("armID", id);
                    break;
                case BodyTypes.Leg:
                    PlayerPrefs.SetInt("legID", id);
                    break;
            }

            character.Load();
        }

        void NextBodyPart()
        {
            switch(bodyType)
            {
                case BodyTypes.Head:
                    bodyType = BodyTypes.Body;
                    id = PlayerPrefs.GetInt("bodyID");
                    break;
                case BodyTypes.Body:
                    bodyType = BodyTypes.Arm;
                    id = PlayerPrefs.GetInt("armID");
                    break;
                case BodyTypes.Arm:
                    bodyType = BodyTypes.Leg;
                    id = PlayerPrefs.GetInt("legID");
                    break;
                case BodyTypes.Leg:
                    bodyType = BodyTypes.Head;
                    id = PlayerPrefs.GetInt("headID");
                    break;
            }
        }

        void LoadGame()
        {
            SceneManager.LoadScene("CharacterGame");
        }
    }
}