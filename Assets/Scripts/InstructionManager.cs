using UnityEngine;

public class InstructionManager : MonoBehaviour {
    
    public static InstructionObject LastFocusedObject
    {
        get
        {
            return InspectionManager.LastFocusedObject is InstructionObject ? InspectionManager.LastFocusedObject as InstructionObject : null;
        }
        private set
        {
        }
    }
    
    public static void NextStep()
    {
        if (LastFocusedObject != null)
        {
            LastFocusedObject.NextStep();
        }
    }

    public static void PreviousStep()
    {
        if (LastFocusedObject != null)
        {
            LastFocusedObject.PreviousStep();
        }
    }
}
