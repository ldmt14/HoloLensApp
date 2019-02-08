using System.Collections.Generic;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace OctoPi
{
    public class OctoPiObject : InspectableObject
    {
        public OctoPiInfoObject octoPiInfo;
        [SerializeField]
        private string domain = "http://10.10.10.13";

        [SerializeField, Tooltip("If the printed Object consists of more Trianlges than this number its hologram will not be displayed. Set to zero for unlimited number of triangles.")]
        private int maxNumberOfTriangles = 10000;

        // Use this for initialization
        void Start()
        {
            InvokeRepeating("UpdateUI", 2, 2);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateUI()
        {
            OctoPiClient.GetJobInformation(domain, OnJobInformationRecieved);
            OctoPiClient.GetStateInformation(domain, (success, response) =>
            {
                if (!success) return;
                octoPiInfo.TemperatureBar.TemperatureData = response.temperature.tool0;
            });
        }

        private void OnJobInformationRecieved(bool jobInformationSuccess, JobInformationResponse response)
        {
            if (!jobInformationSuccess) return;
            octoPiInfo.ProgressBar.value = response.progress.completion * octoPiInfo.ProgressBar.maxValue;
            if (octoPiInfo.PrintedObject != null && octoPiInfo.FileNameText.text.Equals(response.job.file.name))
            {
                return;
            }
            octoPiInfo.FileNameText.text = response.job.file.name;
            string fileName = response.job.file.path ?? response.job.file.name;
            int lastDot = fileName.LastIndexOf('.');
            if (lastDot < 0)
            {
                return;
            }
            string stlFileName = fileName.Substring(0, lastDot) + ".stl";
            string objFileName = fileName.Substring(0, lastDot) + ".obj";
            OctoPiClient.GetFileInformation(domain, "local", stlFileName, (fileInformationSuccess, fileInfo) =>
            {
                if (fileInformationSuccess)
                {
                    OctoPiClient.GetFile(fileInfo.refs.download, (getAndStoreSuccess, text) =>
                    {
                        if (!getAndStoreSuccess) return;
                        string objText = StlConverter.Converter.ConvertStlText(text, maxNumberOfTriangles);
                        if (objText == null)
                        {
                            return;
                        }
                        Mesh holderMesh = new Mesh();
                        ObjImporter newMesh = new ObjImporter();
                        holderMesh = newMesh.ImportFileFromText(objFileName, objText);
                        octoPiInfo.UpdateObjectPrinted(holderMesh);
                    });
                }
            });
        }
    }
}