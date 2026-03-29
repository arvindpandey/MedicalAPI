using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;


//using MedicalAPI.MedicalEntity;
using MedicalAPI.Miscellaneous;
using MedicalAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MedicalAPI.BusinessLogic
{
    public class PatientBL : IPatientService
    { 
        private readonly MedicalDbContext _context;

        public PatientBL(MedicalDbContext context)
        { 
            _context = context;
        }
        public async Task<IEnumerable<PatientResponseDTO>> GetAllPatientsAsync()
        {
            return await _context.PatientEntries.Include(u=>u.User)
                .Select(p => new PatientResponseDTO
                {
                    PatientID = p.PatientId,
                    PatientName = p.PatientName,
                    PatientGeneder = p.PatientGeneder,
                    Address = p.Address,
                    BloodGroup = p.BloodGroup,
                    Age = p.Age,
                    Weight = Convert.ToDecimal(p.Weight),
                    EntryDate = p.EntryDate.ToString(),
                    CreatedByUser = $"{p.User.UserFirstName} {p.User.UserLastName}"
                })
                .ToListAsync(); 
        }
        public async Task<PatientResponseDTO> GetPatientByIdAsync(int id)
        {
            var P = await _context.PatientEntries.FirstOrDefaultAsync(k => k.PatientId == id);
            if (P == null) return null;
            return new PatientResponseDTO
            {
                PatientID = P.PatientId,
                PatientName = P.PatientName,
                PatientGeneder = P.PatientGeneder,
                BloodGroup = P.BloodGroup,
                EntryDate = P.EntryDate.ToString(),
                CreatedByUser = P.UserId.ToString()
            };

        } 
        public async Task<int> CreatePatientAsync(CreatePatientDTO dto, int userId)
        {
            var GetUserID = await _context.TblUsers.FindAsync(userId);
            

            var patientEntry = new PatientEntry
            {
                PatientName = dto.PatientName,
                PatientGeneder = dto.PatientGeneder,
                Address = dto.Address,
                BloodGroup = dto.BloodGroup,
                Age = dto.Age,
                Weight = dto.Weight,
                UserId = GetUserID.UserId

            };
            _context.AddAsync(patientEntry);
            await _context.SaveChangesAsync();
            return patientEntry.PatientId;
        }
        public async Task UpdatePatientAsync(int id, CreatePatientDTO dto)
        {
            var p = await _context.PatientEntries.FindAsync(id);
            if (p == null) throw new Exception("Patient not found");
            p.PatientName = dto.PatientName; p.PatientGeneder = dto.PatientGeneder;
            p.Address = dto.Address; p.BloodGroup = dto.BloodGroup;
            p.Age = dto.Age; p.Weight = dto.Weight;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var p = await _context.PatientEntries.FindAsync(id);
            if (p == null) throw new Exception("Patient not found");
            _context.PatientEntries.Remove(p);
            await _context.SaveChangesAsync();
        }

    }


}
