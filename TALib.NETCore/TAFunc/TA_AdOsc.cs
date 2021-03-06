using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode AdOsc(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, double[] inVolume,
            ref int outBegIdx, ref int outNBElement, double[] outReal, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null || optInFastPeriod < 2 || optInFastPeriod > 100000 ||
                optInSlowPeriod < 2 || optInSlowPeriod > 100000 || outReal == null)
            {
                return RetCode.BadParam;
            }

            int slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

            int lookbackTotal = EmaLookback(slowestPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;

            double ad = default;

            double fastk = 2.0 / (optInFastPeriod + 1);
            double oneMinusFastk = 1.0 - fastk;

            double slowk = 2.0 / (optInSlowPeriod + 1);
            double oneMinusSlowk = 1.0 - slowk;

            CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
            double fastEMA = ad;
            double slowEMA = ad;
            while (today < startIdx)
            {
                CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
            while (today <= startIdx)
            {
                CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

                outReal[outIdx++] = fastEMA - slowEMA;
            }

            outNBElement = outIdx;

            return RetCode.Success;

            void CalculateAd(in double[] high, in double[] low, in double[] close, in double[] volume, ref int t)
            {
                double h = high[t];
                double l = low[t];
                double tmp = h - l;
                double c = close[t];
                if (tmp > 0.0)
                {
                    ad += (c - l - (h - c)) / tmp * volume[t];
                }

                t++;
            }
        }

        public static RetCode AdOsc(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, decimal[] inVolume,
            ref int outBegIdx, ref int outNBElement, decimal[] outReal, int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || inVolume == null || optInFastPeriod < 2 || optInFastPeriod > 100000 ||
                optInSlowPeriod < 2 || optInSlowPeriod > 100000 || outReal == null)
            {
                return RetCode.BadParam;
            }

            int slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

            int lookbackTotal = EmaLookback(slowestPeriod);
            if (startIdx < lookbackTotal)
            {
                startIdx = lookbackTotal;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            int today = startIdx - lookbackTotal;

            decimal ad = default;

            decimal fastk = 2m / (optInFastPeriod + 1);
            decimal oneMinusFastk = Decimal.One - fastk;

            decimal slowk = 2m / (optInSlowPeriod + 1);
            decimal oneMinusSlowk = Decimal.One - slowk;

            CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
            decimal fastEMA = ad;
            decimal slowEMA = ad;
            while (today < startIdx)
            {
                CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;
            }

            int outIdx = default;
            while (today <= startIdx)
            {
                CalculateAd(inHigh, inLow, inClose, inVolume, ref today);
                fastEMA = fastk * ad + oneMinusFastk * fastEMA;
                slowEMA = slowk * ad + oneMinusSlowk * slowEMA;

                outReal[outIdx++] = fastEMA - slowEMA;
            }

            outNBElement = outIdx;

            return RetCode.Success;

            void CalculateAd(in decimal[] high, in decimal[] low, in decimal[] close, in decimal[] volume, ref int t)
            {
                decimal h = high[t];
                decimal l = low[t];
                decimal tmp = h - l;
                decimal c = close[t];
                if (tmp > Decimal.Zero)
                {
                    ad += (c - l - (h - c)) / tmp * volume[t];
                }

                t++;
            }
        }

        public static int AdOscLookback(int optInFastPeriod = 3, int optInSlowPeriod = 10)
        {
            if (optInFastPeriod < 2 || optInFastPeriod > 100000 || optInSlowPeriod < 2 || optInSlowPeriod > 100000)
            {
                return -1;
            }

            var slowestPeriod = optInFastPeriod < optInSlowPeriod ? optInSlowPeriod : optInFastPeriod;

            return EmaLookback(slowestPeriod);
        }
    }
}
