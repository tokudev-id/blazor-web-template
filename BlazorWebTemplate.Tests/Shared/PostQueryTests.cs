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

    [Fact]
    public void NormalizedPageSize_ClampsSmallValues()
    {
        var query = new PostQuery(PageNumber: 1, PageSize: 1);

        Assert.Equal(6, query.NormalizedPageSize);
    }

    [Fact]
    public void NormalizedPageSize_ClampsLargeValues()
    {
        var query = new PostQuery(PageNumber: 1, PageSize: 100);

        Assert.Equal(48, query.NormalizedPageSize);
    }
}
