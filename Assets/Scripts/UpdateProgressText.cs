using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressText : MonoBehaviour
{
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private float maxValue;

    private void Awake()
    {
        progressText = progressText ?? gameObject.GetComponent<Text>();
    }

    public void UpdateProgress(float value)
    {
        if (progressText != null)
        {
            progressText.text = "Progress: " + (int)(100 * value / maxValue) + "%";
        }
    }
}
