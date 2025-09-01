using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditStatistics
{
    internal class SmallSampleComparator    {

        public struct SampleSummary
        {
            public int N;
            public double Mean;
            public double SD;
            public double SEM;
            public double TCritical95;        // t critical for chosen conf (here 95%)
            public (double Lo, double Hi) CI; // two-sided CI for chosen conf
        }

        public struct CompareResult
        {
            public SampleSummary A;
            public SampleSummary B;
            public string MorePrecise; // "A", "B", or "Tie" or "Invalid"
        }

        // Main compare function with configurable confidence (e.g., 0.95)
        public static CompareResult CompareSamples(
            int nA, int meanA, double sdA,
            int nB, int meanB, double sdB,
            double confidence = 0.95)
        {
            var A = Summarize(nA, meanA, sdA, confidence);
            var B = Summarize(nB, meanB, sdB, confidence);

            string winner;
            if (double.IsNaN(A.SEM) || double.IsNaN(B.SEM)) winner = "Invalid";
            else if (Math.Abs(A.SEM - B.SEM) < 1e-12) winner = "Tie";
            else winner = (A.SEM < B.SEM) ? "A" : "B";

            return new CompareResult { A = A, B = B, MorePrecise = winner };
        }

        private static SampleSummary Summarize(int n, int mean, double sd, double confidence)
        {
            if (n <= 0 || sd < 0 || double.IsNaN(mean) || double.IsNaN(sd))
            {
                return new SampleSummary
                {
                    N = n,
                    Mean = mean,
                    SD = sd,
                    SEM = double.NaN,
                    TCritical95 = double.NaN,
                    CI = (double.NaN, double.NaN)
                };
            }

            double sem = sd / Math.Sqrt(n);
            int df = Math.Max(1, n - 1);

            // Try to get t-critical properly via MathNet if available; else fallback to z
            double alpha = 1.0 - confidence;
            double tCrit;
            try
            {
                // Uncomment the following line if MathNet.Numerics is referenced:
                // tCrit = MathNet.Numerics.Distributions.StudentT.InvCDF(0.0, 1.0, df, 1.0 - alpha / 2.0);

                // If you DO NOT have MathNet, comment out above and force fallback:
                throw new InvalidOperationException("MathNet not available in this runtime; using z-approx fallback.");
            }
            catch
            {
                // FALLBACK (use z approx). Suitable only for moderate/large n (n >= ~30).
                // Warn: for small n, this underestimates the CI width.
                double z = NormalZForConfidence(confidence); // e.g. 1.96 for 95%
                tCrit = z;
            }

            var ci = (mean - tCrit * sem, mean + tCrit * sem);

            return new SampleSummary
            {
                N = n,
                Mean = mean,
                SD = sd,
                SEM = sem,
                TCritical95 = tCrit,
                CI = ci
            };
        }

        // Small helper: get z for common confidences (two-sided)
        private static double NormalZForConfidence(double confidence)
        {
            // Common values; add more if you want
            if (Math.Abs(confidence - 0.68) < 1e-9) return 1.0;
            if (Math.Abs(confidence - 0.90) < 1e-9) return 1.6448536269514722;
            if (Math.Abs(confidence - 0.95) < 1e-9) return 1.959963984540054;
            if (Math.Abs(confidence - 0.99) < 1e-9) return 2.5758293035489004;
            // Default fallback: 1.96 (approx 95%)
            return 1.959963984540054;
        }

        /// <summary>
        /// //////////////////////////////////////////////////combine two samples
        /// </summary>

        public struct CombinedSample
        {
            public int N;
            public double Mean;
            public double SD;    // sample standard deviation (unbiased)
            public double Variance;
        }

        // Combine two samples given n, mean, sampleSD for each
        public static CombinedSample Combine(
            int n1, double mean1, double sd1,
            int n2, double mean2, double sd2)
        {
            if (n1 <= 0 || n2 <= 0) throw new ArgumentException("Sample sizes must be > 0.");
            if (sd1 < 0 || sd2 < 0) throw new ArgumentException("Standard deviations must be >= 0.");

            int N = n1 + n2;
            double M = (n1 * mean1 + n2 * mean2) / (double)N;

            // within-sample sums of squared deviations
            double SS1 = (n1 - 1) * sd1 * sd1;
            double SS2 = (n2 - 1) * sd2 * sd2;

            // between-sample correction (difference of means)
            double B = n1 * (mean1 - M) * (mean1 - M) + n2 * (mean2 - M) * (mean2 - M);

            double SS_total = SS1 + SS2 + B;

            double variance = SS_total / (N - 1); // unbiased sample variance
            double sdCombined = Math.Sqrt(variance);

            return new CombinedSample
            {
                N = N,
                Mean = M,
                Variance = variance,
                SD = sdCombined
            };
        }

    }
}
