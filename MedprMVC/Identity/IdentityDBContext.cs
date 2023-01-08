using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedprMVC.Identity;

public class IdentityDBContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public IdentityDBContext(DbContextOptions<IdentityDBContext> options) : base(options)
    {
    }
}