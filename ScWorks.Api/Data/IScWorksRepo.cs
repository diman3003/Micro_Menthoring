using ScWorks.Api.Data.Models;

namespace ScWorks.Api.Data;

public interface IScWorksRepo
{
    bool SaveChanges();
    Task<List<ScWork>> GetAllWorks();
    ScWork GetWorkById(Guid id);
    ScWork GetWorkByObjId(string id);
    bool AddWork(ScWork work);
    void DeleteWork(ScWork work);
    bool UpdateWork(ScWork work);
}

