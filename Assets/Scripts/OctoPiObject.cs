using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace OctoPi
{
    public class OctoPiObject : InspectableObject
    {
        public OctoPiInfoObject octoPiInfo;

        public override void Help()
        {
            throw new System.NotImplementedException();
        }

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
            OctoPiClient.GetJobInformation(OnJobInformationRecieved);
            OctoPiClient.GetStateInformation((success, response) =>
            {
                if (!success) return;
                octoPiInfo.TemperatureBar.TemperatureData = response.temperature.tool0;
            });
        }

        private void OnJobInformationRecieved(bool jobInformationSuccess, JobInformationResponse response)
        {
            if (!jobInformationSuccess) return;
            octoPiInfo.ProgressBar.value = response.progress.completion * octoPiInfo.ProgressBar.maxValue;
            if (octoPiInfo.ObjectPrinted != null && octoPiInfo.FileNameText.text.Equals(response.job.file.name))
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
            OctoPiClient.GetFileInformation("local", stlFileName, (fileInformationSuccess, fileInfo) =>
            {
                if (fileInformationSuccess)
                {
#if WINDOWS_UWP
                    string stlStoragePath = KnownFolders.Objects3D.Path + "/" + stlFileName;
                    string objStoragePath = KnownFolders.Objects3D.Path + "/" + objFileName;
                    OctoPiClient.GetAndStoreFile(fileInfo.refs.download, stlStoragePath, (getAndStoreSuccess) =>
                    {
                        if (!getAndStoreSuccess) return;
                        StlConverter.Converter.Convert(stlStoragePath, objStoragePath);

                        Mesh holderMesh = new Mesh();
                        ObjImporter newMesh = new ObjImporter();
                        holderMesh = newMesh.ImportFile(objStoragePath);

                        GameObject cube = new GameObject();

                        MeshRenderer renderer = cube.AddComponent<MeshRenderer>();
                        MeshFilter filter = cube.AddComponent<MeshFilter>();
                        filter.mesh = holderMesh;
                        cube.transform.localScale = 0.01f * cube.transform.localScale;
                        octoPiInfo.UpdateObjectPrinted(cube);
                    });
#endif
                }
            });
        }
    }
}