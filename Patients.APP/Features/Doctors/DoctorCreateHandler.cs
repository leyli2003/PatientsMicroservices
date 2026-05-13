using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Doctors
{
    public class DoctorCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int BranchId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public bool IsExpert { get; set; }

        public int? UserId { get; set; }

        public List<int> PatientIds { get; set; } = new List<int>();
    }

    public class DoctorCreateHandler : Service<Doctor>, IRequestHandler<DoctorCreateRequest, CommandResponse>
    {
        public DoctorCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorCreateRequest request, CancellationToken cancellationToken)
        {
            var firstName = request.FirstName?.Trim();
            var lastName = request.LastName?.Trim();
            var patientIds = request.PatientIds ?? new List<int>();

            if (!await DbSet<Branch>().AnyAsync(b => b.Id == request.BranchId, cancellationToken))
            {
                return Error("Selected branch was not found.");
            }

            if (patientIds.Any())
            {
                var existingPatientIds = await DbSet<Patient>()
                    .Where(p => patientIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .ToListAsync(cancellationToken);

                var missingPatientIds = patientIds.Except(existingPatientIds).ToList();
                if (missingPatientIds.Any())
                {
                    return Error($"Some patient IDs were not found: {string.Join(", ", missingPatientIds)}");
                }
            }

            if (await DbSet().AnyAsync(d =>
                d.FirstName == firstName &&
                d.LastName == lastName &&
                d.BranchId == request.BranchId, cancellationToken))
            {
                return Error($"Doctor with the same name in the same branch already exists!");
            }

            var entity = new Doctor
            {
                BranchId = request.BranchId,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                IsExpert = request.IsExpert,
                UserId = request.UserId,
                PatientIds = patientIds
            };

            await CreateAsync(entity, cancellationToken);
            return Success($"Doctor {entity.FirstName} {entity.LastName} created successfully.", entity.Id);
        }
    }
}
