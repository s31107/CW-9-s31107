using CW_9_s31107.Controllers;
using CW_9_s31107.Data;
using CW_9_s31107.DTOs;
using CW_9_s31107.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CW_9_s31107_tests_xUnit;

public class PrescriptionsUnitTests
{
    [Fact]
    public async Task AddNewPrescriptionWithAddingPatient()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().ConfigureWarnings(w => w.Ignore(
                InMemoryEventId.TransactionIgnoredWarning)).UseInMemoryDatabase("TestDb").Options;
        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var controller = new PrescriptionsController(new DbService(dbContext));
        
        var prescription = new PrescriptionPostDto
        {
            Date = DateOnly.Parse("2012-12-12"),
            DueDate = DateOnly.Parse("2012-07-08"),
            Medicaments =
            [
                new MedicamentPostDto
                {
                    Description = "Aaa",
                    Dose = 5,
                    IdMedicament = 1
                }
            ],
            Patient = new PatientPostDto
            {
                Birthdate = DateOnly.Parse("1987-11-05"),
                FirstName = "Janusz",
                LastName = "Kowalski",
                IdDoctor = 1,
                IdPatient = 7
            }
        };
        // Act
        var result = await controller.AddPrescription(prescription);
        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        Assert.Contains(dbContext.Patients,
            p => p is { FirstName: "Janusz", LastName: "Kowalski" } &&
                 p.BirthDate == DateOnly.Parse("1987-11-05"));
    }
    
    [Fact]
    public async Task GetPatientTest()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>().ConfigureWarnings(w => w.Ignore(
            InMemoryEventId.TransactionIgnoredWarning)).UseInMemoryDatabase("TestDb").Options;
        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        var controller = new PrescriptionsController(new DbService(dbContext));
        const int patientId = 1;
        // Act
        var result = await controller.GetPatient(patientId);
        // Assert:
        Assert.NotNull(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        var res = okResult.Value as PatientGetWithPrescriptionsDto;
        Assert.NotNull(res);
        Assert.IsType<OkObjectResult>(result);
        Assert.True(res is { FirstName: "Zbigniew", LastName: "Wiśniewski" } 
                    && res.BirthDate == DateOnly.Parse("1987-11-05"));
    }
}