namespace OctoPi
{
    public class HistoricTemperatureDatapoint
    {
        public long time;
        public TemperatureData tool0;
        public TemperatureData tool1;
        public TemperatureData tool2;
        public TemperatureData bed;
    }
}