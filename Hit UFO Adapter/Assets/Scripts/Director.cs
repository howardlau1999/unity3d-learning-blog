public class Director : Singleton<Director>
{
    protected Director()
    {
    }

    public IController currentController { get; set; }
}