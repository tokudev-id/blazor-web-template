using BlazorWebTemplate.Shared.Posts;

namespace BlazorWebTemplate.Tests.Shared;

public sealed class PostQueryTests
{
    [Fact]
    public void Skip_IsZeroForFirstPage()
    {
        var query = new PostQuery(PageNumber: 1, PageSize: 12);

        Assert.Equal(0, query.Skip);
    }

    [Fact]
    public void Skip_UsesPageOffsetForLaterPages()
    {
        var query = new PostQuery(PageNumber: 3, PageSize: 9);

        Assert.Equal(18, query.Skip);
    }
}
