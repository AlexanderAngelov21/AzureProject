﻿

namespace AleksandarWebDevelopment2Project.Models.Posts
{
    using Cars;

    public class PostInListViewModel
    {
        public CarInListViewModel Car { get; init; }

        public string PublishedOn { get; init; }
    }
}