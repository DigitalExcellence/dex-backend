namespace Services.Sources
{
    public interface ISource
    {
        void Search(string searchTerm);
        void getSource(string url);
    }
}