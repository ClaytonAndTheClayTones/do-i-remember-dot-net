using System.Collections.Generic;
using WebApi.Adapters.Common;
using WebApi.Helpers;
using WebApi.Models.Common;

namespace Helpers;

[TestClass]
public class PagingAdapterConvertFromPagingRequestInfoToPagingInfo
{
    [TestMethod]
    public void ConvertsAsExpected()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = 123,
            PageLength = 111,
            IsDescending = true,
            SortBy = "sortColumn"
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 123,
            PageLength = 111,
            IsDescending = true,
            SortBy = "sortColumn"
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }

    [TestMethod]
    public void RespectsDefaultMaxValue()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = 123,
            PageLength = 250,
            IsDescending = true,
            SortBy = "sortColumn"
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 123,
            PageLength = 200,
            IsDescending = true,
            SortBy = "sortColumn"
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }

    [TestMethod]
    public void RespectsMinValues()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = -1,
            PageLength = 0,
            IsDescending = null,
            SortBy = null
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 1,
            PageLength = 1,
            IsDescending = false,
            SortBy = null
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }

    [TestMethod]
    public void RespectsDefaultDefaults()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = null,
            PageLength = null,
            IsDescending = null,
            SortBy = null
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 1,
            PageLength = 25,
            IsDescending = false,
            SortBy = null
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }

    [TestMethod]
    public void RespectsDefaultOverride()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = null,
            PageLength = null,
            IsDescending = null,
            SortBy = null
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 1,
            PageLength = 123,
            IsDescending = false,
            SortBy = null
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo, 123);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }



    [TestMethod]
    public void RespectsMaxOverride()
    {
        // Arrange
        PagingRequestInfo requestInfo = new PagingRequestInfo()
        {
            Page = null,
            PageLength = 555,
            IsDescending = null,
            SortBy = null
        };

        PagingInfo expectedPagingInfo = new PagingInfo()
        {
            Page = 1,
            PageLength = 321,
            IsDescending = false,
            SortBy = null
        };

        PagingAdapter pagingAdapter = new PagingAdapter();

        // Act

        PagingInfo pagingInfo = pagingAdapter.convertFromPagingRequestInfoToPagingInfo(requestInfo, 123, 321);

        // Assert

        Assert.AreEqual(expectedPagingInfo.Page, pagingInfo.Page);
        Assert.AreEqual(expectedPagingInfo.PageLength, pagingInfo.PageLength);
        Assert.AreEqual(expectedPagingInfo.SortBy, pagingInfo.SortBy);
        Assert.AreEqual(expectedPagingInfo.IsDescending, pagingInfo.IsDescending);
    }
}
