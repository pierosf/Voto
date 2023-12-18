using voto.Entities;
using voto.Repository.Interfaces;

namespace Voto.Test;

public class VotanteTests
{

    [SetUp]
    public void Setup()
    {
                
    }

    [Test]
    public void VotanteEsMayorDeEdadDeberiaSerFalso()
    {
        var votanteMenorDeEdad = new Votante(){
            Id = 1,
            FechaNacimiento = "2021-06-26"
        };
        var esMayorDeEdad = votanteMenorDeEdad.EsMayorDeEdad();
        Assert.IsFalse(esMayorDeEdad);
    }

    [Test]
    public void VotanteEsMayorDeEdadDeberiaSerVerdadero()
    {
        var votanteMenorDeEdad = new Votante(){
            Id = 1,
            FechaNacimiento = "1987-06-26"
        };
        var esMayorDeEdad = votanteMenorDeEdad.EsMayorDeEdad();
        Assert.IsTrue(esMayorDeEdad);
    }
}