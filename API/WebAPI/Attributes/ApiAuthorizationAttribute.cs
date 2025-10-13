using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PATOA.WebAPI.Attributes;

public class AdminApiAttribute : AuthorizeAttribute
{
    public AdminApiAttribute()
    {
        Policy = "AdminPolicy";
        Tags = new[] { "Admin" };
    }

    public string[] Tags { get; }
}

public class PublicApiAttribute : AuthorizeAttribute
{
    public PublicApiAttribute()
    {
        Policy = "PublicPolicy";
        Tags = new[] { "Public" };
    }

    public string[] Tags { get; }
} 