using Microsoft.EntityFrameworkCore;
using ScWorks.Api.Data.Models;

namespace ScWorks.Api.Data;

public class ScWorksRepo(ScWorksDbContext context) : IScWorksRepo
{
    public bool AddWork(ScWork work)
    {
        ArgumentNullException.ThrowIfNull(work);
        context.Add(work);
        return SaveChanges();
    }

    public void DeleteWork(ScWork work)
    {
        context.Remove(work);
        SaveChanges();
    }

    public async Task<List<ScWork>> GetAllWorks()
    {
        return await context.ScWorks.ToListAsync();
    }

    public ScWork GetWorkById(Guid id)
    {
        return context.ScWorks.FirstOrDefault(x => x.Id == id);
    }

    public bool UpdateWork(ScWork work)
    {
        context.Update(work);
        return SaveChanges();
    }

    public bool SaveChanges()
    {
        return context.SaveChanges() > 0;
    }


}

