using HealthCare4All.Classes.Users;
using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using HealthCare4All.Data.HTTP.ServerInput;
using HealthCare4All.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Runtime.CompilerServices;
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

app.MapPost("/GetProfile", (AuthToken token, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newHealthcare4AllDbContext);

    return Results.Ok(user.GetProfile());
});

app.MapPost("/GetAllAppointments", (string? userName, AuthToken token, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.Ok(patient.GetAppointments());
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;

        if (userName == null) {
            return Results.BadRequest();
            //return new List<ApiAppointment>();
        }

        return Results.Ok(provider.GetAppointments(userName));
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/AddAppointment", (ApiAppointmentWithAuthToken apiAppointmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    //AuthToken token = new AuthToken();

    User user = UserFactory.Create(apiAppointmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.AddAppointment(apiAppointmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/EditAppointment", (ApiAppointmentWithAuthToken apiAppointmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    //AuthToken token = new AuthToken();

    User user = UserFactory.Create(apiAppointmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.EditAppointment(apiAppointmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/RemoveAppointment", (ApiAppointmentWithAuthToken apiAppointmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    //AuthToken token = new AuthToken();

    User user = UserFactory.Create(apiAppointmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.RemoveAppointment(apiAppointmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/GetAllTreatments", (string? userName, AuthToken token, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;

    if (user is Patient) {
        patient = (Patient)user;

        return Results.Ok(patient.GetTreatments());
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;

        if (userName == null) {
            return Results.BadRequest();
           
        }

        return Results.Ok(provider.GetTreatments(userName));
    } else {
        return Results.BadRequest();   
    }
});

app.MapPost("/AddTreatment", (ApiTreatmentWithAuthToken apiTreatmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(apiTreatmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.AddTreatment(apiTreatmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/EditTreatment", (ApiTreatmentWithAuthToken apiTreatmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(apiTreatmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.EditTreatment(apiTreatmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/RemoveTreatment", (ApiTreatmentWithAuthToken apiTreatmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    User user = UserFactory.Create(apiTreatmentWithAuthToken.Token, newHealthcare4AllDbContext);

    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.RemoveTreatment(apiTreatmentWithAuthToken);

        return Results.Accepted();
    } else {
        return Results.BadRequest();
    }
});


app.MapPost("/AddUser", (UserInfo newUserInfo, Healthcare4AllDbContext newHealthcare4AllDbContext) => {

    try {
        newHealthcare4AllDbContext.UserInfos.Add(newUserInfo);
        newHealthcare4AllDbContext.SaveChanges();
    } catch (Exception e) {
        return Results.BadRequest();
    }

    return Results.Accepted();
});

app.MapPost("/Login", (UserLogin userLogin, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    var loginQuery = from UserInfo in newHealthcare4AllDbContext.UserInfos
                     where (UserInfo.UserName == userLogin.UserName
                     && UserInfo.Password == userLogin.Password
                     && UserInfo.MaxPriviledge >= userLogin.Privilege)
                     select UserInfo;

    int userCount = loginQuery.Count();

    if (userCount == 1) {
        return new AuthToken {
            UserName = userLogin.UserName,
            Privilege = userLogin.Privilege
        };
    } else {
        return new AuthToken {
            UserName = "",
            Privilege = 0
        };
    }
});




app.Run();