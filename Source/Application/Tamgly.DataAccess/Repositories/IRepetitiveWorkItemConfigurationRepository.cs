using Tamgly.DataAccess.Models;

namespace Tamgly.DataAccess.Repositories;

public interface IRepetitiveWorkItemConfigurationRepository
{
    RepetitiveWorkItemConfigurationDatabaseRecord? Find(int parentWorkItemId);
    void Add(RepetitiveWorkItemConfigurationDatabaseRecord entity);
    void Remove(int id);
}