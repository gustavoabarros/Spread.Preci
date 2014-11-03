using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spread.Price.Bonds;
using Spread.Price.CashFlow;
using Spread.Price.Day;
using Spread.Quantify;

namespace Test.QuantifyLib
{
    [TestClass]
    public class TestAmortizedBond
    {
        [TestMethod]
        public void TestAmortizedBond_NPV_RDVT1107102014()
        {
            double faceValue = 1000;
            double rate = 0.1;
            decimal couponRate = (decimal)0.8;

            DateTime currentDay = new DateTime(2014, 9, 15);
            DateTime maturity = new DateTime(2017, 1, 1);

            decimal expectedValue = (decimal)1039.318255;
            decimal errorRange = (decimal)0.001;

            AmortizedFloatingRateBond bond = new AmortizedFloatingRateBond(faceValue, rate, maturity, new Du252(), currentDay);

            Coupon c1 = new Coupon(1000, couponRate, new DateTime(2014, 12, 15), new Du252(), new AccrualDateGroup(currentDay, currentDay));
            Coupon c2 = new Coupon(1000, couponRate, new DateTime(2015, 6, 15), new Du252(), new AccrualDateGroup(currentDay, currentDay));


            decimal diference = Math.Abs(bond.NPV() - expectedValue);
            Assert.IsTrue(diference < errorRange);

        }
    }
}
