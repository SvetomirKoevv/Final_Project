namespace Common.Entities.Other;

public class ResultSet<T>
{
    public bool IsSuccess { get; set; }
    public List<ResultSetError> ErrorMessages { get; set; }
    public T Data { get; set; }
}

public class ResultSetError
{
    public string Name { get; set; } 
    public List<string> Mesages { get; set; } 
}