using System.Text.Json.Nodes;

namespace NoTypeResponse;

public class WrappedPosts
{
    public required JsonNode Posts { get; set; }
}