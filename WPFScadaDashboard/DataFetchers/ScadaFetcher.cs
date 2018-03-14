using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DataPointClasses;
using InStep.eDNA.EzDNAApiNet;

namespace WPFScadaDashboard.DataFetchers
{
    class ScadaFetcher : IFetcherBase
    {
        // Implementing the interface
        public PointResult FetchCurrentPointData(DashBoardDataPoint point)
        {
            return FetchCurrentPointData((DashboardScadaPoint)point);
        }

        public ScadaPointResult FetchCurrentPointData(DashboardScadaPoint point)
        {
            // Fetch a realtime value of the point
            DateTime timestamp = DateTime.Now;

            //get Realtime value
            int nret = RealTime.DNAGetRTAll(point.Id_, out double dval, out timestamp, out string status, out string desc, out string units);

            ScadaPointResult scadaPointResult;
            if (nret == 0)
            {
                scadaPointResult = new ScadaPointResult(dval, status, timestamp, units);
                return scadaPointResult;
            }
            return null;
        }

        // Implementing the interface
        public List<PointResult> FetchHistoricalPointData(IDashboardTimeSeriesPoint dashboardTimeSeriesPoint)
        {
            if (dashboardTimeSeriesPoint.GetType().Equals(typeof(DashboardScadaTimeSeriesPoint)))
            {
                // Check if the point is of type DashboardScadaTimeSeriesPoint

                // Now get the data
                List<ScadaPointResult> ScadaResults = FetchHistoricalPointData((DashboardScadaTimeSeriesPoint)dashboardTimeSeriesPoint);

                // Convert the data into desired format
                List<PointResult> results = new List<PointResult>();
                for (int i = 0; i < ScadaResults.Count; i++)
                {
                    ScadaPointResult res = ScadaResults[i];
                    results.Add(new PointResult(res.Val_));
                }
            }
            return null;
        }

        public List<ScadaPointResult> FetchHistoricalPointData(DashboardScadaTimeSeriesPoint dashboardScadaTimeSeriesPoint)
        {
            try
            {
                string pnt = dashboardScadaTimeSeriesPoint.ScadaPoint_.Id_;
                string type = dashboardScadaTimeSeriesPoint.HistoryFetchStrategy_;
                int nret = 0;
                uint s = 0;
                double dval = 0;
                DateTime timestamp = DateTime.Now;
                DateTime startTime = dashboardScadaTimeSeriesPoint.StartTime_;
                DateTime endTime = dashboardScadaTimeSeriesPoint.EndTime_;
                string status = "";
                TimeSpan period = TimeSpan.FromSeconds(dashboardScadaTimeSeriesPoint.FetchPeriodSecs_);
                //history request initiation
                if (type == "raw")
                { nret = History.DnaGetHistRaw(pnt, startTime, endTime, out s); }
                else if (type == "snap")
                { nret = History.DnaGetHistSnap(pnt, startTime, endTime, period, out s); }
                else if (type == "average")
                { nret = History.DnaGetHistAvg(pnt, startTime, endTime, period, out s); }
                else if (type == "min")
                { nret = History.DnaGetHistMin(pnt, startTime, endTime, period, out s); }
                else if (type == "max")
                { nret = History.DnaGetHistMax(pnt, startTime, endTime, period, out s); }

                // Get history values
                List<ScadaPointResult> historyResults = new List<ScadaPointResult>();
                while (nret == 0)
                {
                    nret = History.DnaGetNextHist(s, out dval, out timestamp, out status);
                    if (status != null)
                    {
                        historyResults.Add(new ScadaPointResult(dval, status, timestamp));
                    }
                }
                return historyResults;
            }
            catch (Exception e)
            {
                // Todo send this to console printing of the dashboard
                Console.WriteLine($"Error while fetching history data of point {dashboardScadaTimeSeriesPoint.ScadaPoint_.Id_}");
                Console.WriteLine($"The exception is {e}");
            }
            return null;
        }
    }
}
