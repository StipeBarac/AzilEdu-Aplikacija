using AzilEdu.Api.Data;
using Microsoft.EntityFrameworkCore;
using AzilEdu.Shared.Models;
using AzilEdu.Api.Security;
using AzilEdu.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AzilEduDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwtOptions = jwtSection.Get<JwtOptions>()
    ?? throw new InvalidOperationException("Nedostaje Jwt konfiguracija.");

if (jwtOptions.SigningKey.Length < 32)
    throw new InvalidOperationException(
        "Jwt:SigningKey mora imati najmanje 32 znaka.");

builder.Services.Configure<JwtOptions>(jwtSection);
builder.Services.AddScoped<JwtTokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy(
        AuthorizationPolicies.Staff,
        policy => policy.RequireRole("Admin", "Employee"));

    options.AddPolicy(
        AuthorizationPolicies.AdminOnly,
        policy => policy.RequireRole("Admin"));
});

builder.Services.Configure<AiOptions>(
    builder.Configuration.GetSection(AiOptions.SectionName));

builder.Services.AddScoped<MockAiService>();
builder.Services.AddScoped<OpenAiService>();

builder.Services.AddScoped<IAiService>(services =>
{
    var provider = builder.Configuration["Ai:Provider"];

    return string.Equals(
        provider,
        "OpenAI",
        StringComparison.OrdinalIgnoreCase)
            ? services.GetRequiredService<OpenAiService>()
            : services.GetRequiredService<MockAiService>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
                AnimalStatusId = 1,
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
                AnimalStatusId = 3,
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
                AnimalStatusId = 1,
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
                AnimalStatusId = 1,
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
                AnimalStatusId = 2,
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
                AnimalStatusId = 3,
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.Volunteers.AnyAsync())
    {
        db.Volunteers.AddRange(
            new Volunteer
            {
                FirstName = "Ana",
                LastName = "Horvat",
                Email = "ana.horvat@example.com",
                Phone = "0912345678",
                Skills = "Šetnja pasa, čišćenje boksova",
                AvailableFrom = new DateTime(2026, 1, 10),
                Notes = "Dolazi vikendom.",
                VolunteerStatusId = 2
            },
            new Volunteer
            {
                FirstName = "Marko",
                LastName = "Kovač",
                Email = "marko.kovac@example.com",
                Phone = "0987654321",
                Skills = "Fotografiranje životinja za oglase",
                AvailableFrom = new DateTime(2026, 2, 1),
                Notes = "Ima iskustva s plašljivim psima.",
                VolunteerStatusId = 1
            },
            new Volunteer
            {
                FirstName = "Ivana",
                LastName = "Novak",
                Email = "ivana.novak@example.com",
                Phone = "0911122334",
                Skills = "Njega mačaka, socijalizacija",
                AvailableFrom = null,
                Notes = "Trenutno na bolovanju.",
                VolunteerStatusId = 3
            },
            new Volunteer
            {
                FirstName = "Petar",
                LastName = "Babić",
                Email = "petar.babic@example.com",
                Phone = "0995566778",
                Skills = "Prijevoz životinja veterinaru",
                AvailableFrom = new DateTime(2025, 11, 20),
                Notes = "Ima kombi.",
                VolunteerStatusId = 4
            }
        );

        await db.SaveChangesAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.Donors.AnyAsync())
    {
        db.Donors.AddRange(
            new Donor
            {
                FirstName = "Ivan",
                LastName = "Marić",
                OrganizationName = "",
                Email = "ivan.maric@example.com",
                Phone = "0912345678",
                Address = "Ulica kralja Tomislava 12",
                City = "Split",
                Notes = "Redovito donira hranu za pse.",
                CreatedAt = new DateTime(2026, 1, 15),
                DonorTypeId = 1,          // Fizička osoba
                DonorStatusId = 2         // Aktivan
            },
            new Donor
            {
                FirstName = "Marija",
                LastName = "Kovačević",
                OrganizationName = "",
                Email = "marija.kovacevic@example.com",
                Phone = "0987654321",
                Address = "Vukovarska 5",
                City = "Zagreb",
                Notes = "Povremeno pomaže s prijevozom.",
                CreatedAt = new DateTime(2026, 2, 3),
                DonorTypeId = 1,          // Fizička osoba
                DonorStatusId = 3         // Povremeni
            },
            new Donor
            {
                FirstName = "",
                LastName = "",
                OrganizationName = "Pet Shop Šapica d.o.o.",
                Email = "info@sapica.hr",
                Phone = "021555123",
                Address = "Poljička cesta 30",
                City = "Split",
                Notes = "Mjesečno donira vreće hrane i pijesak za mačke.",
                CreatedAt = new DateTime(2025, 11, 20),
                DonorTypeId = 2,          // Tvrtka
                DonorStatusId = 2         // Aktivan
            },
            new Donor
            {
                FirstName = "",
                LastName = "",
                OrganizationName = "VetMedika d.o.o.",
                Email = "kontakt@vetmedika.hr",
                Phone = "013456789",
                Address = "Ilica 200",
                City = "Zagreb",
                Notes = "Osigurava popuste na veterinarske usluge.",
                CreatedAt = new DateTime(2026, 1, 8),
                DonorTypeId = 2,          // Tvrtka
                DonorStatusId = 1         // Novi
            },
            new Donor
            {
                FirstName = "",
                LastName = "",
                OrganizationName = "Udruga Šapa prijateljstva",
                Email = "kontakt@sapaprijateljstva.hr",
                Phone = "0915566778",
                Address = "Trg slobode 8",
                City = "Rijeka",
                Notes = "Organizira akcije prikupljanja donacija.",
                CreatedAt = new DateTime(2025, 12, 1),
                DonorTypeId = 3,          // Udruga ili organizacija
                DonorStatusId = 2         // Aktivan
            }
        );

        await db.SaveChangesAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.Employees.AnyAsync())
    {
        db.Employees.AddRange(
            new Employee
            {
                FirstName = "Petra",
                LastName = "Jurić",
                Email = "petra.juric@azil.hr",
                Phone = "0912223344",
                EmployeeNumber = "EMP-001",
                HireDate = new DateTime(2024, 3, 1),
                Notes = "Voditeljica smjene.",
                EmployeePositionId = 1,   // Djelatnik azila
                EmployeeStatusId = 1      // Aktivan
            },
            new Employee
            {
                FirstName = "Ante",
                LastName = "Perić",
                Email = "ante.peric@azil.hr",
                Phone = "0915556677",
                EmployeeNumber = "EMP-002",
                HireDate = new DateTime(2023, 9, 15),
                Notes = "Zadužen za medicinske preglede.",
                EmployeePositionId = 2,   // Veterinar
                EmployeeStatusId = 1      // Aktivan
            },
            new Employee
            {
                FirstName = "Lucija",
                LastName = "Barišić",
                Email = "lucija.barisic@azil.hr",
                Phone = "0918889900",
                EmployeeNumber = "EMP-003",
                HireDate = new DateTime(2025, 1, 10),
                Notes = "Trenutno na bolovanju.",
                EmployeePositionId = 3,   // Koordinator volontera
                EmployeeStatusId = 2      // Na dopustu ili bolovanju
            },
            new Employee
            {
                FirstName = "Ivan",
                LastName = "Šimić",
                Email = "ivan.simic@azil.hr",
                Phone = "0911112233",
                EmployeeNumber = "EMP-004",
                HireDate = new DateTime(2022, 6, 20),
                Notes = "Vodi administraciju i evidenciju.",
                EmployeePositionId = 4,   // Administrator
                EmployeeStatusId = 1      // Aktivan
            },
            new Employee
            {
                FirstName = "Maja",
                LastName = "Kovač",
                Email = "maja.kovac@azil.hr",
                Phone = "0914445566",
                EmployeeNumber = "EMP-005",
                HireDate = new DateTime(2021, 11, 5),
                Notes = "Više ne radi u azilu.",
                EmployeePositionId = 1,   // Djelatnik azila
                EmployeeStatusId = 3      // Neaktivan
            }
        );

        await db.SaveChangesAsync();
    }

    await AppUserSeeder.SeedAsync(db);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.Donations.AnyAsync())
    {
        db.Donations.AddRange(
            new Donation
            {
                DonorId = 1, DonationTypeId = 1, DonationStatusId = 2,
                DonationDate = DateTime.Today.AddDays(-5),
                Amount = 150.00m,
                Notes = "Mjesečna novčana donacija."
            },
            new Donation
            {
                DonorId = 1, DonationTypeId = 2, DonationStatusId = 1,
                DonationDate = DateTime.Today.AddDays(-2),
                ItemName = "Vreća suhe hrane 15 kg", Quantity = 4, EstimatedValue = 80.00m,
                Notes = "Dostavljeno u skladište."
            },
            new Donation
            {
                DonorId = 3, DonationTypeId = 2, DonationStatusId = 3,
                DonationDate = DateTime.Today.AddDays(-20),
                ItemName = "Pijesak za mačke", Quantity = 10, EstimatedValue = 60.00m,
                Notes = "Redovita mjesečna donacija tvrtke."
            },
            new Donation
            {
                DonorId = 2, DonationTypeId = 1, DonationStatusId = 2,
                DonationDate = DateTime.Today.AddDays(-10),
                Amount = 50.00m,
                Notes = "Jednokratna donacija."
            },
            new Donation
            {
                DonorId = 5, DonationTypeId = 3, DonationStatusId = 1,
                DonationDate = DateTime.Today.AddDays(-1),
                ItemName = "Transporteri za životinje", Quantity = 3, EstimatedValue = 120.00m,
                Notes = "Za prijevoz kod veterinara."
            },
            new Donation
            {
                DonorId = 4, DonationTypeId = 5, DonationStatusId = 2,
                DonationDate = DateTime.Today.AddDays(-7),
                ItemName = "Veterinarski pregledi", Quantity = 5, EstimatedValue = 200.00m,
                Notes = "Donirane usluge pregleda."
            }
        );

        await db.SaveChangesAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();
    await db.Database.MigrateAsync();

    if (!await db.VolunteerTasks.AnyAsync())
    {
        db.VolunteerTasks.AddRange(
            new VolunteerTask
            {
                Title = "Jutarnja šetnja Lune",
                Description = "Šetnja i osnovna poslušnost.",
                DueDate = DateTime.Today.AddDays(2),
                VolunteerId = 1, AnimalId = 1,
                VolunteerTaskStatusId = 3, VolunteerTaskTypeId = 1
            },
            new VolunteerTask
            {
                Title = "Hranjenje Rexa",
                Description = "Rok je prošao, treba obaviti.",
                DueDate = DateTime.Today.AddDays(-1),
                VolunteerId = 1, AnimalId = 3,
                VolunteerTaskStatusId = 1, VolunteerTaskTypeId = 2
            },
            new VolunteerTask
            {
                Title = "Socijalizacija mačke Maze",
                Description = "Naviknuti na kontakt s ljudima.",
                DueDate = DateTime.Today.AddDays(5),
                VolunteerId = 2, AnimalId = 2,
                VolunteerTaskStatusId = 2, VolunteerTaskTypeId = 4
            },
            new VolunteerTask
            {
                Title = "Ažuriranje evidencije udomljavanja",
                Description = "Administrativni zadatak, rok probijen.",
                DueDate = DateTime.Today.AddDays(-3),
                VolunteerId = 2, AnimalId = null,
                VolunteerTaskStatusId = 1, VolunteerTaskTypeId = 6
            },
            new VolunteerTask
            {
                Title = "Čišćenje mačje sobe",
                Description = "Redovito tjedno čišćenje.",
                DueDate = DateTime.Today.AddDays(-4),
                CompletedAt = DateTime.Today.AddDays(-2),
                VolunteerId = 3, AnimalId = 4,
                VolunteerTaskStatusId = 4, VolunteerTaskTypeId = 3
            },
            new VolunteerTask
            {
                Title = "Prijevoz Tobija veterinaru",
                Description = "Otkazano zbog pomaka termina.",
                DueDate = DateTime.Today.AddDays(-6),
                VolunteerId = 4, AnimalId = 5,
                VolunteerTaskStatusId = 5, VolunteerTaskTypeId = 5
            }
        );

        await db.SaveChangesAsync();
    }
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
