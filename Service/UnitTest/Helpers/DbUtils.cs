namespace Helpers;

using AutoMapper.Execution;
using System.Xml.Linq;
using Dapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using WebApi.Helpers;

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

        string expectedQuery = "UPDATE test_table SET key1 = @key1, key2 = @key2, key4 = @key4 WHERE id = @id RETURNING *;";

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

        CollectionAssert.AreEqual(expectedParamNames, updatePackage.parameters.ParameterNames.ToList());

        Assert.AreEqual(expectedParameters.Get<string>("key1"), updatePackage.parameters.Get<string>("key1"));
        Assert.AreEqual(expectedParameters.Get<int>("key2"), updatePackage.parameters.Get<int>("key2"));
        Assert.AreEqual(expectedParameters.Get<bool>("key4"), updatePackage.parameters.Get<bool>("key4"));


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
    public void Generates_Valid_Select_Query_And_Params()
    {
        // Arrange 
        string expectedQuery = "SELECT * FROM test_table\n" +
            "WHERE\n" +
            "column1 = @column1 AND\n" +
            "column2 LIKE @column2 AND\n" +
            "column3 = @column3 AND\n" +
            "column4 IN (\n" +
            "@column4_0,\n" +
            "@column4_1,\n" +
            "@column4_2\n" +
            ")";

        List<string> expectedParamNames = new List<string>()
        {
            "column1",
            "column2",
            "column3",
            "column4_0",
            "column4_1",
            "column4_2"
        };

        List<ISearchTerm> inputSearchTerms = new List<ISearchTerm>()
        {
            new ExactMatchSearchTerm<string>("column1","ExactValueCol1"),
            new LikeSearchTerm("column2","LikeValue",LikeTypes.Like),
            new ExactMatchSearchTerm<string>("column3","ExactValueCol3"),
            new InArraySearchTerm<string>("column4",new List<string>() {"listVal1", "listVal2", "listVal3" })
        };

        // Act

        DbUtils dbUtils = new DbUtils();

        SelectQueryPackage? selectQueryPackage = dbUtils.BuildSelectQuery("test_table", inputSearchTerms);

        // Assert

        Assert.IsNotNull(selectQueryPackage);
        Assert.AreEqual(expectedQuery, selectQueryPackage.sql);

        CollectionAssert.AreEqual(expectedParamNames, selectQueryPackage.parameters.ParameterNames.ToList());


        Assert.AreEqual("ExactValueCol1", selectQueryPackage.parameters.Get<string>("column1"));
        Assert.AreEqual("%LikeValue%", selectQueryPackage.parameters.Get<string>("column2"));
        Assert.AreEqual("ExactValueCol3", selectQueryPackage.parameters.Get<string>("column3"));
        Assert.AreEqual("listVal1", selectQueryPackage.parameters.Get<string>("column4_0"));
        Assert.AreEqual("listVal2", selectQueryPackage.parameters.Get<string>("column4_1"));
        Assert.AreEqual("listVal3", selectQueryPackage.parameters.Get<string>("column4_2"));

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
public class ExactMatchGenerateClauseAndParameters
{
    [TestMethod]
    public void GeneratesClauseAndParameters()
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
}


[TestClass]
public class LikeGenerateClauseAndParameters
{
    [TestMethod]
    public void GeneratesClauseAndParametersForStartsWith()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.StartsWith);

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
    public void GeneratesClauseAndParametersForEndsWith()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.EndsWith);

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
    public void GeneratesClauseAndParametersForLike()
    {
        // Arrange
        string columnName = "columnName";
        string value = "stringValue";
        string expectedValue = "%stringValue%";

        LikeSearchTerm likeSearchTerm = new LikeSearchTerm(columnName, value, LikeTypes.Like);

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
    public void GeneratesClauseAndParameters()
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

}
