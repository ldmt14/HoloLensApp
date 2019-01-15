using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GazeManager : MonoBehaviour
    {
        public static GazeManager Instance { get; private set; }

        /// <summary>
        /// This script holds information about the object the Player is currently looking at
        /// </summary>

        // Represents the hologram that is currently being gazed at.
        public GameObject FocusedObject { get; private set; }
        // Represents the raycasst that is currently hitting the focused object.
        public RaycastHit HitInfo { get; private set; }

        // Use this for initialization
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            // Do a raycast into the world based on the user's
            // head position and orientation.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                // If the raycast hit a hologram, use that as the focused object.
                FocusedObject = hitInfo.collider.gameObject;
                HitInfo = hitInfo;
            }
            else
            {
                // If the raycast did not hit a hologram, clear the focused object.
                FocusedObject = null;
            }
        }
    }
}