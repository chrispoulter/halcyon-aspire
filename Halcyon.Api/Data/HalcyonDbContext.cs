using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Data;

public class HalcyonDbContext(DbContextOptions<HalcyonDbContext> options) : DbContext(options)
{
}
