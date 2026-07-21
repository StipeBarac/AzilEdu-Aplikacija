using AzilEdu.Api.Data;
using Microsoft.EntityFrameworkCore;
using AzilEdu.Shared.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AzilEduDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();

    await db.Database.MigrateAsync();

    if (!await db.Animals.AnyAsync())
    {
        db.Animals.AddRange(
            new Animal
            {
                Name = "Luna",
                Species = "Pas",
                Breed = "Labrador",
                Gender = "Ženka",
                Age = 3,
                ArrivalDate = new DateTime(2025, 10, 12),
                IsAdopted = false,
                ImageUrl = "/images/animals/luna.webp",
                Description = "Mirna i druželjubiva kujica koja voli šetnje."
            },
            new Animal
            {
                Name = "Maza",
                Species = "Mačka",
                Breed = "Domaća kratkodlaka",
                Gender = "Ženka",
                Age = 2,
                ArrivalDate = new DateTime(2025, 11, 5),
                IsAdopted = true,
                ImageUrl = "/images/animals/maza.webp",
                Description = "Zaigrana mačka naviknuta na boravak u zatvorenom prostoru."
            },
            new Animal
            {
                Name = "Rex",
                Species = "Pas",
                Breed = "Njemački ovčar",
                Gender = "Mužjak",
                Age = 5,
                ArrivalDate = new DateTime(2026, 1, 20),
                IsAdopted = false,
                ImageUrl = "/images/animals/rex.webp",
                Description = "Aktivan pas koji traži iskusnijeg vlasnika."
            },
            new Animal
            {
                Name = "Nala",
                Species = "Mačka",
                Breed = "Maine Coon mješanac",
                Gender = "Ženka",
                Age = null,
                ArrivalDate = new DateTime(2026, 2, 3),
                IsAdopted = false,
                ImageUrl = "/images/animals/nala.webp",
                Description = "Mlada mačka pronađena bez poznate povijesti."
            },
            new Animal
            {
                Name = "Tobi",
                Species = "Pas",
                Breed = "Mješanac",
                Gender = "Mužjak",
                Age = 1,
                ArrivalDate = null,
                IsAdopted = false,
                ImageUrl = "/images/animals/tobi.webp",
                Description = "Vesel pas kojem datum dolaska još nije potvrđen."
            },
            new Animal
            {
                Name = "Bruno",
                Species = "Pas",
                Breed = "Bigl",
                Gender = "Mužjak",
                Age = 4,
                ArrivalDate = new DateTime(2025, 9, 18),
                IsAdopted = true,
                ImageUrl = "/images/animals/bruno.webp",
                Description = "Udomljen pas koji ostaje u evidenciji azila."
            }
        );

        await db.SaveChangesAsync();
    }
}



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.HousingUnits.AnyAsync())
    {
        db.HousingUnits.AddRange(
    


    new HousingUnit
    {
        Id = 1,
        Name = "Boks A1",
        UnitType = "Boks",
        Capacity = 4,
        Occupied = 4,                                  // PUN
        LastCleanedAt = new DateTime(2026, 7, 15),
        IsActive = true,
        ImageUrl = "images/HousingUnits/boks-a1.jpg",
        Note = "Veliki psi, pristup vanjskom dvorištu."
    },
            new HousingUnit
            {
                Id = 2,
                Name = "Boks A2",
                UnitType = "Boks",
                Capacity = 3,
                Occupied = 1,                                  // ima mjesta
                LastCleanedAt = new DateTime(2026, 7, 13),
                IsActive = true,
                ImageUrl = "images/HousingUnits/boks-a2.jpg",
                Note = "Mirniji boks, pogodan za starije pse."
            },
        new HousingUnit
        {
            Id = 3,
            Name = "Mačja soba B1",
            UnitType = "Soba",
            Capacity = 6,
            Occupied = 4,
            LastCleanedAt = null,                          // nepoznato
            IsActive = true,
            ImageUrl = "images/HousingUnits/soba-b1.jpg",
            Note = "Penjalice za mačke."
        },
        new HousingUnit
        {
            Id = 4,
            Name = "Mačja soba B2",
            UnitType = "Soba",
            Capacity = 4,
            Occupied = 4,                                  // PUN
            LastCleanedAt = new DateTime(2026, 7, 16),
            IsActive = true,
            ImageUrl = "images/HousingUnits/soba-b2.jpg",
            Note = "Trenutno popunjeno, čeka se udomljavanje."
        },
        new HousingUnit
        {
            Id = 5,
            Name = "Karantena K1",
            UnitType = "Karantena",
            Capacity = 2,
            Occupied = 1,
            LastCleanedAt = new DateTime(2026, 7, 14),
            IsActive = true,
            ImageUrl = "images/HousingUnits/karantena-k1.jpg",
            Note = "Novopristigle životinje, obavezan pregled."
        },
        new HousingUnit
        {
            Id = 6,
            Name = "Boks C3",
            UnitType = "Boks",
            Capacity = 5,
            Occupied = 0,                                  // neaktivan → prazan
            LastCleanedAt = new DateTime(2026, 6, 18),
            IsActive = false,                              // NEAKTIVNA
            ImageUrl = "images/HousingUnits/boks-c3.jpg",
            Note = "Zatvoren zbog sanacije poda."
        }
    );

    await db.SaveChangesAsync();

    }
}


    


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
