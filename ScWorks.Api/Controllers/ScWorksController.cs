using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ScWorks.Api.Data;
using ScWorks.Api.Data.Models;

namespace ScWorks.Api.Controllers;
internal class ScWorksController
{
    readonly IMongoDatabase _database;
    readonly IMongoCollection<Work> _works;

    #region private
    public ScWorksController()
    {
        _database = new MongoClient("mongodb://localhost:27017").GetDatabase("Menthoring");
        _works = _database.GetCollection<Work>("ScWorks");
    }

    /// <summary>
    /// Get File from Mongo to Local path.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="path"></param>
    private void GetFile(string id, string path)
    {
        GridFSBucket gridFS = new(_database);
        using Stream fs = File.OpenWrite(path);
        gridFS.DownloadToStream(new ObjectId(id), fs);
    }

    /// <summary>
    /// Put File from Local path to Mongo.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private string PutFile(string path)
    {
        GridFSBucket gridFS = new(_database);
        FileInfo fi = new(path);
        using Stream fs = File.OpenRead(fi.FullName);
        ObjectId id = gridFS.UploadFromStream(fi.Name, fs);
        return id.ToString();
    }

    /// <summary>
    /// Delete File From Mongo.
    /// </summary>
    /// <param name="id"></param>
    private void DeleteFile(string id)
    {
        GridFSBucket gridFS = new(_database);
        gridFS.Delete(new ObjectId(id));
    }
    #endregion private

    public void Register(WebApplication app)
    {
        //GET ALL
        app.MapGet("/Men/ScWorks", async (IScWorksRepo db) =>
        {
            var t1 = db.GetAllWorks();
            var t2 = Task.Run(() =>
                {
                    return _works.Find(work => true).ToList();
                });

            await Task.WhenAll(t1, t2);
            return t1.Result;
        }).Produces<IEnumerable<ScWork>>(StatusCodes.Status200OK).WithName("GetAllScWorks").WithTags("Getters");

        //GET BY ID
        app.MapGet("/Men/ScWorks/{id}", (IScWorksRepo db, Guid id) =>
        {
            var work = db.GetWorkById(id);

            if (work != null)
            {
                GetFile(work.Annotation, $@"D:\Temp\{work.Annotation}.txt");
                return Results.Ok(work);
            }

            return Results.NotFound();
        }).Produces<ScWork>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("GetWorkById").WithTags("Getters");

        //CREATE
        app.MapPost("/Men/ScWorks", (IScWorksRepo db, ScWork work) =>
        {
            IGridFSBucket gridFS = new GridFSBucket(_database);
            FileInfo fi = new(work.FilePath);
            using Stream fs = File.OpenRead(fi.FullName);
            ObjectId id = gridFS.UploadFromStream(fi.Name, fs);
            work.Annotation = id.ToString();
            db.AddWork(work);

            return Results.Created("/Men/ScWorks", work);
        }).Accepts<ScWork>("application/json").Produces<ScWork>(StatusCodes.Status201Created).WithName("Add").WithTags("Creators");

        //UPDATE
        app.MapPut("/Men/ScWorks", (IScWorksRepo db, ScWork work) =>
        {
            return db.UpdateWork(work) ? Results.Ok(work) : Results.NotFound();
        }).Accepts<ScWork>("application/json").WithName("Update").WithTags("Updaters");

        //DELETE
        app.MapDelete("/Men/ScWorks/{id}", (IScWorksRepo db, Guid id) =>
        {
            var work = db.GetWorkById(id);

            if (work == null)
                return Results.NotFound();

            db.DeleteWork(work);
            DeleteFile(work.Annotation);
            return Results.NoContent();
        }).Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound).WithName("Delete").WithTags("Deleters");

        //GET FILE
        app.MapGet("/Men/ScWorks/File/{id}/{path}", (string id, string path) =>
        {
            GetFile(id, path);
            return Results.Ok(path);
        }).Produces<string>(StatusCodes.Status200OK).WithName("Get File").WithTags("Getters");

        //UPDATE FILE
        app.MapPut("/Men/ScWorks/File/{id}/{path}", (IScWorksRepo db, string id, string path) =>
        {
            var work = db.GetWorkByObjId(id);

            if (work != null)
            {
                DeleteFile(id);
                string newId = PutFile(path);
                work.Annotation = newId;
                db.UpdateWork(work);

                return Results.Ok(path);
            }

            return Results.NotFound(id);
        }).Produces<string>(StatusCodes.Status200OK).Produces<string>(StatusCodes.Status404NotFound).WithName("UpdateFile").WithTags("Updaters");
    }
}
