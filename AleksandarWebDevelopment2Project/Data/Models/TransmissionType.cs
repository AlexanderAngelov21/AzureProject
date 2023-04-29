﻿
namespace AleksandarWebDevelopment2Project.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class TransmissionType
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TransmissionTypeNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}