namespace Recipit.Constants.Followers
{
    public class RecipitResult
    {
        public string Value { get; set; } = default!;
    }

    public static class Follower
    {
        public const string Created = nameof(Follower) + " created: {Entity}";
        public const string Deleted = nameof(Follower) + " deleted: {Entity}";
        public const string Edited = nameof(Follower) + " edited: {Entity}";
    }

    public static class Product
    {
        public const string Created = nameof(Product) + " created: {Entity}";
        public const string Deleted = nameof(Product) + " deleted: {Entity}";
        public const string Edited = nameof(Product) + " edited: {Entity}";
    }

    public static class Comment
    {
        public const string Created = nameof(Comment) + " created: {Entity}";
        public const string Deleted = nameof(Comment) + " deleted: {Entity}";
    }

    public static class Recipie
    {
        public const string Created = nameof(Recipie) + " created: {Entity}";
        public const string Deleted = nameof(Recipie) + " deleted: {Entity}";
        public const string Edited = nameof(Recipie) + " edited: {Entity}";
    }

}
