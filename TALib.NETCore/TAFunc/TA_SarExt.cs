using System;

namespace TALib
{
    public partial class Core
    {
        public static RetCode SarExt(int startIdx, int endIdx, double[] inHigh, double[] inLow, ref int outBegIdx, ref int outNBElement,
            double[] outReal, double optInStartValue = 0.0, double optInOffsetOnReverse = 0.0, double optInAccelerationInitLong = 0.02,
            double optInAccelerationLong = 0.02, double optInAccelerationMaxLong = 0.2, double optInAccelerationInitShort = 0.02,
            double optInAccelerationShort = 0.02, double optInAccelerationMaxShort = 0.2)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInOffsetOnReverse < 0.0 || optInAccelerationInitLong < 0.0 ||
                optInAccelerationLong < 0.0 || optInAccelerationMaxLong < 0.0 || optInAccelerationInitShort < 0.0 ||
                optInAccelerationShort < 0.0 || optInAccelerationMaxShort < 0.0)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            double sar;
            double ep;
            bool isLong;

            double afLong = optInAccelerationInitLong;
            double afShort = optInAccelerationInitShort;
            if (afLong > optInAccelerationMaxLong)
            {
                optInAccelerationInitLong = optInAccelerationMaxLong;
                afLong = optInAccelerationInitLong;
            }

            if (optInAccelerationLong > optInAccelerationMaxLong)
            {
                optInAccelerationLong = optInAccelerationMaxLong;
            }

            if (afShort > optInAccelerationMaxShort)
            {
                optInAccelerationInitShort = optInAccelerationMaxShort;
                afShort = optInAccelerationInitShort;
            }

            if (optInAccelerationShort > optInAccelerationMaxShort)
            {
                optInAccelerationShort = optInAccelerationMaxShort;
            }

            if (optInStartValue.Equals(0.0))
            {
                int _ = default;
                var epTemp = new double[1];
                RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref _, ref _, epTemp, 1);
                isLong = epTemp[0] <= 0.0;

                if (retCode != RetCode.Success)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }
            }
            else if (optInStartValue > 0.0)
            {
                isLong = true;
            }
            else
            {
                isLong = false;
            }

            outBegIdx = startIdx;
            int outIdx = default;

            int todayIdx = startIdx;

            double newHigh = inHigh[--todayIdx];
            double newLow = inLow[--todayIdx];
            if (optInStartValue.Equals(0.0))
            {
                if (isLong)
                {
                    ep = inHigh[todayIdx];
                    sar = newLow;
                }
                else
                {
                    ep = inLow[todayIdx];
                    sar = newHigh;
                }
            }
            else if (optInStartValue > 0.0)
            {
                ep = inHigh[todayIdx];
                sar = optInStartValue;
            }
            else
            {
                ep = inLow[todayIdx];
                sar = Math.Abs(optInStartValue);
            }

            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];

            while (todayIdx <= endIdx)
            {
                double prevLow = newLow;
                double prevHigh = newHigh;
                newLow = inLow[todayIdx];
                newHigh = inHigh[todayIdx++];
                if (isLong)
                {
                    if (newLow <= sar)
                    {
                        isLong = false;
                        sar = ep;

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }

                        if (!optInOffsetOnReverse.Equals(0.0))
                        {
                            sar += sar * optInOffsetOnReverse;
                        }

                        outReal[outIdx++] = -sar;

                        afShort = optInAccelerationInitShort;
                        ep = newLow;

                        sar += afShort * (ep - sar);

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }
                    }
                    else
                    {
                        outReal[outIdx++] = sar;
                        if (newHigh > ep)
                        {
                            ep = newHigh;
                            afLong += optInAccelerationLong;
                            if (afLong > optInAccelerationMaxLong)
                            {
                                afLong = optInAccelerationMaxLong;
                            }
                        }

                        sar += afLong * (ep - sar);

                        if (sar > prevLow)
                        {
                            sar = prevLow;
                        }

                        if (sar > newLow)
                        {
                            sar = newLow;
                        }
                    }
                }
                else if (newHigh >= sar)
                {
                    isLong = true;
                    sar = ep;

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }

                    if (!optInOffsetOnReverse.Equals(0.0))
                    {
                        sar -= sar * optInOffsetOnReverse;
                    }

                    outReal[outIdx++] = sar;

                    afLong = optInAccelerationInitLong;
                    ep = newHigh;

                    sar += afLong * (ep - sar);

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
                else
                {
                    outReal[outIdx++] = -sar;
                    if (newLow < ep)
                    {
                        ep = newLow;
                        afShort += optInAccelerationShort;
                        if (afShort > optInAccelerationMaxShort)
                        {
                            afShort = optInAccelerationMaxShort;
                        }
                    }

                    sar += afShort * (ep - sar);

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static RetCode SarExt(int startIdx, int endIdx, decimal[] inHigh, decimal[] inLow, ref int outBegIdx, ref int outNBElement,
            decimal[] outReal, decimal optInStartValue = Decimal.Zero, decimal optInOffsetOnReverse = Decimal.Zero,
            decimal optInAccelerationInitLong = 0.02m, decimal optInAccelerationLong = 0.02m, decimal optInAccelerationMaxLong = 0.2m,
            decimal optInAccelerationInitShort = 0.02m, decimal optInAccelerationShort = 0.02m, decimal optInAccelerationMaxShort = 0.2m)
        {
            if (startIdx < 0 || endIdx < 0 || endIdx < startIdx)
            {
                return RetCode.OutOfRangeStartIndex;
            }

            if (inHigh == null || inLow == null || outReal == null || optInOffsetOnReverse < Decimal.Zero ||
                optInAccelerationInitLong < Decimal.Zero || optInAccelerationLong < Decimal.Zero ||
                optInAccelerationMaxLong < Decimal.Zero || optInAccelerationInitShort < Decimal.Zero ||
                optInAccelerationShort < Decimal.Zero || optInAccelerationMaxShort < Decimal.Zero)
            {
                return RetCode.BadParam;
            }

            if (startIdx < 1)
            {
                startIdx = 1;
            }

            if (startIdx > endIdx)
            {
                outBegIdx = 0;
                outNBElement = 0;
                return RetCode.Success;
            }

            decimal sar;
            decimal ep;
            bool isLong;

            decimal afLong = optInAccelerationInitLong;
            decimal afShort = optInAccelerationInitShort;
            if (afLong > optInAccelerationMaxLong)
            {
                optInAccelerationInitLong = optInAccelerationMaxLong;
                afLong = optInAccelerationInitLong;
            }

            if (optInAccelerationLong > optInAccelerationMaxLong)
            {
                optInAccelerationLong = optInAccelerationMaxLong;
            }

            if (afShort > optInAccelerationMaxShort)
            {
                optInAccelerationInitShort = optInAccelerationMaxShort;
                afShort = optInAccelerationInitShort;
            }

            if (optInAccelerationShort > optInAccelerationMaxShort)
            {
                optInAccelerationShort = optInAccelerationMaxShort;
            }

            if (optInStartValue == Decimal.Zero)
            {
                int _ = default;
                var epTemp = new decimal[1];
                RetCode retCode = MinusDM(startIdx, startIdx, inHigh, inLow, ref _, ref _, epTemp, 1);
                isLong = epTemp[0] <= Decimal.Zero;

                if (retCode != RetCode.Success)
                {
                    outBegIdx = 0;
                    outNBElement = 0;
                    return retCode;
                }
            }
            else if (optInStartValue > Decimal.Zero)
            {
                isLong = true;
            }
            else
            {
                isLong = false;
            }

            outBegIdx = startIdx;
            int outIdx = default;

            int todayIdx = startIdx;

            decimal newHigh = inHigh[--todayIdx];
            decimal newLow = inLow[--todayIdx];
            if (optInStartValue.Equals(0.0))
            {
                if (isLong)
                {
                    ep = inHigh[todayIdx];
                    sar = newLow;
                }
                else
                {
                    ep = inLow[todayIdx];
                    sar = newHigh;
                }
            }
            else if (optInStartValue > Decimal.Zero)
            {
                ep = inHigh[todayIdx];
                sar = optInStartValue;
            }
            else
            {
                ep = inLow[todayIdx];
                sar = Math.Abs(optInStartValue);
            }

            newLow = inLow[todayIdx];
            newHigh = inHigh[todayIdx];

            while (todayIdx <= endIdx)
            {
                decimal prevLow = newLow;
                decimal prevHigh = newHigh;
                newLow = inLow[todayIdx];
                newHigh = inHigh[todayIdx++];
                if (isLong)
                {
                    if (newLow <= sar)
                    {
                        isLong = false;
                        sar = ep;

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }

                        if (!optInOffsetOnReverse.Equals(0.0))
                        {
                            sar += sar * optInOffsetOnReverse;
                        }

                        outReal[outIdx++] = -sar;

                        afShort = optInAccelerationInitShort;
                        ep = newLow;

                        sar += afShort * (ep - sar);

                        if (sar < prevHigh)
                        {
                            sar = prevHigh;
                        }

                        if (sar < newHigh)
                        {
                            sar = newHigh;
                        }
                    }
                    else
                    {
                        outReal[outIdx++] = sar;
                        if (newHigh > ep)
                        {
                            ep = newHigh;
                            afLong += optInAccelerationLong;
                            if (afLong > optInAccelerationMaxLong)
                            {
                                afLong = optInAccelerationMaxLong;
                            }
                        }

                        sar += afLong * (ep - sar);

                        if (sar > prevLow)
                        {
                            sar = prevLow;
                        }

                        if (sar > newLow)
                        {
                            sar = newLow;
                        }
                    }
                }
                else if (newHigh >= sar)
                {
                    isLong = true;
                    sar = ep;

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }

                    if (!optInOffsetOnReverse.Equals(0.0))
                    {
                        sar -= sar * optInOffsetOnReverse;
                    }

                    outReal[outIdx++] = sar;

                    afLong = optInAccelerationInitLong;
                    ep = newHigh;

                    sar += afLong * (ep - sar);

                    if (sar > prevLow)
                    {
                        sar = prevLow;
                    }

                    if (sar > newLow)
                    {
                        sar = newLow;
                    }
                }
                else
                {
                    outReal[outIdx++] = -sar;
                    if (newLow < ep)
                    {
                        ep = newLow;
                        afShort += optInAccelerationShort;
                        if (afShort > optInAccelerationMaxShort)
                        {
                            afShort = optInAccelerationMaxShort;
                        }
                    }

                    sar += afShort * (ep - sar);

                    if (sar < prevHigh)
                    {
                        sar = prevHigh;
                    }

                    if (sar < newHigh)
                    {
                        sar = newHigh;
                    }
                }
            }

            outNBElement = outIdx;

            return RetCode.Success;
        }

        public static int SarExtLookback()
        {
            return 1;
        }
    }
}
