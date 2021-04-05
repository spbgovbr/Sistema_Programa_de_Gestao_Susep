using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.Helpers
{
    public static class WorkingDays
    {
        public static int DiffDays(DateTime from, DateTime to, List<DateTime> feriados, Boolean ignoreFromAndTo = true)
        {
            return Days(from, to, feriados, ignoreFromAndTo).Count();
        }

        public static decimal DiffTime(DateTime from, DateTime to, List<DateTime> feriados, int workingHours)
        {
            if (to.Date == from.Date)
            {
                var sameDayTotalHours = Decimal.Parse(to.Subtract(from).TotalHours.ToString());
                return (sameDayTotalHours > workingHours ? workingHours : sameDayTotalHours < 1 ? 1 : sameDayTotalHours);
            }

            //Calcula as horas de dias completos
            var diffDays = DiffDays(from, to, feriados, true);
            double totalHours = diffDays * workingHours;

            //Calcula as horas do primeiro dia
            //Considera 18 horas como fim do expediente                  
            if (from.Hour >= 18)
            {//Se tiver começado após as 18, considera que somente trabalhou 1 hora naquele dia
                totalHours += 1;
            }
            else
            {
                var firstDayHours = new DateTime(from.Year, from.Month, from.Day, 18, 0, 0).Subtract(from);
                totalHours += (firstDayHours.TotalHours > workingHours ? workingHours : firstDayHours.TotalHours);
            }

            //Calcula as horas do último dia
            //Considera 8 horas como início do expediente                  
            if (to.Hour <= 8)
            {//Se tiver começado antes das 8h, considera que trabalhou 1 hora naquele dia
                totalHours += 1;
            }
            else
            {
                //Calcula as horas do último dia
                var lastDayHours = to.Subtract(new DateTime(to.Year, to.Month, to.Day, 8, 0, 0));
                totalHours += (lastDayHours.TotalHours > workingHours ? workingHours : lastDayHours.TotalHours);
            }

            return Decimal.Parse(totalHours.ToString());
        }

        public static IEnumerable<DateTime> Days(DateTime from, DateTime to, List<DateTime> feriados, Boolean ignoreFromAndTo = true)
        {
            var dates = new List<DateTime>();
            if (to.Date < from.Date) to = from;
            if (ignoreFromAndTo)
            {
                from = from.AddDays(1);
                to = to.AddDays(-1);
            }
            while (from.Date <= to.Date)
            {                
                if (from.DayOfWeek != DayOfWeek.Saturday &&
                    from.DayOfWeek != DayOfWeek.Sunday &&
                    (feriados == null || 
                     (feriados != null && !feriados.Any(f => f.Date == from.Date))))
                {
                    dates.Add(from.Date);
                }

                from = from.AddDays(1);
            }
            return dates;
        }
    }
}
