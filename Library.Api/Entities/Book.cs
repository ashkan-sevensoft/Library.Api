namespace Library.Api.Entities;

/// <summary>
/// کتاب
/// </summary>
public class Book
{
 
    /// <summary>
    /// شناسه
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// عنوان کتاب
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// نویسنده
    /// </summary>
    public string Author { get; set; } = string.Empty;

    //public StatusEnum status { get; set; }
}