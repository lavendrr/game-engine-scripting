using UnityEngine;

namespace CharacterEditor
{
    public class MaterialManager : MonoBehaviour
    {
        private static MaterialManager instance;

        [Header("Arm Materials")]
        [SerializeField] private Material m_Arm01;
        [SerializeField] private Material m_Arm02;
        [SerializeField] private Material m_Arm03;

        [Header("Leg Materials")]
        [SerializeField] private Material m_Leg01;
        [SerializeField] private Material m_Leg02;
        [SerializeField] private Material m_Leg03;

        [Header("Head Materials")]
        [SerializeField] private Material m_Head01;
        [SerializeField] private Material m_Head02;
        [SerializeField] private Material m_Head03;

        [Header("Body Materials")]
        [SerializeField] private Material m_Body01;
        [SerializeField] private Material m_Body02;
        [SerializeField] private Material m_Body03;        

        private void Awake()
        {
            instance = this;
        }

        public static Material Get(BodyTypes bodyType, int id)
        {
            switch (bodyType)
            {
                case BodyTypes.Arm: return instance.GetArm(id);
                case BodyTypes.Leg: return instance.GetLeg(id);
                case BodyTypes.Head: return instance.GetHead(id);
                case BodyTypes.Body: return instance.GetBody(id);
                default: return instance.GetArm(id);
            }
        }

        private Material GetArm(int id)
        {
            switch (id)
            {
                case 0: return m_Arm01;
                case 1: return m_Arm02;
                case 2: return m_Arm03;
                default: return m_Arm03;
            }
        }

        private Material GetLeg(int id)
        {
            switch (id)
            {
                case 1: return m_Leg01;
                case 2: return m_Leg02;
                case 3: return m_Leg03;
                default: return m_Leg03;
            }
        }

        private Material GetHead(int id)
        {
            switch (id)
            {
                case 1: return m_Head01;
                case 2: return m_Head02;
                case 3: return m_Head03;
                default: return m_Head03;
            }
        }

        private Material GetBody(int id)
        {
            switch (id)
            {
                case 1: return m_Body01;
                case 2: return m_Body02;
                case 3: return m_Body03;
                default: return m_Body03;
            }
        }
    }
}