using HealthCare4All.Classes.Users;
using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using HealthCare4All.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Healthcare4AllDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/GetProfile", (AuthToken token, Healthcare4AllDbContext newNealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newNealthcare4AllDbContext);

    return user.GetProfile();
});

app.MapPost("/GetAppointments", (string? userName, AuthToken token, Healthcare4AllDbContext newNealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newNealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return patient.GetAppointments();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;

        if (userName == null) {
            userName = "";
        }

        return provider.GetAppointments(userName);
    } else {
        return new List<ApiAppointment>();
    }
});

app.MapPost("/GetAllTreatments", (string ? userName, AuthToken token, Healthcare4AllDbContext newNealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newNealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;

    if (user is Patient) {
        patient = (Patient)user;

        return patient.GetTreatments();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;

        if (userName == null) {
            userName = "";
        }

        return new List<ApiTreatment>();
    } else {
        return new List<ApiTreatment>();
    }
});




app.Run();