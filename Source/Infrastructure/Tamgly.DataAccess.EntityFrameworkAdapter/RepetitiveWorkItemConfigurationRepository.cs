using Tamgly.DataAccess.Models;
using Tamgly.DataAccess.Repositories;

namespace Tamgly.DataAccess.EntityFrameworkAdapter;

public class RepetitiveWorkItemConfigurationRepository : IRepetitiveWorkItemConfigurationRepository
{
    private readonly TamglyEntityFrameworkDbContext _context;

    public RepetitiveWorkItemConfigurationRepository(TamglyEntityFrameworkDbContext context)
    {
        _context = context;
    }

    public RepetitiveWorkItemConfigurationDatabaseRecord? Find(int parentWorkItemId)
    {
        return _context.RepetitiveWorkItemConfigurations.Find(parentWorkItemId);
    }

    public void Add(RepetitiveWorkItemConfigurationDatabaseRecord entity)
    {
        _context.RepetitiveWorkItemConfigurations.Add(entity);
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        RepetitiveWorkItemConfigurationDatabaseRecord? value = Find(id);
        if (value is null)
            return;

        _context.Remove(value);
        _context.SaveChanges();
    }
}