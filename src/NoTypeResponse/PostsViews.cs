namespace NoTypeResponse
{
    public class PostsViews
    {
        public required string Title { get; set; }
        public int Views { get; set; }
    }

    public class WrappedPostsViews
    {
        public required List<PostsViews> Posts { get; set; }
    }
}