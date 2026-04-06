using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Patients
{
    public class PatientUpdateRequest : Request, IRequest<CommandResponse>
    {
        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public Genders Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int? UserId { get; set; }

        public List<int> DoctorIds { get; set; } = new();
    }

    public class PatientUpdateHandler : Service<Patient>, IRequestHandler<PatientUpdateRequest, CommandResponse>
    {
        public PatientUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Patient> DbSet()
        {
            return base.DbSet().Include(p => p.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(PatientUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(p =>
                p.Id != request.Id &&
                p.FirstName == request.FirstName.Trim() &&
                p.LastName == request.LastName.Trim() &&
                p.BirthDate == request.BirthDate, cancellationToken))
                return Error($"Patient with the same name and birth date exists!");

            var entity = await DbSet().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Patient not found!");

            Delete(entity.DoctorPatients.ToList());

            entity.Height = request.Height;
            entity.Weight = request.Weight;
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.UserId = request.UserId;
            entity.DoctorPatients = request.DoctorIds.Select(doctorId => new DoctorPatient
            {
                DoctorId = doctorId,
                PatientId = entity.Id
            }).ToList();

            await UpdateAsync(entity, cancellationToken);
            return Success($"Patient {request.FirstName.Trim()} {request.LastName.Trim()} updated successfully.", entity.Id);
        }
    }
}