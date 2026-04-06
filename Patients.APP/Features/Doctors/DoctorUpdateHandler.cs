using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Doctors
{
    public class DoctorUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int BranchId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public bool IsExpert { get; set; }

        public int? UserId { get; set; }

        public List<int> PatientIds { get; set; } = new();
    }

    public class DoctorUpdateHandler : Service<Doctor>, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        public DoctorUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Doctor> DbSet()
        {
            return base.DbSet().Include(d => d.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(d =>
                d.Id != request.Id &&
                d.FirstName == request.FirstName.Trim() &&
                d.LastName == request.LastName.Trim() &&
                d.BranchId == request.BranchId, cancellationToken))
                return Error($"Doctor with the same name in the same branch exists!");

            var entity = await DbSet().SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Doctor not found!");

            Delete(entity.DoctorPatients.ToList());

            entity.BranchId = request.BranchId;
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.IsExpert = request.IsExpert;
            entity.UserId = request.UserId;
            entity.DoctorPatients = request.PatientIds.Select(patientId => new DoctorPatient
            {
                DoctorId = entity.Id,
                PatientId = patientId
            }).ToList();

            await UpdateAsync(entity, cancellationToken);
            return Success($"Doctor {request.FirstName.Trim()} {request.LastName.Trim()} updated successfully.", entity.Id);
        }
    }
}