using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace NoTypeResponse.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ExternalServiceController : ControllerBase
{
    private readonly ILogger<ExternalServiceController> _logger;
    private readonly HttpClient _httpClient;


    public ExternalServiceController(ILogger<ExternalServiceController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpGet("posts", Name = "GetPostsFromExternalServer")]
    public async Task<ActionResult> GetPosts()
    {
        var response = await _httpClient.GetAsync("http://localhost:3000/posts");
        var content = await response.Content.ReadAsStringAsync();
        return Ok(content);
    }

    [HttpGet("wrap-posts", Name = "GetWrapPostFromExternalServer")]
    public async Task<ActionResult> GetWrapPost()
    {
        var response = await _httpClient.GetAsync($"http://localhost:3000/posts");
        var content = await response.Content.ReadFromJsonAsync<JsonNode>() ?? new JsonArray();
        return Ok(new WrappedPosts() { Posts = content });
    }

    [HttpGet("get-views", Name = "GetPostsViewsFromExternalServer")]
    public async Task<ActionResult> GetPostsViewsFromExternalServer()
    {
        var response = await _httpClient.GetAsync($"http://localhost:3000/posts");
        var content = await response.Content.ReadFromJsonAsync<JsonNode>() ?? new JsonArray();
        var postAndViews = content.AsArray().Select(post =>
        {
            var uppercaseTitle = post?["title"]?.ToString().ToUpper() ?? "";
            return new PostsViews()
            {
                Title = uppercaseTitle,
                Views = post?["views"]?.GetValue<int>() ?? 0
            };
        }).ToList();
        return Ok(new WrappedPostsViews() { Posts = postAndViews });
    }

    [HttpGet("unest-object", Name = "GetUnestObjectFromExternalServer")]
    public async Task<ActionResult> GetUnestObjectFromExternalServer()
    {
        var response = await _httpClient.GetAsync($"http://localhost:3000/nested");
        var content = await response.Content.ReadFromJsonAsync<JsonNode>() ?? JsonValue.Create("{}");
        var nestedKey = content?["key"] ?? JsonValue.Create("{}");
        return Ok(nestedKey);
    }
}
