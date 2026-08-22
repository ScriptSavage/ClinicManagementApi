using System.Security.Cryptography;
using ApplicationCore.Exceptions;
using ApplicationCore.Helpers.Pagination;
using ApplicationCore.Prescription.Dto;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Doctor;
using Infrastructure.Repositories.Medicine;
using Infrastructure.Repositories.Patient;
using Infrastructure.Repositories.Prescription;
using Infrastructure.Repositories.Visit;

namespace ApplicationCore.Prescription.Service;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository  _prescriptionRepository;
    private readonly IVisitRepository _visitRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository  _doctorRepository;
    private readonly IMedicineRepository _medicineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository, 
        IVisitRepository visitRepository, 
        IPatientRepository patientRepository,
        IDoctorRepository  doctorRepository,
        IMedicineRepository medicineRepository,
        IUnitOfWork unitOfWork)
    {
        _prescriptionRepository = prescriptionRepository;
        _visitRepository = visitRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _medicineRepository = medicineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddNewPrescription(Guid patientId, string userId, PrescriptionDto.Request dto)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient is null)
        {
            throw new DoesNotExistsException("Patient not found");
        }
        

        var doctor = await _doctorRepository.FindDoctorByUserIdAsync(Guid.Parse(userId));

        if (doctor is null)
        {
            throw new DoesNotExistsException("Doctor not found");
        }

        
        var prescription = new Infrastructure.Entities.Prescription
        {
            PrescriptionId = Guid.NewGuid(),
            PatientId = patient.PatientId,
            DoctorId = doctor.DoctorId,
            Code = GenerateRandomCode()
        };


        foreach (var item in dto.Medicines)
        {
            var medicine = await _medicineRepository.GetMedicineAsync(item.MedicineId);

            if (medicine is null)
            {
                throw new DoesNotExistsException("Medicine not found");
            }
            
            prescription.MedicinePrescriptions.Add(new MedicinePrescription()
            {
                Dosage = item.Dosage,
                Frequency = item.Frequency,
                Instructions = item.Instructions,
                Quantity = item.Quantity,
                Medicine = medicine,
            });

        }

        await _prescriptionRepository.AddNewPrescriptionAsync(prescription);

        await _unitOfWork.SaveChangesAsync();
    }
    
    
    
    
    private static string GenerateRandomCode()
    {
        const int loginLength = 10;
        const string chars = "0123456789";

        var characters = new char[loginLength];
        var randomBuffer = new byte[loginLength];
            
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBuffer);
        }

        for (var i = 0; i < loginLength; i++)
        {
            characters[i] = chars[randomBuffer[i] % chars.Length];
        }

        return  new string(characters);
    }
}