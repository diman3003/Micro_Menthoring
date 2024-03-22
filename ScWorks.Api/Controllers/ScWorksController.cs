using ScWorks.Api.Data;
using ScWorks.Api.Data.Models;

namespace ScWorks.Api.APIs;
internal class ScWorksController
{
    public void Register(WebApplication app)
    {
        // GET ALL
        app.MapGet("/Men/ScWorks", async (IScWorksRepo db) => await db.GetAllWorks())
            .Produces<IEnumerable<ScWork>>(StatusCodes.Status200OK)
            .WithName("GetAllScWorks")
            .WithTags("Getters");

        //GET BY ID
        app.MapGet("/Men/ScWorks/{id}", (IScWorksRepo db, Guid id) =>
        {
            var work = db.GetWorkById(id);
            return work is not null ? Results.Ok(work) : Results.NotFound();
        })
            .Produces<ScWork>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetWorkById")
            .WithTags("Getters");

        //CREATE
        app.MapPost("/Men/ScWorks", (IScWorksRepo db, ScWork work) =>
        {
            db.AddWork(work);
            return Results.Created("/Men/ScWorks", work);
        })
            .Accepts<ScWork>("application/json")
            .Produces<ScWork>(StatusCodes.Status201Created)
            .WithName("Add")
            .WithTags("Creators");

        //UPDATE
        app.MapPut("/Men/ScWorks", (IScWorksRepo db, ScWork work) =>
        {
            return db.UpdateWork(work) ? Results.Ok(work) : Results.NotFound();
        })
            .Accepts<ScWork>("application/json")
            .WithName("Update")
            .WithTags("Updaters");

        //DELETE
        app.MapDelete("/Men/ScWorks/{id}", (IScWorksRepo db, Guid id) => 
        {
            var work = db.GetWorkById(id);
            if (work == null) return Results.NotFound();
            db.DeleteWork(work);
            return Results.NoContent();
        })
            .Produces(StatusCodes.Status204NoContent)
            .WithName("Delete")
            .WithTags("Deleters");
    }
}
