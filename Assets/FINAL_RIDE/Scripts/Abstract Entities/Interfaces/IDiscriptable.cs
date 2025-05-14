public interface IDiscriptable
{
    public string Name { get; set; }

    public string Discription { get; }

    void Describe(string text);

}