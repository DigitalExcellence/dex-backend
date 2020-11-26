namespace Models
{

    public interface IDataSource
    {
        string Guid { get; }

        string Name { get; }

        string BaseUrl { get; }

        string OauthUrl { get; }

        string ClientId { get; }

        string ClientSecret { get; }

    }

}
