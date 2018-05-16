namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class VariableTime
    {
        public int HoursOffset_ { get; set; }
        public int MinsOffset_ { get; set; }
        public int SecsOffset_ { get; set; }

        public VariableTime(int hoursOffset_, int minsOffset_, int secsOffset_)
        {
            HoursOffset_ = hoursOffset_;
            MinsOffset_ = minsOffset_;
            SecsOffset_ = secsOffset_;
        }

        public VariableTime(VariableTime variableTime)
        {
            HoursOffset_ = variableTime.HoursOffset_;
            MinsOffset_ = variableTime.MinsOffset_;
            SecsOffset_ = variableTime.SecsOffset_;
        }

        public VariableTime()
        {
        }
    }
}
