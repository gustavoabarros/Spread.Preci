using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spread.Price.Day;

namespace Spread.Price.Bonds.Specialized
{
    public class LTN : FixedRateBond
    {
        public LTN(double faceValue, double rate, DateTime maturity):
        base(faceValue, rate, maturity, new Du252 ()){
        }

    }
}
