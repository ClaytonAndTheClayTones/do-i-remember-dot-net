namespace UnitTest;

using WebApi.Helpers;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        // Arrange

        Dictionary<string, object?> testDict = new Dictionary<string, object?>()
        {
            { "key1" , "value1" },
            { "key2", "value2" },
            { "key3", null } ,
            { "key4" ,"value4" } ,

        };

        string expectedQuery = "UPDATE test_table SET key1 = @key1, key2 = @key2, key4 = @key4 WHERE id = @id;";
        // Act

        DbUtils dbUtils = new DbUtils();

        string? query = dbUtils.BuildUpdateQuery("test_table", testDict);

        // Assert

        Assert.IsNotNull(query);
        Assert.AreEqual(expectedQuery, query);
    }
}
