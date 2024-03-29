﻿
namespace AleksandarWebDevelopment2Project.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class FuelType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(FuelTypeNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}