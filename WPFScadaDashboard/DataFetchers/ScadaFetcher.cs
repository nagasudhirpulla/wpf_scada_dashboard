using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;
using InStep.eDNA.EzDNAApiNet;
using System.Threading;

namespace WPFScadaDashboard.DataFetchers
{
    class ScadaFetcher : IFetcherBase
    {
        public const string fetcherType = "ScadaFetcher";
        public string FetcherType_ { get { return fetcherType; } set { } }

        // Implementing the interface
        public IPointResult FetchCurrentPointData(IDataPoint point)
        {
            return FetchCurrentPointData((ScadaDataPoint)point);
        }

        public ScadaPointResult FetchCurrentPointData(ScadaDataPoint point)
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

        // Implementing interface
        public List<IPointResult> FetchHistoricalPointData(IDashboardTimeSeriesPoint dashboardTimeSeriesPoint)
        {
            if (dashboardTimeSeriesPoint.GetType().FullName == typeof(DashboardScadaTimeSeriesPoint).FullName)
            {
                List<ScadaPointResult> scadaPointResults = FetchHistoricalPointData((DashboardScadaTimeSeriesPoint)dashboardTimeSeriesPoint);

                List<IPointResult> pointResults = new List<IPointResult>();

                for (int i = 0; i < scadaPointResults.Count; i++)
                {
                    pointResults.Add(scadaPointResults.ElementAt(i));
                }

                return pointResults;
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
                DateTime startTime = dashboardScadaTimeSeriesPoint.StartTimeAbsolute_;
                DateTime endTime = dashboardScadaTimeSeriesPoint.EndTimeAbsolute_;
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

        public List<ScadaPointResult> FetchHistoricalPointDataTest(DashboardScadaTimeSeriesPoint dashboardScadaTimeSeriesPoint)
        {
            // return random values for testing
            List<ScadaPointResult> historyResults = new List<ScadaPointResult>();
            Random rand = new Random();
            int numPeriods = (int)(dashboardScadaTimeSeriesPoint.GetEndTime() - dashboardScadaTimeSeriesPoint.GetStartTime()).TotalSeconds / dashboardScadaTimeSeriesPoint.FetchPeriodSecs_;
            for (int i = 0; i < numPeriods; i++)
            {
                historyResults.Add(new ScadaPointResult(rand.Next(0, 100), "Good", dashboardScadaTimeSeriesPoint.GetStartTime().AddSeconds(dashboardScadaTimeSeriesPoint.FetchPeriodSecs_ * i)));
            }
            return historyResults;
        }

        public async Task<List<ScadaPointResult>> FetchHistoricalPointDataAsync(DashboardScadaTimeSeriesPoint dashboardScadaTimeSeriesPoint, CancellationToken ct)
        {
            await Task.Delay(3000);
            List<ScadaPointResult> historyResults = new List<ScadaPointResult>();
            if (AppSettingsHelper.GetSetting<bool>("scada_fetch_random", false))
            {
                // If random scada data fetch was selected
                historyResults = FetchHistoricalPointDataTest(dashboardScadaTimeSeriesPoint);
            }
            else
            {
                historyResults = FetchHistoricalPointData(dashboardScadaTimeSeriesPoint);
            }
            return historyResults;
        }
    }
}
