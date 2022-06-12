using PRO506;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class TaxCalculationUnitTest
    {
        private readonly TaxRates _taxRates = new TaxRates
        {
            UpTo14000 = 0.105,
            Over14000UpTo48000 = 0.175,
            Over48000UpTo70000 = 0.3,
            Over70000UpTo180000 = 0.33,
            Over180000 = 0.39
        };

        //[Fact]
        //public void Income_taxRates_null_Expected_Exception_Test()
        //{
        //    var grossAnnualIncome = 10000;
        //    TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
        //    var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
        //    Assert.Equal(1050, resNetSalary);
        //}

        [Fact]
        public void Income10000_taxExpected_1050_Test()
        {
            var grossAnnualIncome = 10000;
            TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
            var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
            Assert.Equal(1050, resNetSalary);
        }

        [Fact]
        public void Income45000_taxExpected_6895_Test()
        {
            var grossAnnualIncome = 45000;
            TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
            var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
            Assert.Equal(6895, resNetSalary);
        }

        [Fact]
        public void Income_60000_taxExpected_11020_Test()
        {
            var grossAnnualIncome = 60000;
            TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
            var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
            Assert.Equal(11020, resNetSalary);
        }

        [Fact]
        public void Income_100000_taxExpected__Test()
        {
            var grossAnnualIncome = 100000;
            TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
            var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
            Assert.Equal(23920, resNetSalary);
        }

        [Fact]
        public void Income_DoubleMax_taxExpected__Test()
        {
            double grossAnnualIncome = 200000;
            TaxCalculator salaryCalculator = new TaxCalculator(_taxRates);
            var resNetSalary = salaryCalculator.CalculateTax(grossAnnualIncome);
            Assert.Equal(58120, resNetSalary);
        }
    }
}