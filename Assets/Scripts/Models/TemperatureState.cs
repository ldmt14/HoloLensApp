using System.Collections.Generic;

namespace OctoPi
{
    public class TemperatureState
    {
        public TemperatureData tool0;
        public TemperatureData tool1;
        public TemperatureData tool2;
        public TemperatureData bed;
        public List<HistoricTemperatureDatapoint> history;
    }
}