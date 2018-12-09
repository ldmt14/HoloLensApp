namespace OctoPi
{
    public class PrinterStateFlags
    {
        public bool operational;
        public bool paused;
        public bool printing;
        public bool pausing;
        public bool cancelling;
        public bool sdReady;
        public bool error;
        public bool ready;
        public bool closedOrError;
    }
}