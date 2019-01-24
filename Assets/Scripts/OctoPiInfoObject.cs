using UnityEngine;
using UnityEngine.UI;

namespace OctoPi
{
    public class OctoPiInfoObject : MonoBehaviour
    {
        public Slider ProgressBar;
        public TemperatureUI TemperatureBar;
        public Text FileNameText;
        [SerializeField]
        internal GameObject PrintedObject;

        public void UpdateObjectPrinted(Mesh mesh)
        {
            MeshRenderer renderer = PrintedObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                renderer = PrintedObject.AddComponent<MeshRenderer>();
            }
            MeshFilter filter = PrintedObject.GetComponent<MeshFilter>();
            if (filter == null)
            {
                filter = PrintedObject.AddComponent<MeshFilter>();
            }
            filter.mesh = mesh;
        }

        public void OnDisable()
        {
            PrintedObject.SetActive(false);
        }

        public void OnEnable()
        {
            PrintedObject.SetActive(true);
        }
    }
}
