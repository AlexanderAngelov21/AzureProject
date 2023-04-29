using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AleksandarWebDevelopment2Project.Models.Cars
{
    public class BaseCarViewModel
    {
        public int Id { get; init; }

        public string Make { get; init; }

        public string Model { get; init; }

        public int Year { get; init; }

        public decimal Price { get; init; }
    }
}