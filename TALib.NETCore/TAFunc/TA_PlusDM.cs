using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode PlusDM(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = PlusDMLookback(optInTimePeriod);

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

            int today;
            double diffP;
            double prevLow;
            double prevHigh;
            double diffM;
            int outIdx = default;
            if (optInTimePeriod <= 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    double tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    outReal[outIdx++] = diffP > 0.0 && diffP > diffM ? diffP : 0.0;
                }

                outNBElement = outIdx;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            double prevPlusDM = default;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM];
            while (i-- != 0)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }
            }

            outReal[0] = prevPlusDM;
            outIdx = 1;

            while (today < endIdx)
            {
                today++;
                double tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > 0.0 && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }

                outReal[outIdx++] = prevPlusDM;
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode PlusDM(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = PlusDMLookback(optInTimePeriod);

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

            int today;
            decimal diffP;
            decimal prevLow;
            decimal prevHigh;
            decimal diffM;
            int outIdx = default;
            if (optInTimePeriod <= 1)
            {
                outBegIdx = startIdx;
                today = startIdx - 1;
                prevHigh = inHigh[today];
                prevLow = inLow[today];
                while (today < endIdx)
                {
                    today++;
                    decimal tempReal = inHigh[today];
                    diffP = tempReal - prevHigh;
                    prevHigh = tempReal;
                    tempReal = inLow[today];
                    diffM = prevLow - tempReal;
                    prevLow = tempReal;
                    outReal[outIdx++] = diffP > Decimal.Zero && diffP > diffM ? diffP : Decimal.Zero;
                }

                outNBElement = outIdx;
                return RetCode.Success;
            }

            outBegIdx = startIdx;
            decimal prevPlusDM = default;
            today = startIdx - lookbackTotal;
            prevHigh = inHigh[today];
            prevLow = inLow[today];
            int i = optInTimePeriod - 1;
            while (i-- > 0)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM += diffP;
                }
            }

            i = (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM];
            while (i-- != 0)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }
            }

            outReal[0] = prevPlusDM;
            outIdx = 1;

            while (today < endIdx)
            {
                today++;
                decimal tempReal = inHigh[today];
                diffP = tempReal - prevHigh;
                prevHigh = tempReal;
                tempReal = inLow[today];
                diffM = prevLow - tempReal;
                prevLow = tempReal;
                if (diffP > Decimal.Zero && diffP > diffM)
                {
                    prevPlusDM = prevPlusDM - prevPlusDM / optInTimePeriod + diffP;
                }
                else
                {
                    prevPlusDM -= prevPlusDM / optInTimePeriod;
                }

                outReal[outIdx++] = prevPlusDM;
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int PlusDMLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 1 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + (int) Globals.UnstablePeriod[(int) FuncUnstId.PlusDM] - 1 : 1;
        }
    }
}
