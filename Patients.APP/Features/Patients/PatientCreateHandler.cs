using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Patients
{
    public class PatientCreateRequest : Request, IRequest<CommandResponse>
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

        public List<int> DoctorIds { get; set; } = new List<int>();
    }

    public class PatientCreateHandler : Service<Patient>, IRequestHandler<PatientCreateRequest, CommandResponse>
    {
        public PatientCreateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Patient> DbSet()
        {
            return base.DbSet().Include(p => p.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(PatientCreateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(p =>
                p.FirstName == request.FirstName.Trim() &&
                p.LastName == request.LastName.Trim() &&
                p.BirthDate == request.BirthDate, cancellationToken))
                return Error($"Patient with the same name and birth date exists!");

            var entity = new Patient
            {
                Height = request.Height,
                Weight = request.Weight,
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                UserId = request.UserId,
                DoctorPatients = request.DoctorIds.Select(doctorId => new DoctorPatient
                {
                    DoctorId = doctorId
                }).ToList()
            };

            await CreateAsync(entity, cancellationToken);
            return Success($"Patient {request.FirstName.Trim()} {request.LastName.Trim()} created successfully.", entity.Id);
        }
    }
}