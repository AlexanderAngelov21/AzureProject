using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AleksandarWebDevelopment2Project.Models.Cars
{
    public class LatestPostsCarViewModel : BaseCarViewModel
    {
        public int Horsepower { get; set; }

        public string FuelType { get; init; }

        public string TransmissionType { get; init; }

        public string CoverImage { get; init; }
    }
}