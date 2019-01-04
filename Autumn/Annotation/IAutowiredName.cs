namespace Autumn.Annotation
{
    public interface IAutowiredName
    {
        string[] Names { get; }

        bool IsName(string name);
    }
}