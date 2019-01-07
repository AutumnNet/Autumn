namespace Autumn.Net.Annotation
{
    public interface IAutowiredName
    {
        string[] Names { get; }

        bool IsName(string name);
    }
}