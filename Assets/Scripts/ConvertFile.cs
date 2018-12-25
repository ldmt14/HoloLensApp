using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
#if WINDOWS_UWP
using Windows.Storage;
#endif
public class ConvertFile : MonoBehaviour {

    string stlObject = @"solid model
facet normal 0.0 0.0 -1.0
outer loop
vertex 20.0 0.0 0.0
vertex 0.0 -20.0 0.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal 0.0 0.0 -1.0
outer loop
vertex 0.0 -20.0 0.0
vertex 20.0 0.0 0.0
vertex 20.0 -20.0 0.0
endloop
endfacet
facet normal -0.0 -1.0 -0.0
outer loop
vertex 20.0 -20.0 20.0
vertex 0.0 -20.0 0.0
vertex 20.0 -20.0 0.0
endloop
endfacet
facet normal -0.0 -1.0 -0.0
outer loop
vertex 0.0 -20.0 0.0
vertex 20.0 -20.0 20.0
vertex 0.0 -20.0 20.0
endloop
endfacet
facet normal 1.0 0.0 0.0
outer loop
vertex 20.0 0.0 0.0
vertex 20.0 -20.0 20.0
vertex 20.0 -20.0 0.0
endloop
endfacet
facet normal 1.0 0.0 0.0
outer loop
vertex 20.0 -20.0 20.0
vertex 20.0 0.0 0.0
vertex 20.0 0.0 20.0
endloop
endfacet
facet normal -0.0 -0.0 1.0
outer loop
vertex 20.0 -20.0 20.0
vertex 0.0 0.0 20.0
vertex 0.0 -20.0 20.0
endloop
endfacet
facet normal -0.0 -0.0 1.0
outer loop
vertex 0.0 0.0 20.0
vertex 20.0 -20.0 20.0
vertex 20.0 0.0 20.0
endloop
endfacet
facet normal -1.0 0.0 0.0
outer loop
vertex 0.0 0.0 20.0
vertex 0.0 -20.0 0.0
vertex 0.0 -20.0 20.0
endloop
endfacet
facet normal -1.0 0.0 0.0
outer loop
vertex 0.0 -20.0 0.0
vertex 0.0 0.0 20.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal -0.0 1.0 0.0
outer loop
vertex 0.0 0.0 20.0
vertex 20.0 0.0 0.0
vertex 0.0 0.0 0.0
endloop
endfacet
facet normal -0.0 1.0 0.0
outer loop
vertex 20.0 0.0 0.0
vertex 0.0 0.0 20.0
vertex 20.0 0.0 20.0
endloop
endfacet
endsolid model
";

	// Use this for initialization
	void Start ()
    {
        string folderPath = "";
#if WINDOWS_UWP
        StorageFolder folder = KnownFolders.Objects3D;
        folderPath = folder.Path;
#endif
        System.IO.File.WriteAllText(folderPath + "/cube.stl", stlObject);
        Debug.Log("File Written");
        StlConverter.Converter.Convert(folderPath + "/cube.stl", folderPath + "/cube.obj");
        Debug.Log("File Converted");
        Debug.Log(System.IO.File.ReadAllText(folderPath + "/cube.obj"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
