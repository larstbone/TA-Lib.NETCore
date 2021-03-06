namespace TALib
{
    public partial class Core
    {
        public static RetCode Mavp(int startIdx, int endIdx, double[] inReal, double[] inPeriods, MAType optInMAType,
            ref int outBegIdx, ref int outNBElement, double[] outReal, int optInMinPeriod = 2, int optInMaxPeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inPeriods == null || outReal == null || optInMinPeriod < 2 || optInMinPeriod > 100000 ||
                optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MaLookback(optInMAType, optInMaxPeriod);
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

            var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
            if (tempInt > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outputSize = endIdx - tempInt + 1;
            var localOutputArray = new double[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (var i = 0; i < outputSize; i++)
            {
                tempInt = (int) inPeriods[startIdx + i];
                if (tempInt < optInMinPeriod)
                {
                    tempInt = optInMinPeriod;
                }
                else if (tempInt > optInMaxPeriod)
                {
                    tempInt = optInMaxPeriod;
                }

                localPeriodArray[i] = tempInt;
            }

            for (var i = 0; i < outputSize; i++)
            {
                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    int localNbElement = default;
                    int localBegIdx = default;
                    RetCode retCode = Ma(startIdx, endIdx, inReal, optInMAType, ref localBegIdx, ref localNbElement,
                        localOutputArray, curPeriod);
                    if (retCode != RetCode.Success)
                    {
                        outBegIdx = 0;
                        outNBElement = 0;
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (var j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }
            }

            outBegIdx = startIdx;
            outNBElement = outputSize;

            return RetCode.Success;
        }

        public static RetCode Mavp(int startIdx, int endIdx, decimal[] inReal, decimal[] inPeriods,
            MAType optInMAType, ref int outBegIdx, ref int outNBElement, decimal[] outReal, int optInMinPeriod = 2, int optInMaxPeriod = 30)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inReal == null || inPeriods == null || outReal == null || optInMinPeriod < 2 || optInMinPeriod > 100000 ||
                optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int lookbackTotal = MaLookback(optInMAType, optInMaxPeriod);
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

            var tempInt = lookbackTotal > startIdx ? lookbackTotal : startIdx;
            if (tempInt > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            int outputSize = endIdx - tempInt + 1;
            var localOutputArray = new decimal[outputSize];
            int[] localPeriodArray = new int[outputSize];
            for (var i = 0; i < outputSize; i++)
            {
                tempInt = (int) inPeriods[startIdx + i];
                if (tempInt < optInMinPeriod)
                {
                    tempInt = optInMinPeriod;
                }
                else if (tempInt > optInMaxPeriod)
                {
                    tempInt = optInMaxPeriod;
                }

                localPeriodArray[i] = tempInt;
            }

            for (var i = 0; i < outputSize; i++)
            {
                int curPeriod = localPeriodArray[i];
                if (curPeriod != 0)
                {
                    int localNbElement = default;
                    int localBegIdx = default;
                    RetCode retCode = Ma(startIdx, endIdx, inReal, optInMAType, ref localBegIdx, ref localNbElement,
                        localOutputArray, curPeriod);
                    if (retCode != RetCode.Success)
                    {
                        outBegIdx = 0;
                        outNBElement = 0;
                        return retCode;
                    }

                    outReal[i] = localOutputArray[i];
                    for (var j = i + 1; j < outputSize; j++)
                    {
                        if (localPeriodArray[j] == curPeriod)
                        {
                            localPeriodArray[j] = 0;
                            outReal[j] = localOutputArray[j];
                        }
                    }
                }
            }

            outBegIdx = startIdx;
            outNBElement = outputSize;

            return RetCode.Success;
        }

        public static int MavpLookback(MAType optInMAType, int optInMaxPeriod = 30)
        {
            if (optInMaxPeriod < 2 || optInMaxPeriod > 100000)
            {
                return -1;
            }

            return MaLookback(optInMAType, optInMaxPeriod);
        }
    }
}
