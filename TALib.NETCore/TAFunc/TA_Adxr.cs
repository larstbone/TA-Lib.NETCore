namespace TALib
{
    public partial class Core
    {
        public static RetCode Adxr(int startIdx, int endIdx, double[] inHigh, double[] inLow, double[] inClose, ref int outBegIdx,
            ref int outNBElement, double[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int adxrLookback = AdxrLookback(optInTimePeriod);
            if (startIdx < adxrLookback)
            {
                startIdx = adxrLookback;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            var adx = new double[endIdx - startIdx + optInTimePeriod];

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, adx,
                optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = default;
            int outIdx = default;
            int nbElement = endIdx - startIdx + 2;
            while (--nbElement != 0)
            {
                outReal[outIdx++] = (adx[i++] + adx[j++]) / 2.0;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode Adxr(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, decimal[] inClose, ref int outBegIdx,
            ref int outNBElement, decimal[] outReal, int optInTimePeriod = 14)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || inClose == null || outReal == null || optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return RetCode.BadParam;
            }

            int adxrLookback = AdxrLookback(optInTimePeriod);
            if (startIdx < adxrLookback)
            {
                startIdx = adxrLookback;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            var adx = new decimal[endIdx - startIdx + optInTimePeriod];

            RetCode retCode = Adx(startIdx - (optInTimePeriod - 1), endIdx, inHigh, inLow, inClose, ref outBegIdx, ref outNBElement, adx,
                optInTimePeriod);
            if (retCode != RetCode.Success)
            {
                return retCode;
            }

            int i = optInTimePeriod - 1;
            int j = default;
            int outIdx = default;
            int nbElement = endIdx - startIdx + 2;
            while (--nbElement != 0)
            {
                outReal[outIdx++] = (adx[i++] + adx[j++]) / 2m;
            }

            outBegIdx = startIdx;
            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int AdxrLookback(int optInTimePeriod = 14)
        {
            if (optInTimePeriod < 2 || optInTimePeriod > 100000)
            {
                return -1;
            }

            return optInTimePeriod > 1 ? optInTimePeriod + AdxLookback(optInTimePeriod) - 1 : 3;
        }
    }
}
