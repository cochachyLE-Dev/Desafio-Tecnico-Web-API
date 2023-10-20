using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Models.Order.ViewModels
{
    public class CreateViewModel
    {
        private const string dateFormat = "yyyy-MM-dd";
        public (string minDate, string maxDate) DateRange { get; set; }
        public string[] DisableDateArray { get; set; }
        public SelectList Clients { get; set; }
        public OrderModel Order { get; set; }
        public IEnumerable<OrderDetailModel> OrderDetail { get; set; }

        public async Task<(string, string)> GetDateRangeDefault() {
            return await Task.Run(() => 
            {
                DateTime minDate = DateTime.Now.AddDays(1), maxDate = DateTime.Now.AddDays(8);
                if (minDate.DayOfWeek == DayOfWeek.Sunday || minDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    minDate = minDate.AddDays(1);
                    maxDate = maxDate.AddDays(1);
                }
                if (maxDate.DayOfWeek == DayOfWeek.Sunday || maxDate.DayOfWeek == DayOfWeek.Saturday)
                    maxDate = maxDate.AddDays(1);

                return (minDate.ToString(dateFormat), maxDate.ToString(dateFormat));
            });
        }
        public async Task<string[]> GetDisabledDateArrayDefault(DateTime[] otherDates)
        {
            return await Task.Run(() => {

                DateTime minDate = DateTime.Now.AddDays(1);
                List<string> dates = new List<string>();

                foreach (int day in Enumerable.Range(1, 9))
                {
                    DateTime date = DateTime.Now.AddDays(day);
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        dates.Add(date.ToString(dateFormat));
                }

                if (otherDates.Any())
                    dates.AddRange(otherDates.Select(i => i.ToString(dateFormat)));

                return dates.Distinct().ToArray();
            });
        }        
    }
    public class CreateDetailViewModel
    {
        public SelectList Products { get; set; }
        public OrderDetailModel Item { get; set; }
    }
}