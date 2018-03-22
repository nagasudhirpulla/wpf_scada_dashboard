using LiveCharts;
using LiveCharts.Geared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFScadaDashboard.DashboardConfigClasses;
using WPFScadaDashboard.DashboardDataPointClasses;
using WPFScadaDashboard.DataFetchers;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for LinePlotCellUC.xaml
    /// </summary>
    public partial class LinePlotCellUC : UserControl, ICellUC
    {
        public LinePlotCellConfig LinePlotCellConfig_;
        public SeriesCollection SeriesCollection { get; set; }
        public long Step { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public List<DateTime> timeStamps_ { get; set; }
        public double MeasPeriodSecs_ { get; set; } = 1;

        private void DoInitialWireUp()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            timeStamps_ = new List<DateTime> { DateTime.Now };
            //Labels = new string[0];
            Step = 1;

            YFormatter = value => String.Format("{0:0.###}", value);
            XFormatter = delegate (double val)
            {
                if (timeStamps_.Count > 0)
                {
                    DateTime startTimeStamp = timeStamps_.ElementAt(0);
                    DateTime timeStamp;
                    timeStamp = startTimeStamp.AddSeconds(val * MeasPeriodSecs_);
                    return timeStamp.ToString("HH:mm:ss.fff\ndd-MMM-yy");
                }
                return val.ToString();
            };
            MyChart.LegendLocation = LegendLocation.Top;
            DataContext = this;
            FetchAndPlotData();
        }

        // constructor
        public LinePlotCellUC()
        {
            DoInitialWireUp();
        }

        // constructor
        public LinePlotCellUC(LinePlotCellConfig linePlotCellConfig)
        {
            LinePlotCellConfig_ = linePlotCellConfig;
            DoInitialWireUp();
        }

        // Implementing Interface
        public IDashboardCellConfig GetDashboardCellConfig()
        {
            return LinePlotCellConfig_;
        }

        public string CellTitle { get { return LinePlotCellConfig_.Name_; } set { LinePlotCellConfig_.Name_ = value; } }

        public int RowIndex { get { return LinePlotCellConfig_.CellPosition_.RowIndex_; } }

        public int ColumnIndex { get { return LinePlotCellConfig_.CellPosition_.ColIndex_; } }

        public int RowSpan { get { return LinePlotCellConfig_.CellPosition_.RowSpan_; } }

        public int ColumnSpan { get { return LinePlotCellConfig_.CellPosition_.ColSpan_; } }

        public string CellHeight
        {
            get
            {
                if (LinePlotCellConfig_.GetHeightMode() == LinePlotCellConfig.VariableHeightMode)
                {
                    return "";
                }
                return LinePlotCellConfig_.Height_.ToString();
            }
        }

        public string CellWidth
        {
            get
            {
                if (LinePlotCellConfig_.GetWidthMode() == LinePlotCellConfig.VariableWidthMode)
                {
                    return "";
                }
                return LinePlotCellConfig_.Width_.ToString();
            }
        }

        public double CellMinHeight { get { return LinePlotCellConfig_.MinHeight_; } set { LinePlotCellConfig_.MinHeight_ = value; } }

        public double CellMinWidth { get { return LinePlotCellConfig_.MinWidth_; } set { LinePlotCellConfig_.MinWidth_ = value; } }

        public string CellHorizontalAlignment { get { return LinePlotCellConfig_.HorizontalAlignment_; } set { LinePlotCellConfig_.SetHorizontalAlignment(value); } }

        public string CellVerticalAlignment { get { return LinePlotCellConfig_.VerticalAlignment_; } set { LinePlotCellConfig_.SetVerticalAlignment(value); } }

        public void FetchAndPlotData()
        {
            SeriesCollection.Clear();
            // get each point data and bind it to the Series Collection
            // iterate through each timeseries point in the config
            for (int i = 0; i < LinePlotCellConfig_.TimeSeriesPoints_.Count; i++)
            {
                if (i == 0)
                {
                    timeStamps_.Clear();
                }
                IDashboardTimeSeriesPoint pnt = LinePlotCellConfig_.TimeSeriesPoints_.ElementAt(i);
                if (pnt.GetType().FullName == typeof(DashboardScadaTimeSeriesPoint).FullName)
                {
                    // handle if the point is a scada point
                    ScadaFetcher scadaFetcher = new ScadaFetcher();
                    DashboardScadaTimeSeriesPoint scadaTimeseriesPnt = (DashboardScadaTimeSeriesPoint)pnt;
                    List<ScadaPointResult> results = scadaFetcher.FetchHistoricalPointDataTest(scadaTimeseriesPnt);
                    List<double> plotVals = new List<double>();
                    for (int resCounter = 0; resCounter < results.Count; resCounter++)
                    {
                        ScadaPointResult res = results[resCounter];
                        plotVals.Add(res.Val_);
                        if (i == 0)
                        {
                            timeStamps_.Add(res.ResultTime_);
                        }
                    }
                    if (i == 0 && timeStamps_.Count > 1)
                    {
                        MeasPeriodSecs_ = (timeStamps_[1] - timeStamps_[0]).Seconds;
                        Step = (long)MeasPeriodSecs_;
                    }
                    SeriesCollection.Add(new GLineSeries() { Title = scadaTimeseriesPnt.ScadaPoint_.Name_, Values = new GearedValues<double>(plotVals), PointGeometry = null, Fill = Brushes.Transparent, StrokeThickness = 1, LineSmoothness = 0 });
                }
            }

        }


    }
}

/*
 todo handle x and y formatters
 todo keep customizable legend locations by binding
 todo handle zoom, pan, disable Animations, hoverable by binding so that they can be defined in config
 todo use labels for each time series since the the plots may be aligned but the timestamps may be different
     */
