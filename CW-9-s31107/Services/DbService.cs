using CW_9_s31107.Data;
using CW_9_s31107.DTOs;
using CW_9_s31107.Exceptions;
using CW_9_s31107.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_9_s31107.Services;

public interface IDbService
{
    public Task<PrescriptionGetDto> CreatePrescription(PrescriptionPostDto prescriptionDto);
    public Task<PatientGetWithPrescriptionsDto> GetPatient(int id);
}

public class DbService(AppDbContext data) : IDbService
{
    private async Task<Patient> CreatePatientIfNotExists(int idPatient, string firstName, string lastName, 
        DateOnly birthDate)
    {
        var patient = await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == idPatient);
        if (patient is not null) return patient;
        patient = new Patient
        {
            BirthDate = birthDate,
            FirstName = firstName,
            LastName = lastName
        };
        await data.Patients.AddAsync(patient);
        return patient;
    }

    private async Task<Doctor> GetDoctor(int doctorId)
    {
        var doctor = await data.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == doctorId);
        if (doctor is null)
        {
            throw new NotFoundException($"Doctor with id: {doctorId} not found!");
        }
        return doctor;
    }

    private async Task<Medicament> GetMedicament(int medicamentId)
    {
        var medicament = await data.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicamentId);
        if (medicament is null)
        {
            throw new NotFoundException($"Medicament with id {medicamentId} not found!");
        }
        return medicament;
    }
    
    public async Task<PrescriptionGetDto> CreatePrescription(PrescriptionPostDto prescriptionDto)
    {
        await using var transaction = await data.Database.BeginTransactionAsync();
        if (prescriptionDto.DueDate >= prescriptionDto.Date)
        {
            throw new PrescriptionDateException("Date is lower than Due date!");
        }

        if (prescriptionDto.Medicaments.DistinctBy(med => med.IdMedicament).Count() > 10)
        {
            throw new ExceededNumberOfMedicaments("Number of medicaments exceeded 10!");
        }
        try
        {
            // Adding patient:
            var patient = await CreatePatientIfNotExists(prescriptionDto.Patient.IdPatient,
                prescriptionDto.Patient.FirstName, prescriptionDto.Patient.LastName, prescriptionDto.Patient.Birthdate);
            // Getting doctor:
            var doctor = await GetDoctor(prescriptionDto.Patient.IdDoctor);
            // Creating prescription:
            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                Patient = patient,
                Doctor = doctor
            };
            await data.Prescriptions.AddAsync(prescription);
            
            var dtoMedicaments = new List<MedicamentGetDto>();
            foreach (var medicamentDto in prescriptionDto.Medicaments)
            {
                var medicament = await GetMedicament(medicamentDto.IdMedicament);
                var prescriptionMedicament = new PrescriptionMedicament
                {
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Description,
                    Prescription = prescription,
                    Medicament = medicament
                };
                dtoMedicaments.Add(new MedicamentGetDto
                {
                    Description = medicament.Description,
                    IdMedicament = medicament.IdMedicament,
                    Name = medicament.Name,
                    Type = medicament.Type,
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Description
                });
                await data.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
            }
            await data.SaveChangesAsync();
            return new PrescriptionGetDto
            {
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Doctor = new DoctorGetDto
                {
                    Email = doctor.Email,
                    FirstName = doctor.FirstName,
                    IdDoctor = doctor.IdDoctor,
                    LastName = doctor.LastName
                },
                IdPrescription = prescription.IdPrescription,
                Medicaments = dtoMedicaments,
                Patient = new PatientGetDto
                {
                    BirthDate = patient.BirthDate,
                    FirstName = patient.FirstName,
                    IdPatient = patient.IdPatient,
                    LastName = patient.LastName
                }
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.CommitAsync();
        }
    }

    public async Task<PatientGetWithPrescriptionsDto> GetPatient(int id)
    {
        var patient = await data.Patients.FirstOrDefaultAsync(p => p.IdPatient == id);
        if (patient is null)
        {
            throw new NotFoundException($"Patient with id {id} not found!");
        }

        var prescriptions = await data.Prescriptions
            .Where(p => p.IdPatient == id).OrderBy(p => p.DueDate).ToListAsync();
        var prescriptionDtos = new List<PrescriptionGetDetailedDto>();
        
        foreach (var p in prescriptions)
        {
            var doctor = await data.Doctors
                .Where(d => d.IdDoctor == p.IdDoctor)
                .Select(d => new DoctorGetDetailedDto
                {
                    IdDoctor = d.IdDoctor,
                    FirstName = d.FirstName
                })
                .FirstAsync();
            var medicamentLinks = await data.PrescriptionMedicaments
                .Where(m => m.IdPrescription == p.IdPrescription)
                .ToListAsync();
            var medicamentDtos = new List<MedicamentGetDetailedDto>();

            foreach (var m in medicamentLinks)
            {
                var name = await data.Medicaments
                    .Where(mm => mm.IdMedicament == m.IdMedicament)
                    .Select(mm => mm.Name)
                    .FirstAsync();
                
                medicamentDtos.Add(new MedicamentGetDetailedDto
                {
                    IdMedicament = m.IdMedicament,
                    Description = m.Details,
                    Dose = m.Dose,
                    Name = name
                });
            }
            prescriptionDtos.Add(new PrescriptionGetDetailedDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = doctor,
                Medicaments = medicamentDtos
            });
        }
        return new PatientGetWithPrescriptionsDto
        {
            IdPatient = id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescriptionDtos
        };
    }
}