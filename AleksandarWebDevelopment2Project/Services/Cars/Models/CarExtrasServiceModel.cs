using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AleksandarWebDevelopment2Project.Services.Cars.Models
{
    public class CarExtrasServiceModel : BaseCarSpecificationServiceModel
    {
        public int TypeId { get; set; }

        public string TypeName { get; set; }
    }
}