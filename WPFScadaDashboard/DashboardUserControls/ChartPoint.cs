using System;

public class ChartPoint{
    public double Val_ { get; set; }
    public DateTime Time_ { get; set; }

    public ChartPoint(double val_, DateTime time_)
    {
        Val_ = val_;
        Time_ = time_;
    }
}
/*
 todo handle x and y formatters
 todo keep customizable legend locations by binding
 todo handle zoom, pan, disable Animations, hoverable by binding so that they can be defined in config
 todo use labels for each time series since the the plots may be aligned but the timestamps may be different
     */
