using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AleksandarWebDevelopment2Project.Models.Posts.Contracts
{
    public interface ISortableModel
    {
        PostsSorting Sorting { get; set; }
    }
}