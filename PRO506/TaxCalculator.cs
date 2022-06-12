using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO506
{
    // Calculates salary, tax, net, kiwi saver
    public class TaxCalculator
    {
        private readonly TaxRates _taxRates;
        private const int NumberWeeksYearly = 52;
        private const int NumberHoursWekly = 40;
        private const int NumberOfPercents = 100;
        private const int FortnightlyNumberOfWeeks = 2;
        public const int HumberOfHoursForghtnigtlyPay = 80;
        // "taxRates" table of tax rates for each threashold. not nullable.
        public TaxCalculator(TaxRates taxRates)
        {
            if (taxRates == null) throw new ArgumentNullException("Cannot be null", "taxRates");
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
            if (annualIncome <= 14000)
            {
                return annualIncome * _taxRates.UpTo14000;
            }
            // in this block annual salary higher than 14000, but less than 48000 for instance 45000
            if (annualIncome > 14000 && annualIncome <= 48000)
            {
                return (annualIncome - 14000) * _taxRates.Over14000UpTo48000 +
                    (14000 * _taxRates.UpTo14000);
            }

            // in this block annual salary higher than 48000, but less than 70000 for instance 60000
            if (annualIncome > 48000 && annualIncome <= 70000)
            {
                return (annualIncome - 48000) * _taxRates.Over48000UpTo70000 +
                     (48000 - 14000) * _taxRates.Over14000UpTo48000 +
                     (14000 * _taxRates.UpTo14000);
            }

            //
            // in this block annual salary higher than 70000, but less than 180000 for instance 100000
            if (annualIncome > 70000 && annualIncome <= 180000)
            {
                return
                     (annualIncome - 70000) * _taxRates.Over70000UpTo180000 +
                     (70000 - 48000) * _taxRates.Over48000UpTo70000 +
                     (48000 - 14000) * _taxRates.Over14000UpTo48000 +
                     (14000 * _taxRates.UpTo14000);
            }

            //in this block all the rest of the cases, means higher than 180000, for instance 200000 or 10000000000000 (what ever)
            return (annualIncome - 180000) * _taxRates.Over180000 +
                     (180000 - 70000) * _taxRates.Over70000UpTo180000 +
                     (70000 - 48000) * _taxRates.Over48000UpTo70000 +
                     (48000 - 14000) * _taxRates.Over14000UpTo48000 +
                     (14000 * _taxRates.UpTo14000);
        }

    }
}
