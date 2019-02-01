using System;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace Manager
{
    public class GestureManager : MonoBehaviour
    {
        public static GestureManager Instance { get; private set; }

        // Represents the hologram that is currently being gazed at.
        public GameObject FocusedObject { get; private set; }

        GestureRecognizer recognizer;

        // Use this for initialization
        void Start()
        {
            Instance = this;

            // Set up a GestureRecognizer to detect Select gestures.
            recognizer = new GestureRecognizer();
            recognizer.Tapped += (args) =>
            {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
                {
                    FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
                }
            };
            recognizer.NavigationCompleted += (args) =>
            {
                if (args.normalizedOffset.x < 0 && (-args.normalizedOffset.x) > Math.Abs(args.normalizedOffset.y))
                {
                    if (FocusedObject != null)
                    {
                        FocusedObject.SendMessageUpwards("OnLeftSwipe", SendMessageOptions.DontRequireReceiver);
                    }
                }
                else if (args.normalizedOffset.x > 0 && args.normalizedOffset.x > Math.Abs(args.normalizedOffset.y))
                {
                    if (FocusedObject != null)
                    {
                        FocusedObject.SendMessageUpwards("OnRightSwipe", SendMessageOptions.DontRequireReceiver);
                    }
                }
            };
            recognizer.StartCapturingGestures();
        }

        // Update is called once per frame
        void Update()
        {
            // Figure out which hologram is focused this frame.
            GameObject oldFocusObject = FocusedObject;

            FocusedObject = GazeManager.Instance.FocusedObject;

            // If the focused object changed this frame,
            // start detecting fresh gestures again.
            if (FocusedObject != oldFocusObject)
            {
                recognizer.CancelGestures();
                recognizer.StartCapturingGestures();
            }
        }
    }
}