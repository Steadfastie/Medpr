using MedprModels;

namespace MedprWebAPI.Utils;

public class LinkCover
{
    /// <summary>
    /// Method return HATEOAS cover for some model from app
    /// </summary>
    /// <param name="controller">Controller name</param>
    /// <param name="id">ID of model</param>
    /// <returns></returns>
    public static List<Link> GenerateLinks(string controller, string id)
    {
        var links = new List<Link>()
        {
            new Link()
            {
                Href = $"https://localhost:5001/{controller}/{id}",
                Rel = "self",
                Type = "GET"
            },
            new Link()
            {
                Href = $"https://localhost:5001/{controller}/{id}",
                Rel = "update",
                Type = "PATCH"
            },
            new Link()
            {
                Href = $"https://localhost:5001/{controller}/{id}",
                Rel = "delete",
                Type = "DELETE"
            }
        };

        return links;
    }
}
