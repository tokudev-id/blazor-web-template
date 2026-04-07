using BlazorWebTemplate.Web.Features.Posts.State;
using PostSortDirection = BlazorWebTemplate.Shared.Common.SortDirection;

namespace BlazorWebTemplate.Tests.Web;

public sealed class PostsPageStateTests
{
    [Fact]
    public void ApplyRouteParameters_NormalizesAndStoresInputs()
    {
        var state = new PostsPageState();

        var query = state.ApplyRouteParameters(2, "ops", "news", "title", "Ascending", 24);

        Assert.Equal("ops", state.QueryInput);
        Assert.Equal("news", state.TagInput);
        Assert.Equal("title", state.SortByInput);
        Assert.Equal(PostSortDirection.Ascending, state.SortDirectionInput);
        Assert.Equal(24, state.PageSizeInput);
        Assert.Equal(2, query.PageNumber);
    }

    [Fact]
    public void BuildApplyLink_UsesCurrentState()
    {
        var state = new PostsPageState
        {
            QueryInput = "ops",
            TagInput = "news",
            SortByInput = "title",
            SortDirectionInput = PostSortDirection.Ascending,
            PageSizeInput = 24
        };

        var link = state.BuildApplyLink();

        Assert.Equal("/posts?page=1&q=ops&tag=news&sort=title&direction=Ascending&size=24", link);
    }
}
