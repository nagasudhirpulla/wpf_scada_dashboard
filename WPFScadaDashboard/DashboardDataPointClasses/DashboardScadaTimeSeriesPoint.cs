﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class DashboardScadaTimeSeriesPoint : IDashboardTimeSeriesPoint
    {
        public const string timeSeriesType = "ScadaTimeSeriesPoint";
        private const string AbsoluteMode = "absolute";
        private const string VariableMode = "variable";

        public string TimeSeriesType_ { get { return timeSeriesType; } set { } }
        public DateTime StartTimeAbsolute_ { get; set; }
        public DateTime EndTimeAbsolute_ { get; set; }
        public string StartTimeMode_ { get; set; } = AbsoluteMode;
        public string EndTimeMode_ { get; set; } = AbsoluteMode;
        public VariableTime StartTimeVariable_ { get; set; }
        public VariableTime EndTimeVariable_ { get; set; }

        // The Scada Point of the TimeSeries Data Configuration
        public ScadaDataPoint ScadaPoint_ { get; set; }

        // History fetch Startegy
        public string HistoryFetchStrategy_ { get; set; } = "snap";

        // Periodicity of history fetch
        public int FetchPeriodSecs_ { get; set; } = 60;

        public DashboardScadaTimeSeriesPoint(ScadaDataPoint ScadaPoint, DateTime StartTime, DateTime EndTime)
        {
            StartTimeAbsolute_ = StartTime;
            EndTimeAbsolute_ = EndTime;
            ScadaPoint_ = ScadaPoint;
        }

        // created just for the sake of the deserialization
        public DashboardScadaTimeSeriesPoint()
        {
            StartTimeAbsolute_ = DateTime.Now;
            EndTimeAbsolute_ = DateTime.Now;
            ScadaPoint_ = new ScadaDataPoint(null);
        }

        public IDataPoint GetDataPoint()
        {
            return ScadaPoint_;
        }

        public void SetFetchPeriodMins(int mins)
        {
            FetchPeriodSecs_ = mins * 60;
        }

        public void SetFetchPeriodHours(int hours)
        {
            FetchPeriodSecs_ = hours * 60 * 60;
        }

        public void SetFetchPeriodDays(int days)
        {
            FetchPeriodSecs_ = days * 24 * 60 * 60;
        }

        public DateTime GetStartTime()
        {
            if (StartTimeMode_ == AbsoluteMode)
            {
                return StartTimeAbsolute_;
            }
            else
            {
                return DateTime.Now.AddHours(StartTimeVariable_.HoursOffset_).AddMinutes(StartTimeVariable_.MinsOffset_).AddSeconds(StartTimeVariable_.SecsOffset_);
            }
        }

        public DateTime GetEndTime()
        {
            if (EndTimeMode_ == AbsoluteMode)
            {
                return EndTimeAbsolute_;
            }
            else
            {
                return DateTime.Now.AddHours(EndTimeVariable_.HoursOffset_).AddMinutes(EndTimeVariable_.MinsOffset_).AddSeconds(EndTimeVariable_.SecsOffset_);
            }
        }
    }
}
