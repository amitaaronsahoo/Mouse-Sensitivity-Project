using System.Linq;

//need to make a object of type web application
var builder = WebApplication.CreateBuilder(args);

//have to configure this builder object before building said object
builder.Services.AddCors(options =>{
    options.AddPolicy("AllowAll", policy =>{policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();});
});
//this is just the configuration I need for the api object named builder

//DONT CURRENTLY HAVE A STATIC WEBSITE SO WILL ALLOW ALL ORIGINS

//added CORS configuration to let the frontend access the API that I am creating
//also had to add the policies to make it so that the api can accept all requests, allow any of the GET POST PUT DELETE, and headers for the front end javascript

var app = builder.Build();
//named app and had app build the api called builder which i made above

app.UseCors("AllowAll");
//this is the configuration that i just made for the app object named app to use the policy i made for the builder object named builder


// The "database" I had to fake since for this project an actual database is out of the scope of the project
//THE API will read and write to "profiles" object that i made below
var profiles = new List<SensitivityProfile>
{
    new SensitivityProfile
    {
        Id = 1,
        GameName = "Valorant",
        FieldOfView = 103,
        MouseDPI = 800,
        InGameSensitivity = 0.35,
        CmPer360 = 34.2,
        Notes = "Default crosshair, claw grip"
    },
    new SensitivityProfile
    {
        Id = 2,
        GameName = "Apex Legends",
        FieldOfView = 110,
        MouseDPI = 1600,
        InGameSensitivity = 1.5,
        CmPer360 = 28.7,
        Notes = "ADS sensitivity matched to hipfire"
    }
};

// This is just a variable that tracks the next Id to hand out and starts right after the initial entries
var nextId = profiles.Count + 1;

// THIS IS WHAT HAPPENS WHEN SOMETHING SENDS POST
app.MapPost("/api/profiles", (SensitivityProfile input) =>  //git 
{
    var profile = new SensitivityProfile
    {
        Id = nextId++,
        GameName = input.GameName,
        FieldOfView = input.FieldOfView,
        MouseDPI = input.MouseDPI,
        InGameSensitivity = input.InGameSensitivity,
        CmPer360 = input.CmPer360,
        Notes = input.Notes
    };

    profiles.Add(profile);

    // 201 Created + Location header pointing at GET /api/profiles/{id}.
    return Results.Created($"/api/profiles/{profile.Id}", profile);
});



// the "profiles" list is full of these objects
// Public so the xUnit test project can work with this
public class SensitivityProfile
{
    public int Id { get; set; }
    public string GameName { get; set; } = string.Empty;
    public int FieldOfView { get; set; }
    public int MouseDPI { get; set; }
    public double InGameSensitivity { get; set; }
    public double CmPer360 { get; set; }
    public string? Notes { get; set; }
}



