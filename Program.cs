var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<WineValidator>(); //validator toevoegen altijd op de 2de regel
var app = builder.Build();

app.MapGet("/", () => "Hello World!"); //anonymous function -> geen paramaters

//static list meerdere clietns maken kunne we de lijst delen
var wines = new List<Wines>();
wines.Add(new Wines { WineId = 1, Name = "Chardonnay", Year = 2010, Grapes = "Chardonnay", Country = "France", Price = 45 });
wines.Add(new Wines { WineId = 2, Name = "Chianti", Year = 1944, Grapes = "Chianti", Country = "Italy", Price = 20 });
wines.Add(new Wines { WineId = 3, Name = "Pinot Noir", Year = 2010, Grapes = "Pinot Noir", Country = "France", Price = 30 });
wines.Add(new Wines { WineId = 4, Name = "Cabernet Sauvignon", Year = 2010, Grapes = "Cabernet Sauvignon", Country = "France", Price = 35 });


//get all wines
app.MapGet("/wines", () =>
{    // return de wijnen van de static lijst op lijn 7
    return Results.Ok(wines);
});

//post wine per id
//met validator
app.MapPost("/wines", (IValidator<Wines> validator, Wines wine) =>
{
    var result = validator.Validate(wine); //validator aanroepen
    if (!result.IsValid)
    {   //als de validator niet geldig is dan een badrequest terugsturen anders een created
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage }); // error message terugsturen -> anonieme object
        return Results.BadRequest(result.Errors);
    }
    wine.WineId = wines.Max(w => w.WineId) + 1;
    wines.Add(wine);
    return Results.Created($"/wines/{wine.WineId}", wine);
});
//zonder validator
// app.MapPost("/wines", (Wines wine) =>
// {
//     wine.WineId = wines.Count() + 1;
//     wines.Add(wine);

//     return Results.Created($"/wines/{wine.WineId}", wine);
// });

//delete
app.MapDelete("/wines/{wineId}", (int wineId) =>
{
    var wine = wines.FirstOrDefault(w => w.WineId == wineId);
    if (wine == null)
    {

        return Results.NotFound($"Wine {wineId} not found or doesn't exist");
    }

    wines.Remove(wine);

    return Results.Ok("Wine deleted");

});

//update 
app.MapPut("/wines/{wineId}", (int wineId, Wines wine) =>
{
    var existingWine = wines.FirstOrDefault(w => w.WineId == wineId);
    if (existingWine == null)
    {
        return Results.NotFound();
    }

    existingWine.Name = wine.Name;
    existingWine.Country = wine.Country;
    existingWine.Year = wine.Year;
    existingWine.Price = wine.Price;
    existingWine.Color = wine.Color;
    existingWine.Grapes = wine.Grapes;

    return Results.Ok();
});


//run on port 5000
app.Run("http://localhost:5000");
