using HealthCare4All.Classes.Users;
using HealthCare4All.Data;
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

app.MapPost("/GetAppointments", (AuthToken token, Healthcare4AllDbContext newNealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newNealthcare4AllDbContext);

    return user.GetProfile();
}); 




app.Run();