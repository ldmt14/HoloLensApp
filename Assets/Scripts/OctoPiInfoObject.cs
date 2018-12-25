using UnityEngine;
using UnityEngine.UI;

namespace OctoPi
{
    public class OctoPiInfoObject : MonoBehaviour
    {
        public Slider ProgressBar;
        public TemperatureUI TemperatureBar;
        public Text FileNameText;
        public GameObject ObjectPrinted;

        public void UpdateObjectPrinted(GameObject newObject)
        {
            if (ObjectPrinted != null)
            {
                Destroy(ObjectPrinted);
            }
            ObjectPrinted = newObject;
            newObject.transform.parent = transform;
            newObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
