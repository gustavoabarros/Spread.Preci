using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spread.Price.CashFlow
{
    public class AmortizingPayment : CashFlow
    {

        #region "Constant"

        //Truncada na sexta decimal, segundo regra da NTN-B
        private int DECIMAL_ROUNDING = 8;

        #endregion


        private decimal _nominal;
        private decimal _rate;

        #region "Properties"

        public decimal Nominal
        {
            get { return _nominal; }
        }

        public decimal Rate
        {
            get { return _rate; }
        }
                
        #endregion

        public override decimal Amount()
        {
            return this.Nominal * this.Rate;
        }

        public AmortizingPayment(decimal nominal, decimal rate, DateTime paymentDate)
            : base(paymentDate, nominal)
        {
            _rate = rate;
            _nominal = nominal;
        }


    }
}
