using System;

public class ChartTimePoint
{
    public double Val_ { get; set; }
    public DateTime Time_ { get; set; }

    public ChartTimePoint(double val_, DateTime time_)
    {
        Val_ = val_;
        Time_ = time_;
    }
}