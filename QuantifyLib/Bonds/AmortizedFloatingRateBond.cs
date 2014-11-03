using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spread.Price.CashFlow;
using Spread.Price.Day;

namespace Spread.Price.Bonds
{
    public class AmortizedFloatingRateBond : Bond 
    {

        #region "Variables"


        protected double BASE_100 = 100;

        protected double _faceValue;
        protected double _rate;
        protected double _effectiveRate;

        protected double _index;


        private DayCounter _dayCounter;
        private List<Coupon> _coupons = new List<Coupon>();

        public List<Coupon> Coupons
        {
            get { return _coupons; }
            set { _coupons = value; }
        }

        private List<AmortizingPayment> _amortizingPayments = new List<AmortizingPayment>();
        public List<AmortizingPayment> AmortizingPayments
        {
            get { return _amortizingPayments; }
            set { _amortizingPayments = value; }
        }


        #endregion

        public AmortizedFloatingRateBond(double faceValue, double rate, DateTime maturity, DayCounter dayCounter, DateTime currentDate)
        {
            this._faceValue = faceValue;
            this._rate = rate;
            this._maturityDate = maturity;
            this._currentDate = currentDate;
            this._dayCounter = dayCounter;
        }

        public AmortizedFloatingRateBond(double faceValue, double rate, DateTime maturity, DayCounter dayCounter)
        {
            this._faceValue = faceValue;
            this._rate = rate;
            this._maturityDate = maturity;
            this._currentDate = DateTime.Today;
            this._dayCounter = dayCounter;
        }

        #region "Private Methods"

        protected override void PerformCalculate()
        {
            _NVP = _CalculateEventsPresentValue();
        }

        private decimal _CalculateEventsPresentValue()
        {
            decimal _amortizadedRate = 0;
            decimal totalPresenteValue = 0;

            var eventDates = (from c in _coupons
                    select c.PaymentDate).Distinct().OrderBy( d => d)  ;

            foreach (DateTime eventDate in eventDates)
            {
                if(_coupons.Exists (c => c.PaymentDate == eventDate ) )
                {
                    var coupon = (from c in _coupons
                                  where c.PaymentDate==eventDate 
                                  select c).Single();
                    decimal couponAmortizadedValue = _CalculateCouponPresentValue(coupon) * (1 - _amortizadedRate);

                    totalPresenteValue += couponAmortizadedValue;

                }

                if (_amortizingPayments.Exists(c => c.PaymentDate == eventDate))
                {
                    var amortization = (from a in _amortizingPayments
                                  where a.PaymentDate == eventDate
                                  select a).Single();

                    decimal amortizationValue = _CalculateAmortizationPresentValue(amortization);

                    totalPresenteValue += amortizationValue;

                    _amortizadedRate += amortization.Rate;

                }
                
            }

            return totalPresenteValue;
        }


        private decimal _CalculateAmortizationPresentValue(AmortizingPayment amortization)
        {
            DateTime bizCouponPaymentDay = BusinessDay.Instance.CurrentOrNextBusinessDay(amortization.PaymentDate);
            DateTime currentBizDay = BusinessDay.Instance.CurrentOrNextBusinessDay(_currentDate.AddDays(1));

            double numberOfPeriods = (double)_dayCounter.YearFraction(currentBizDay, bizCouponPaymentDay);

            double discount = System.Math.Pow(1 + _rate, numberOfPeriods);

            double nominalAtFutureValue = (double)amortization.Nominal * (1 + _index);

            var presentValue = (decimal)((decimal)nominalAtFutureValue * amortization.Rate / (decimal)discount);

            return presentValue;
        }

        private decimal _CalculateCouponPresentValue(Coupon coupon)
        {
            DateTime bizCouponPaymentDay = BusinessDay.Instance.CurrentOrNextBusinessDay(coupon.PaymentDate);
            DateTime currentBizDay = BusinessDay.Instance.CurrentOrNextBusinessDay(_currentDate.AddDays(1));

            double numberOfPeriods = (double)_dayCounter.YearFraction(currentBizDay, bizCouponPaymentDay);

            double discount = System.Math.Pow(1 + _rate, numberOfPeriods);

            double nominalAtFutureValue = (double)coupon.Nominal * (1 + _index);

            var presentValue = (decimal)((decimal)nominalAtFutureValue * coupon.EffectiveRate / (decimal)discount);

            return presentValue;
        }

        #endregion

        #region "Public Methods"

        public override decimal CleanPrice()
        {
            return _NVP;
        }

        public override decimal DirtyPrice()
        {
            return _NVP;
        }

        public override decimal CurrentYield()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
