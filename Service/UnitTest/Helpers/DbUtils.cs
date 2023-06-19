namespace Helpers;

using AutoMapper.Execution;
using System.Xml.Linq;
using Dapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using WebApi.Helpers;
using WebApi.Models.Common;

public class TestType
{
    public string? key1 { get; set; }
    public int? key2 { get; set; }
    public string? key3 { get; set; }
    public bool? key4 { get; set; }
}

[TestClass]
public class BuildUpdateQuery
{
    [TestMethod]
    public void Generates_Valid_Update_Query_And_Params()
    {
        // Arrange

        TestType testObject = new TestType()
        {
            key1 = "value1",
            key2 = 123,
            key3 = null,
            key4 = true
        };

        List<string> expectedParamNames = new List<string>()
        {
            "id",
            "key1",
            "key2",
            "key4"
        };

        string expectedQuery = "UPDATE test_table SET key1 = @key1, key2 = @key2, key4 = @key4, updated_at = @updated_at WHERE id = @id RETURNING *;";

        Guid expectedGuid = new Guid();
        var expectedParameters = new DynamicParameters();

        expectedParameters.Add("key1", "value1");
        expectedParameters.Add("key2", 123);
        expectedParameters.Add("key4", true);

        // Act

        DbUtils dbUtils = new DbUtils();

        UpdateQueryPackage? updatePackage = dbUtils.BuildUpdateQuery("test_table", expectedGuid, testObject);

        // Assert

        Assert.IsNotNull(updatePackage);
        Assert.AreEqual(expectedQuery, updatePackage.sql);
        Assert.AreEqual(expectedGuid, updatePackage.parameters.Get<Guid>("id"));

        CollectionAssert.AreEqual(expectedParamNames.Take(3).ToList(), updatePackage.parameters.ParameterNames.Take(3).ToList());

        Assert.AreEqual(expectedParameters.Get<string>("key1"), updatePackage.parameters.Get<string>("key1"));
        Assert.AreEqual(expectedParameters.Get<int>("key2"), updatePackage.parameters.Get<int>("key2"));
        Assert.AreEqual(expectedParameters.Get<bool>("key4"), updatePackage.parameters.Get<bool>("key4"));
        Assert.IsNotNull(updatePackage.parameters.Get<DateTime>("updated_at"));


    }

    [TestMethod]
    public void Generates_Nothing_If_No_Params_Set()
    {
        // Arrange

        TestType testObject = new TestType()
        {
            key1 = null,
            key2 = null,
            key3 = null,
            key4 = null
        };

        // Act

        DbUtils dbUtils = new DbUtils();

        UpdateQueryPackage? updatePackage = dbUtils.BuildUpdateQuery("test_table", new Guid(), testObject);

        // Assert

        Assert.IsNull(updatePackage);
    }
}


[TestClass]
public class BuildSelectQuery
{
    [TestMethod]
    public void Generates_Valid_Select_Query_And_Params_Without_Paging()
    {
        // Arrange 
        string expectedQuery = "SELECT * FROM test_table\n" +
            "WHERE\n" +
            "column1 = @column1 AND\n" +
            "column2 ILIKE @column2 AND\n" +
            "LOWER(column3) = LOWER(@column3) AND\n" +
            "column4 LIKE @column4 AND\n" +
            "column5 IN (\n" +
            "@column5_0,\n" +
            "@column5_1,\n" +
            "@column5_2\n" +
            ") AND\n" +
            "LOWER(column6) IN (\n" +
            "LOWER(@column6_0),\n" +
            "LOWER(@column6_1),\n" +
            "LOWER(@column6_2)\n" +
            ")";

        List<string> expectedParamNames = new List<string>()
        {
            "column1",
            "column2",
            "column3",
            "column4",
            "column5_0",
            "column5_1",
            "column5_2",
            "column6_0",
            "column6_1",
            "column6_2",
        };

        List<ISearchTerm> inputSearchTerms = new List<ISearchTerm>()
        {
            new ExactMatchSearchTerm<string>("column1","ExactValueCol1"),
            new LikeSearchTerm("column2","LikeCIValue",LikeTypes.Like),
            new ExactMatchSearchTerm<string>("column3","ExactValueCol3", true),
            new LikeSearchTerm("column4","LikeValue",LikeTypes.EndsWith, false),
            new InArraySearchTerm<string>("column5",new List<string>() {"listVal1", "listVal2", "listVal3" }),
            new InArraySearchTerm<string>("column6",new List<string>() {"CIlistVal1", "CIlistVal2", "CIlistVal3" }, true)
        };

        // Act

        DbUtils dbUtils = new DbUtils();

        SelectQueryPackage? selectQueryPackage = dbUtils.BuildSelectQuery("test_table", inputSearchTerms, null, true);

        // Assert

        Assert.IsNotNull(selectQueryPackage);
        Assert.AreEqual(expectedQuery, selectQueryPackage.sql);

        CollectionAssert.AreEqual(expectedParamNames, selectQueryPackage.parameters.ParameterNames.ToList());

        Assert.AreEqual("ExactValueCol1", selectQueryPackage.parameters.Get<string>("column1"));
        Assert.AreEqual("%LikeCIValue%", selectQueryPackage.parameters.Get<string>("column2"));
        Assert.AreEqual("ExactValueCol3", selectQueryPackage.parameters.Get<string>("column3"));
        Assert.AreEqual("%LikeValue", selectQueryPackage.parameters.Get<string>("column4"));
        Assert.AreEqual("listVal1", selectQueryPackage.parameters.Get<string>("column5_0"));
        Assert.AreEqual("listVal2", selectQueryPackage.parameters.Get<string>("column5_1"));
        Assert.AreEqual("listVal3", selectQueryPackage.parameters.Get<string>("column5_2"));
        Assert.AreEqual("CIlistVal1", selectQueryPackage.parameters.Get<string>("column6_0"));
        Assert.AreEqual("CIlistVal2", selectQueryPackage.parameters.Get<string>("column6_1"));
        Assert.AreEqual("CIlistVal3", selectQueryPackage.parameters.Get<string>("column6_2"));

    }

    [TestMethod]
    public void Generates_Valid_Select_Query_And_Params_With_Paging()
    {
        // Arrange 
        string expectedQuery = "SELECT *, count(*) over() as full_count FROM test_table\n" +
            "WHERE\n" +
            "column1 = @column1 AND\n" +
            "column2 ILIKE @column2 AND\n" +
            "LOWER(column3) = LOWER(@column3) AND\n" +
            "column4 LIKE @column4 AND\n" +
            "column5 IN (\n" +
            "@column5_0,\n" +
            "@column5_1,\n" +
            "@column5_2\n" +
            ") AND\n" +
            "LOWER(column6) IN (\n" +
            "LOWER(@column6_0),\n" +
            "LOWER(@column6_1),\n" +
            "LOWER(@column6_2)\n" +
            ")" +
            "\nOFFSET @offset LIMIT @limit";

        List<string> expectedParamNames = new List<string>()
        {
            "column1",
            "column2",
            "column3",
            "column4",
            "column5_0",
            "column5_1",
            "column5_2",
            "column6_0",
            "column6_1",
            "column6_2",
            "limit",
            "offset"

        };

        List<ISearchTerm> inputSearchTerms = new List<ISearchTerm>()
        {
            new ExactMatchSearchTerm<string>("column1","ExactValueCol1"),
            new LikeSearchTerm("column2","LikeCIValue",LikeTypes.Like),
            new ExactMatchSearchTerm<string>("column3","ExactValueCol3", true),
            new LikeSearchTerm("column4","LikeValue",LikeTypes.EndsWith, false),
            new InArraySearchTerm<string>("column5",new List<string>() {"listVal1", "listVal2", "listVal3" }),
            new InArraySearchTerm<string>("column6",new List<string>() {"CIlistVal1", "CIlistVal2", "CIlistVal3" }, true)
        };

        // Act

        DbUtils dbUtils = new DbUtils();

        SelectQueryPackage? selectQueryPackage = dbUtils.BuildSelectQuery("test_table", inputSearchTerms, null, false);

        // Assert

        Assert.IsNotNull(selectQueryPackage);
        Assert.AreEqual(expectedQuery, selectQueryPackage.sql);

        CollectionAssert.AreEqual(expectedParamNames, selectQueryPackage.parameters.ParameterNames.ToList());

        Assert.AreEqual("ExactValueCol1", selectQueryPackage.parameters.Get<string>("column1"));
        Assert.AreEqual("%LikeCIValue%", selectQueryPackage.parameters.Get<string>("column2"));
        Assert.AreEqual("ExactValueCol3", selectQueryPackage.parameters.Get<string>("column3"));
        Assert.AreEqual("%LikeValue", selectQueryPackage.parameters.Get<string>("column4"));
        Assert.AreEqual("listVal1", selectQueryPackage.parameters.Get<string>("column5_0"));
        Assert.AreEqual("listVal2", selectQueryPackage.parameters.Get<string>("column5_1"));
        Assert.AreEqual("listVal3", selectQueryPackage.parameters.Get<string>("column5_2"));
        Assert.AreEqual("CIlistVal1", selectQueryPackage.parameters.Get<string>("column6_0"));
        Assert.AreEqual("CIlistVal2", selectQueryPackage.parameters.Get<string>("column6_1"));
        Assert.AreEqual("CIlistVal3", selectQueryPackage.parameters.Get<string>("column6_2"));
        Assert.AreEqual(0, selectQueryPackage.parameters.Get<int>("offset"));
        Assert.AreEqual(25, selectQueryPackage.parameters.Get<int>("limit"));
    }

    [TestMethod]
    public void Generates_Nothing_If_No_Params_Set()
    {
        // Arrange

        TestType testObject = new TestType()
        {
            key1 = null,
            key2 = null,
            key3 = null,
            key4 = null
        };

        // Act

        DbUtils dbUtils = new DbUtils();

        UpdateQueryPackage? updatePackage = dbUtils.BuildUpdateQuery("test_table", new Guid(), testObject);

        // Assert

        Assert.IsNull(updatePackage);
    }
}


[TestClass]
public class BuildPagingInfo
{
    [TestMethod]
    public void Generates_Valid_Paging_Info_Query_And_Params()
    {
        // Arrange
        DynamicParameters initialDynamicParams = new DynamicParameters();
        initialDynamicParams.Add("dummy", "dummyValue");

        SelectQueryPackage package = new SelectQueryPackage("DUMMY LINE", initialDynamicParams);

        string expectedQuery = $"DUMMY LINE" +
            $"\nORDER BY @sort_by DESC" +
            $"\nOFFSET @offset LIMIT @limit";

        DynamicParameters expectedParams = new DynamicParameters();
        expectedParams.Add("dummy", "dummyValue");
        expectedParams.Add("sort_by", "beef");
        expectedParams.Add("limit", 21);
        expectedParams.Add("offset", 42);


        PagingInfo pagingInfo = new PagingInfo()
        {
            Page = 3,
            PageLength = 21,
            IsDescending = true,
            SortBy = "beef"
        };

        // Act 

        DbUtils utils = new DbUtils();

        utils.BuildPagingInfo(ref package, pagingInfo);

        // Assert

        Assert.AreEqual(expectedQuery, package.sql);

        Assert.AreEqual(expectedParams.Get<string>("dummy"), package.parameters.Get<string>("dummy"));
        Assert.AreEqual(expectedParams.Get<string>("sort_by"), package.parameters.Get<string>("sort_by"));
        Assert.AreEqual(expectedParams.Get<int>("limit"), package.parameters.Get<int>("limit"));
        Assert.AreEqual(expectedParams.Get<int>("offset"), package.parameters.Get<int>("offset"));
    }

    [TestMethod]
    public void GeneratesDefaultIfNotSkippingPagingAndNoPagingSupplied()
    {
        // Arrange
        DynamicParameters initialDynamicParams = new DynamicParameters();
        initialDynamicParams.Add("dummy", "dummyValue");

        SelectQueryPackage package = new SelectQueryPackage("DUMMY LINE", initialDynamicParams);

        string expectedQuery = $"DUMMY LINE" +
            $"\nOFFSET @offset LIMIT @limit";

        DynamicParameters expectedParams = new DynamicParameters();
        expectedParams.Add("dummy", "dummyValue");
        expectedParams.Add("limit", 25);
        expectedParams.Add("offset", 0);

        // Act 

        DbUtils utils = new DbUtils();

        utils.BuildPagingInfo(ref package, null);

        // Assert

        Assert.AreEqual(expectedQuery, package.sql);

        Assert.AreEqual(expectedParams.Get<string>("dummy"), package.parameters.Get<string>("dummy"));
        Assert.AreEqual(expectedParams.Get<int>("limit"), package.parameters.Get<int>("limit"));
        Assert.AreEqual(expectedParams.Get<int>("offset"), package.parameters.Get<int>("offset"));
    }
}

[TestClass]
public class ExactMatchGenerateClauseAndParameters
{
    [TestMethod]
    public void GeneratesClauseAndParametersCaseSensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";

        ExactMatchSearchTerm<string> exactMatchSearchTerm = new ExactMatchSearchTerm<string>(columnName, value);

        string expectedClause = $"{columnName} = @{columnName}";
        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = exactMatchSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(value, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersCaseInsensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";

        ExactMatchSearchTerm<string> exactMatchSearchTerm = new ExactMatchSearchTerm<string>(columnName, value, true);

        string expectedClause = $"LOWER({columnName}) = LOWER(@{columnName})";
        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = exactMatchSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(value, clauseAndParameters.Parameters.Get<object>(columnName));
    }
}


[TestClass]
public class LikeGenerateClauseAndParameters
{
    [TestMethod]
    public void GeneratesClauseAndParametersForStartsWithCaseInsensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.StartsWith);

        string expectedClause = $"{columnName} ILIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersForStartsWithCaseSensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.StartsWith, false);

        string expectedClause = $"{columnName} LIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersForEndsWithCaseInsensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.EndsWith);

        string expectedClause = $"{columnName} ILIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersForEndsWithCaseSensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.EndsWith, false);

        string expectedClause = $"{columnName} LIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersForLikeCaseInsensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.Like);

        string expectedClause = $"{columnName} ILIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersForLikeCaseSensitive()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.Like, false);

        string expectedClause = $"{columnName} LIKE @{columnName}";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName"
        };

        // Act

        ClauseAndParameters clauseAndParameters = likeSearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedValue, clauseAndParameters.Parameters.Get<object>(columnName));
    }

}

[TestClass]
public class InArraySearchTermsGenerateClauseAndParameters
{
    [TestMethod]
    public void GeneratesClauseAndParametersCaseSensitive()
    {
        // Arrange
        string columnName = "columnName";
        List<string> values = new List<string> { "value1", "value2", "value3" };

        InArraySearchTerm<string> inArraySearchTerm = new InArraySearchTerm<string>(columnName, values);

        string expectedClause = $"{columnName} IN (\n@{columnName}_0,\n@{columnName}_1,\n@{columnName}_2\n)";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName_0",
            "columnName_1",
            "columnName_2"
        };

        // Act

        ClauseAndParameters clauseAndParameters = inArraySearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual("value1", clauseAndParameters.Parameters.Get<object>($"{columnName}_0"));
        Assert.AreEqual("value2", clauseAndParameters.Parameters.Get<object>($"{columnName}_1"));
        Assert.AreEqual("value3", clauseAndParameters.Parameters.Get<object>($"{columnName}_2"));
    }

    [TestMethod]
    public void GeneratesClauseAndParametersCaseInsensitive()
    {
        // Arrange
        string columnName = "columnName";
        List<string> values = new List<string> { "value1", "value2", "value3" };

        InArraySearchTerm<string> inArraySearchTerm = new InArraySearchTerm<string>(columnName, values, true);

        string expectedClause = $"LOWER({columnName}) IN (\nLOWER(@{columnName}_0),\nLOWER(@{columnName}_1),\nLOWER(@{columnName}_2)\n)";

        List<string> expectedParamNames = new List<string>()
        {
            "columnName_0",
            "columnName_1",
            "columnName_2"
        };

        // Act

        ClauseAndParameters clauseAndParameters = inArraySearchTerm.GenerateClauseAndParameters();

        // Assert

        Assert.AreEqual(expectedClause, clauseAndParameters.Clause);

        CollectionAssert.AreEqual(expectedParamNames, clauseAndParameters.Parameters.ParameterNames.ToList());

        Assert.AreEqual("value1", clauseAndParameters.Parameters.Get<object>($"{columnName}_0"));
        Assert.AreEqual("value2", clauseAndParameters.Parameters.Get<object>($"{columnName}_1"));
        Assert.AreEqual("value3", clauseAndParameters.Parameters.Get<object>($"{columnName}_2"));
    }

}
