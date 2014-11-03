using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spread.Price.Day
{
    public class BusinessDay
    {
        private List<DateTime> _bizDays;
        public List<DateTime> BizDays
        {
            get { return _bizDays; }
            set { _bizDays = value; }
        }

        private static BusinessDay _instance = null;
        public static BusinessDay Instance
        {
            get 
            {
                if (_instance==null)
                {
                    _instance = new BusinessDay();
                }

                return _instance;
            }
        }

        public BusinessDay()
        {
            _bizDays = new List<DateTime>();

            try
            {
                using (StreamReader sr = new StreamReader("bizdays.txt"))
                {
                    while(sr.EndOfStream==false)
                    {
                        String line = sr.ReadLine();
                        string[] values = line.Split(',');

                        _bizDays.Add(new DateTime( int.Parse (values[2]), int.Parse (values[1]), int.Parse (values[0])));
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            
        }

        public long Count(DateTime from, DateTime until)
        {
            _Validate();

            long count = 0;
            DateTime auxDate = from;

            while (auxDate <= until )
            {
                if (IsBizDay(auxDate)==true)
                {
                    count++;
                }

                auxDate = auxDate.AddDays(1);
            }

            return count;
        }

        public DateTime CurrentOrNextBusinessDay(DateTime day)
        {
            DateTime biz = day;

            while (!_bizDays.Contains(biz))
            {
                biz = biz.AddDays(1);
            }

            return biz;
        }

        private void _Validate()
        {
            if (_bizDays == null)
            {
                throw new Exception("BusinessDay was not initialized. Please set up the list of business days.");
            }

        }

        private bool IsBizDay(DateTime day)
        {
            bool bizDay;

            if (_bizDays.Contains(day))
            {
                bizDay = true;
            }
            else
            {
                return false;
            }
                        
            return bizDay;

        }
    }
}
