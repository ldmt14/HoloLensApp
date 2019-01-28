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
        [SerializeField]
        [Tooltip("If set to true the printed object will be set to match the size specified in the .stl-File. Otherwise it will be set to fit inside a sphere with a radius of the \"scale factor\".")]
        private bool displayObjectInRealSize = true;
        [SerializeField]
        [Tooltip("Sets the scale of the printed object. If the .stl-File contains coordinates in millimeters this should be set to 0.001.")]
        private float scaleFactor = 0.001f;
        private Vector3 positionOfPrintedObject;

        public void Start()
        {
            if (PrintedObject != null)
            {
                positionOfPrintedObject = PrintedObject.transform.localPosition;
            }
        }

        public void UpdateObjectPrinted(Mesh mesh)
        {
            float localScale = displayObjectInRealSize ? scaleFactor : (scaleFactor / mesh.bounds.max.magnitude);
            PrintedObject.transform.localPosition = positionOfPrintedObject + (new Vector3(-mesh.bounds.center.x, mesh.bounds.size.y, -mesh.bounds.center.z) * localScale);
            PrintedObject.transform.localScale = new Vector3(localScale, localScale, localScale);
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
