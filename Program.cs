using HealthCare4All.Classes.Users;
using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using HealthCare4All.Data.HTTP.ServerInput;
using HealthCare4All.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualBasic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
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

SecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes("as4vvalvj0fsddfKEY*&^^&)-pa-alkamogusdjfjkf[+l234lsd;ss;ddkfhjkljhl;;;;;';"));
JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters() {
    ValidateIssuerSigningKey = true,
    RequireSignedTokens = true,
    IssuerSigningKey = securityKey,

    ValidateIssuer = true,
    ValidateAudience = false,
    ValidIssuer = "Healthcare4All",
    ValidateActor = false,
};

ClaimsPrincipal? GetClaimsPrincipleFromEncodedJwt(string encodedJwt) {
    SecurityToken securityToken = new JwtSecurityToken();
    try {
        return jwtSecurityTokenHandler.ValidateToken(encodedJwt, tokenValidationParameters, out securityToken);
    } catch (Exception e) {
        System.Diagnostics.Debug.WriteLine(e);
        return null;
    }
}


 

app.MapPost("/GetProfile", (string encodedJwt, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(encodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);

    Console.WriteLine(user.UserName);

    return Results.Ok(user.GetProfile());
});

app.MapPost("/GetAllAppointments", (string? userName, string encodedJwt, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(encodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    System.Diagnostics.Debug.WriteLine("##########################");
    System.Diagnostics.Debug.WriteLine("HIT");
    System.Diagnostics.Debug.WriteLine("##########################");

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
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
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiAppointmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.AddAppointment(apiAppointmentWithAuthToken);

        return Results.Ok();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/EditAppointment", (ApiAppointmentWithAuthToken apiAppointmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiAppointmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.EditAppointment(apiAppointmentWithAuthToken);

        return Results.Ok();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/RemoveAppointment", (ApiAppointmentWithAuthToken apiAppointmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiAppointmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.RemoveAppointment(apiAppointmentWithAuthToken);

        return Results.Ok();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/GetAllTreatments", (string? userName, string encodedJwt, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(encodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
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
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiTreatmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.AddTreatment(apiTreatmentWithAuthToken);

        return Results.Ok();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/EditTreatment", (ApiTreatmentWithAuthToken apiTreatmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiTreatmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.EditTreatment(apiTreatmentWithAuthToken);

        return Results.Ok();
    } else {
        return Results.BadRequest();
    }
});

app.MapPost("/RemoveTreatment", (ApiTreatmentWithAuthToken apiTreatmentWithAuthToken, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    ClaimsPrincipal? claimsPrincipal = GetClaimsPrincipleFromEncodedJwt(apiTreatmentWithAuthToken.EncodedJwt);

    if (claimsPrincipal == null) {
        return Results.BadRequest();
    }

    User user = UserFactory.Create(claimsPrincipal, newHealthcare4AllDbContext);
    Patient patient;
    HealthcareProvider provider;


    if (user is Patient) {
        patient = (Patient)user;

        return Results.BadRequest();
    } else if (user is HealthcareProvider) {
        provider = (HealthcareProvider)user;
        provider.RemoveTreatment(apiTreatmentWithAuthToken);

        return Results.Ok();
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

    return Results.Ok();
});

app.MapPost("/Login", (UserLogin userLogin, Healthcare4AllDbContext newHealthcare4AllDbContext) => {
    SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();
    securityTokenDescriptor.SigningCredentials = new SigningCredentials(
        securityKey,
        Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

    var loginQuery = from UserInfo in newHealthcare4AllDbContext.UserInfos
                     where (UserInfo.UserName == userLogin.UserName
                     && UserInfo.Password == userLogin.Password
                     && UserInfo.MaxPriviledge >= userLogin.Privilege)
                     select UserInfo;

    int userCount = loginQuery.Count();

    

    jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
    if (userCount == 1) {
        securityTokenDescriptor.Issuer = "HealthCare4All";
        securityTokenDescriptor.Claims = new Dictionary<string, object>();
        securityTokenDescriptor.Claims.Add("UserName", userLogin.UserName);
        securityTokenDescriptor.Claims.Add("Privilege", userLogin.Privilege);
        securityTokenDescriptor.Issuer = "Healthcare4All";
        //return Results.Ok(jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor));
        return Results.Ok(jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptor));
    } else {
        return Results.BadRequest();
    }
});




app.Run();