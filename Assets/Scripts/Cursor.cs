using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField]
        private GameObject cursorVisual;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Manager.GazeManager.Instance.FocusedObject != null)
            {
                transform.position = Manager.GazeManager.Instance.HitInfo.point;
                transform.forward = Manager.GazeManager.Instance.HitInfo.normal;
                cursorVisual.SetActive(true);
            }
            else
            {
                cursorVisual.SetActive(false);
            }
        }
    }
}