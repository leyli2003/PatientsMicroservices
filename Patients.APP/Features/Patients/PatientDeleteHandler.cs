using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Patients
{
    public class PatientDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class PatientDeleteHandler : Service<Patient>, IRequestHandler<PatientDeleteRequest, CommandResponse>
    {
        public PatientDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Patient> DbSet()
        {
            return base.DbSet().Include(p => p.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(PatientDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSet().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Patient not found!");

            Delete(entity.DoctorPatients.ToList());
            await DeleteAsync(entity, cancellationToken);
            return Success("Patient deleted successfully.", entity.Id);
        }
    }
}