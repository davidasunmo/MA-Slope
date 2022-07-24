using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MASlope : Indicator
    {

        [Parameter("Source", DefaultValue = "Close")]
        public DataSeries Source { get; set; }

        [Parameter("Moving Average Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 25, Step = 1)]
        public int MAPeriod { get; set; }

        [Parameter(DefaultValue = 7, Step = 1)]
        public int AngleThreshold { get; set; }

        [Output("Angle", PlotType = PlotType.Histogram, LineColor = "Red")]
        public IndicatorDataSeries Angle { get; set; }

        [Output("Raw Angle", PlotType = PlotType.Histogram, LineColor = "Transparent")]
        public IndicatorDataSeries SignedAngle { get; set; }

        [Output("Angle Threshold Level", LineColor = "Black", LineStyle = LineStyle.DotsRare)]
        public IndicatorDataSeries AngleLevel { get; set; }

        private MovingAverage MAObject { get; set; }
        private IndicatorDataSeries MA
        {
            get { return MAObject.Result; }
        }

        protected override void Initialize()
        {
            MAObject = Indicators.MovingAverage(Source, MAPeriod, MAType);
        }

        public override void Calculate(int index)
        {
            AngleLevel[index] = AngleThreshold;

            if (double.IsNaN(MA[index - 1]))
            {
                Angle[index] = 0;
                return;
            }
            SignedAngle[index] = Math.Atan((MA[index] - MA[index - 1]) / Symbol.PipSize) * 180 / Math.PI;
            Angle[index] = Math.Abs(SignedAngle[index]);

        }
    }
}
