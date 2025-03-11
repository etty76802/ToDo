
using Microsoft.EntityFrameworkCore; 
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
new MySqlServerVersion(new Version(8, 0, 21))));

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (ToDoDbContext db) => await db.Items.ToListAsync());

app.MapGet("/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
    return item != null ? Results.Ok(item) : Results.NotFound();
});

app.MapPost("/addItem", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/addItem/{item.Id}", item);
});

app.MapPut("/updateItem/{id}", async (ToDoDbContext db,int id, Item item) =>
{
    var i = await db.Items.FindAsync(id);
    if (i == null) return Results.NotFound();
    
    i.IsComplete = item.IsComplete;
    await db.SaveChangesAsync();
    return Results.Ok(i);
});

app.MapDelete("/removeItem/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FirstOrDefaultAsync(x => x.Id == id);
    if (item == null) return Results.NotFound();
    
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

