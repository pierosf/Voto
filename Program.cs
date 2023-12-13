using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ServiceReference;
using voto.Context;
using voto.Entities;
using voto.Exceptions;
using voto.ExternalServices;
using voto.ExternalServices.Interfaces;
using voto.Helpers;
using voto.Repository;
using voto.Repository.Interfaces;
using voto.Services;
using voto.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var connection = new SqliteConnection("DataSource=votos.db");
connection.Open();
builder.Services.AddDbContext<VotoDb>(opt => opt.UseSqlite(connection));

builder.Services.AddControllers();

builder.Services.AddScoped<IVotoRepository,VotoRepository>();
builder.Services.AddScoped<IVotoService, VotoService>();
builder.Services.AddScoped<IVotanteService, VotanteService>();
builder.Services.AddScoped<ICandidatoService, CandidatoService>();

builder.Services.AddHttpClient();


var app = builder.Build();


// app.MapGet("/votos", async (VotoDb _db) => {
//     Stopwatch reloj = new();
//     reloj.Start();
//     var resultadosVotaciones = new List<Resultado>();
//     var votos = _db.Votos.ToList();
//     resultadosVotaciones = votos.GroupBy(v => v.CandidatoId).Select(v => new Resultado(){CandidatoId = v.Key, Votos = v.Count(), Candidato = string.Empty}).ToList();
//     List<Task<HttpResponseMessage>> consultas = new();
//     foreach (var resultado in resultadosVotaciones)
//     {
//         if (resultado.CandidatoId.HasValue) 
//         {
//             var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"http://localhost:5117/candidatos/{resultado.CandidatoId.Value}"));
//             consultas.Add(client.SendAsync(request, CancellationToken.None));
//         }
//     }
//     await Task.WhenAll(consultas);
//     List<Candidato> candidatosDesdeApi = new();
//     foreach (var consulta in consultas)
//     {
//         consulta.Result.EnsureSuccessStatusCode();
//         candidatosDesdeApi.Add(JsonSerializer.Deserialize<Candidato>(await consulta.Result.Content.ReadAsStringAsync()));
//     }
//     foreach (var resultado in resultadosVotaciones)
//     {
//         if (resultado.CandidatoId.HasValue)
//         {
//             var candidato = candidatosDesdeApi.FirstOrDefault( c => c.Id == resultado.CandidatoId.Value);
//             resultado.Candidato = $"{candidato.Nombre} {candidato.Apellido}";
//         }
//         else
//         {
//             resultado.Candidato = "Nulos";
//         }
//     }
//     return TypedResults.Ok(resultadosVotaciones);
// });
// app.MapPost("/votos", async ([FromBody] Voto _nuevoVoto, VotoDb _db) => {
    
//     DateTime FechaNacimiento = DateTime.ParseExact(votante.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture);

//     var soapServiceChannel = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
//     var respuesta = await soapServiceChannel.SubtractAsync(DateTime.Now.Year, FechaNacimiento.Year);

//     if (respuesta > 16)
//     {
//         _db.Votos.Add(_nuevoVoto);
//         _db.SaveChanges();
//         return TypedResults.Ok(_nuevoVoto.Id);
//     }
//     else 
//     {
//         throw new CustomException("El votante no tiene la edad para votar", 500);
//     }
// });

app.MapControllers();

app.Use(async (context, next) => {
    using (var _db = context.RequestServices.GetRequiredService<VotoDb>())
    {
       _db.Logs.Add(new Log() {
            Message = $"{context.Request.Path} - {context.Request.Method}",
            Date = DateTime.Now,
            Type = TypeEnum.REQUEST
        });
        await next.Invoke();
        _db.Logs.Add(new Log() {
            Message = $"{context.Request.Path} - {context.Request.Method} - {context.Response.StatusCode}",
            Date = DateTime.Now,
            Type = context.Response.StatusCode == 500 ? TypeEnum.ERROR : TypeEnum.RESPONSE
        });
        _db.SaveChanges(); 
    }
});

app.Use(async (context, next) => {
    try 
    {
        await next.Invoke();
    }
    catch(CustomException ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new ErrorEntity() {Message = ex.Message, StatusCode = ex.StatusCode});
    }
});


app.Run();