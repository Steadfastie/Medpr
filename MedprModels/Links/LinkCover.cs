using MedprModels.Interfaces;

namespace MedprModels.Links;

public static class LinkCover
{
    /// <summary>
    /// Method return HATEOAS cover for model with Links property
    /// </summary>
    /// <param name="model">Model with Links property</param>
    /// <param name="controller">Controller name</param>
    /// <param name="id">ID of model</param>
    /// <returns></returns>
    public static T GenerateLinks<T>(this T model, string controller) where T : IHateoas
    {
        if (!model.Links.Any())
        {
            model.Links = new List<Link>()
            {
                new Link()
                {
                    Href = $"https://localhost:7259/{controller}/{model.Id}",
                    Rel = "self",
                    Type = "GET"
                },
                new Link()
                {
                    Href = $"https://localhost:7259/{controller}/{model.Id}",
                    Rel = "update",
                    Type = "PATCH"
                },
                new Link()
                {
                    Href = $"https://localhost:7259/{controller}/{model.Id}",
                    Rel = "delete",
                    Type = "DELETE"
                }
            };
        }
        return model;
    }
}
