using WebApi.Helpers;

namespace Helpers;

[TestClass]
public class ConvertDelimitedStringToGuidList
{
    [TestMethod]
    public void Generates_Valid_List_With_Supplied_Delimiter_Omitting_Invalid()
    {
        // Arrange
        Guid guid1 = Guid.NewGuid();
        string value1 = guid1.ToString();

        string value2 = "aklsdnlkasndfg";

        Guid guid3 = Guid.NewGuid();
        string value3 = guid3.ToString();

        Guid guid4 = Guid.NewGuid();
        string value4 = guid4.ToString();


        string input = $"{value1}+{value2}+{value3}+{value4}";

        List<Guid> expectedOutput = new List<Guid>(new Guid[]{guid1, guid3, guid4});

        
        // Act

        CommonUtils commonUtils = new CommonUtils();

        List<Guid> result = commonUtils.ConvertDelimitedStringToGuidList(input, "+");

        // Assert  
        CollectionAssert.AreEqual(expectedOutput, result);
    }

    [TestMethod]
    public void Defaults_To_Comma()
    {
        // Arrange
        Guid guid1 = Guid.NewGuid();
        string value1 = guid1.ToString();
 
        Guid guid2 = Guid.NewGuid();
        string value2 = guid2.ToString();
         
        string input = $"{value1},{value2}";

        List<Guid> expectedOutput = new List<Guid>(new Guid[] { guid1, guid2 });


        // Act

        CommonUtils commonUtils = new CommonUtils();

        List<Guid> result = commonUtils.ConvertDelimitedStringToGuidList(input);

        // Assert  
        CollectionAssert.AreEqual(expectedOutput, result);
    }
}
