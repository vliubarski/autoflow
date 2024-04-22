namespace AutoFlow.Models;

public enum Menu
    {

    [StringValue("1")]
    ListContact,

    [StringValue("2")]
    AddContact,

    [StringValue("3")]
    DeleteContact,

    [StringValue("4")]
    EditContact,

    [StringValue("5")]
    SearchByName,

    [StringValue("6")]
    SortByField,

    [StringValue("7")]
    ExportToCsvFile,

    [StringValue("8")]
    Quit,

    [StringValue("abc")]
    Undefined

}

public class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}