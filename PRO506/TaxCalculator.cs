using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO506
{
    // Calculates salary, tax, net, kiwi saver
    public struct TaxCalculator
    {
        private readonly TaxRates _taxRates;
        private const int NumberWeeksYearly = 52;
        private const int NumberHoursWekly = 40;
        private const int NumberOfPercents = 100;
        private const int FortnightlyNumberOfWeeks = 2;
        public const int HumberOfHoursForghtnigtlyPay = 80;
        private const int Threashold14k = 14000;
        private const int Threashold48k = 48000;
        private const int ThreasholdBetween14kAnd48k = 34000;
        private const int ThreasholdBetween48kAnd70k = 22000;
        private const int ThreasholdBetween70kAnd180k = 110000;
        private const int Threashold70k = 70000;
        private const int Threashold180k = 180000;

        // "taxRates" table of tax rates for each threashold. not nullable.
        public TaxCalculator(TaxRates taxRates)
        {
            this._taxRates = taxRates;
        }

        public double CalculateNetAnnualSalary(double grossAnnualIncome, double kiwiSaver)
        {
            return grossAnnualIncome - CalculateTax(grossAnnualIncome) - kiwiSaver;
        }

        public double CalculateFortnightlyPay(double income)
        {
            return income / NumberWeeksYearly * FortnightlyNumberOfWeeks;
        }

        public double CalculateKiwiSaver(double grossAnnualIncome, double kiwiSaver)
        {
            return grossAnnualIncome * kiwiSaver / NumberOfPercents;//Calcualte kivi saver
        }

        public double CalculateHourlyRate(double grossAnnualIncome)
        {
            return grossAnnualIncome / NumberWeeksYearly / NumberHoursWekly;//Hourly rate of employee
        }

        // returns tax value based on annual salary, "annualIncome" is annual salary in dollars
        public double CalculateTax(double annualIncome)
        {
            if (annualIncome <= 0) return 0;
            //in this block annual salary less than 14000, for instance 10000
            if (annualIncome <= Threashold14k)
            {
                return annualIncome * _taxRates.UpTo14000;
            }

            // in this block annual salary higher than 14000, but less than 48000 for instance 45000
            if (annualIncome > Threashold14k && annualIncome <= Threashold48k)
            {
                return (annualIncome - Threashold14k) * _taxRates.Over14000UpTo48000 +
                    Threashold14k * _taxRates.UpTo14000;
            }

            // in this block annual salary higher than 48000, but less than 70000 for instance 60000
            if (annualIncome > Threashold48k && annualIncome <= Threashold70k)
            {
                return (annualIncome - Threashold48k) * _taxRates.Over48000UpTo70000 +
                     ThreasholdBetween14kAnd48k * _taxRates.Over14000UpTo48000 +
                     Threashold14k * _taxRates.UpTo14000;
            }

            // in this block annual salary higher than 70000, but less than 180000 for instance 100000
            if (annualIncome > Threashold70k && annualIncome <= Threashold180k)
            {
                return (annualIncome - Threashold70k) * _taxRates.Over70000UpTo180000 +
                     ThreasholdBetween48kAnd70k * _taxRates.Over48000UpTo70000 +
                     ThreasholdBetween14kAnd48k * _taxRates.Over14000UpTo48000 +
                     Threashold14k * _taxRates.UpTo14000;
            }

            //in this block all the rest of the cases, means higher than 180000, for instance 200000 or 10000000000000 (what ever)
            return (annualIncome - Threashold180k) * _taxRates.Over180000 +
                     ThreasholdBetween70kAnd180k * _taxRates.Over70000UpTo180000 +
                     ThreasholdBetween48kAnd70k * _taxRates.Over48000UpTo70000 +
                     ThreasholdBetween14kAnd48k * _taxRates.Over14000UpTo48000 +
                     Threashold14k * _taxRates.UpTo14000;
        }

    }
}
