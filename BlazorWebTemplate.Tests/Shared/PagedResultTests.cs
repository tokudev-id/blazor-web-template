using BlazorWebTemplate.Shared.Common.Responses;

namespace BlazorWebTemplate.Tests.Shared;

public sealed class PagedResultTests
{
    [Theory]
    [InlineData(0, 20, 0)]
    [InlineData(1, 20, 1)]
    [InlineData(20, 20, 1)]
    [InlineData(21, 20, 2)]
    [InlineData(100, 20, 5)]
    [InlineData(101, 20, 6)]
    public void TotalPages_ComputesCorrectly(int totalCount, int pageSize, int expected)
    {
        var result = new PagedResult<string>([], 1, pageSize, totalCount);
        Assert.Equal(expected, result.TotalPages);
    }

    [Fact]
    public void TotalPages_IsZeroWhenPageSizeIsZero()
    {
        var result = new PagedResult<string>([], 1, 0, 100);
        Assert.Equal(0, result.TotalPages);
    }

    [Theory]
    [InlineData(1, 3, true)]
    [InlineData(2, 3, true)]
    [InlineData(3, 3, false)]
    public void HasNextPage_ReflectsPagePosition(int pageNumber, int totalPages, bool expected)
    {
        var pageSize = 10;
        var totalCount = totalPages * pageSize;
        var result = new PagedResult<string>([], pageNumber, pageSize, totalCount);
        Assert.Equal(expected, result.HasNextPage);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    public void HasPreviousPage_ReflectsPagePosition(int pageNumber, bool expected)
    {
        var result = new PagedResult<string>([], pageNumber, 10, 100);
        Assert.Equal(expected, result.HasPreviousPage);
    }
}
