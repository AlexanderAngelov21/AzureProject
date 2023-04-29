

namespace AleksandarWebDevelopment2Project.Models.Posts
{
    using Cars;

    public class PostInLatestListViewModel
    {
        public LatestPostsCarViewModel Car { get; init; }

        public string PublishedOn { get; init; }
    }
}