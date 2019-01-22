using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuMarkEventHandler : MonoBehaviour {

    [Serializable]
    public struct DictionaryEntry
    {
        public string VuforiaId;
        public GameObject Hologram;
    }

    public List<DictionaryEntry>  Holograms;
    private Dictionary<string, GameObject> hologramDictionary;

    private VuMarkManager vuMarkManager;

	// Use this for initialization
	void Start () {
        hologramDictionary = new Dictionary<string, GameObject>();
        foreach (var entry in Holograms)
        {
            hologramDictionary[entry.VuforiaId] = entry.Hologram;
        }
        vuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        vuMarkManager.RegisterVuMarkDetectedCallback(OnVumarkDetected);
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    void OnDestroy()
    {
        vuMarkManager.UnregisterVuMarkDetectedCallback(OnVumarkDetected);
    }

    void OnVumarkDetected(VuMarkTarget target)
    {
        GameObject hologram;
        if (hologramDictionary.TryGetValue(target.InstanceId.StringValue, out hologram))
        {
            foreach (var vumark in vuMarkManager.GetActiveBehaviours(target))
            {
                hologram.transform.position = vumark.transform.position;
                hologram.transform.rotation = vumark.transform.rotation;
            }
            hologram.SetActive(true);
        }
    }
}
