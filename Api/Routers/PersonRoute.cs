using FafaAPI.Domain.Models;
using FafaAPI.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using FafaAPI.Application.DTO;

namespace FafaAPI.Api.Routers;

public static class PersonRoute
{
    public static void PersonRoutes(this WebApplication app)
    {
        var route = app.MapGroup("/person");

        route.MapPost("", 
            async (PersonRequest req, PersonContext context) =>
        {
            try
            {
                var person = new PersonModel(req.name);
                await context.AddAsync(person);
                await context.SaveChangesAsync();
                return Results.Created(person.Id.ToString(), person);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.BadRequest(e.Message);
            }
        });
        
        route.MapGet("",
            async (PersonContext context) =>
            {
                try
                {
                    var people = await context.People.ToListAsync();
                    return people.Count != 0 ? Results.Ok(people) : Results.NotFound();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.BadRequest(e.Message);
                }
            }
            
        );

        route.MapGet("{id:guid}",
            async (Guid id, PersonContext context) =>
            {
                try
                {
                    var person = await context.People.FindAsync(id);
                    return person == null ? Results.NotFound() : Results.Ok(person);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Results.BadRequest(e.Message);
                }
            });
        
        route.MapPut("{id:guid}", 
            async (Guid id, PersonRequest req, PersonContext context) =>
        {
            try
            {
                var person = await context.People.FindAsync(id);
                
                if (person == null)
                    return Results.NotFound();

                person.ChangeName(req.name);
                await context.SaveChangesAsync();
                return Results.Ok(person);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.BadRequest(e.Message);
            }
        });

        route.MapDelete("{id:guid}", 
            async (Guid id, PersonContext context) =>
        {
            try
            {
                var person = await context.People.FindAsync(id);
                
                if (person == null)
                    return Results.NotFound();
                
                context.People.Remove(person);
                await context.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.BadRequest(e.Message);
            }
        });
    }
}