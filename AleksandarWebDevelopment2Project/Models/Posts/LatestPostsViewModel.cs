

namespace AleksandarWebDevelopment2Project.Models.Posts
{
    using System.Collections.Generic;

    public class LatestPostsViewModel
    {
        public IEnumerable<PostInLatestListViewModel> LatestPosts { get; init; }
    }
}