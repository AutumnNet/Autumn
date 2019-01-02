namespace Autumn.Annotation
{
    public interface IAutowiredName
    {
        string Name { get; }
        string[] Names { get; set; }

        bool IsName(string name);
    }
}