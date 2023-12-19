using Moq;
using voto.Entities;
using voto.Exceptions;
using voto.ExternalServices.Interfaces;
using voto.Repository.Interfaces;
using voto.Services;

namespace Voto.Test;

[TestFixture]
public class CrearVotoTest
{

    private readonly Mock<IVotanteService> mockVotanteService = new Mock<IVotanteService>();
    private readonly Mock<ICandidatoService> mockCandidatoService = new Mock<ICandidatoService>();
    private readonly Mock<IVotoRepository> mockVotoRepository = new Mock<IVotoRepository>();
    private readonly Votante votanteMenorDeEdad = new Votante() 
    {
        FechaNacimiento = "2019-05-09",
        Id = 1
    };
    private readonly Votante votanteMayorDeEdad = new Votante() 
    {
        FechaNacimiento = "1989-05-09",
        Id = 2
    };
    private readonly voto.Context.Voto votoDeMayorDeEdad = new voto.Context.Voto() 
    {
        CandidatoId = 1,
        VotanteId = 3
    };
    private readonly voto.Context.Voto votoDeMenorDeEdad = new voto.Context.Voto() 
    {
        CandidatoId = 2,
        VotanteId = 1
    };
    private readonly int VotoCreado = 23;

    [SetUp]
    public void Setup()
    {            
        mockVotanteService.Setup(vs =>  vs.Obtener(1)).ReturnsAsync(votanteMenorDeEdad);
        mockVotanteService.Setup(vs =>  vs.Obtener(3)).ReturnsAsync(votanteMayorDeEdad);
        mockVotoRepository.Setup(vr => vr.Crear(votoDeMayorDeEdad)).ReturnsAsync(VotoCreado);

    }

    [Test]
    public async Task CrearConVotoVotanteMayorDeEdadDaOk()
    {
        var servicioVoto = new VotoService(mockVotanteService.Object, mockCandidatoService.Object, mockVotoRepository.Object);
        var respuesta = await servicioVoto.Crear(votoDeMayorDeEdad);
        Assert.That(respuesta, Is.EqualTo(VotoCreado));
    }

    [Test]
    public async Task CrearVotoConVotanteMenorDeEdadDaError()
    {
        var servicioVoto = new VotoService(mockVotanteService.Object, mockCandidatoService.Object, mockVotoRepository.Object);
        var respuesta = await servicioVoto.Crear(votoDeMayorDeEdad);
        Assert.ThrowsAsync<CustomException>(async () => await servicioVoto.Crear(votoDeMenorDeEdad));
    }

}