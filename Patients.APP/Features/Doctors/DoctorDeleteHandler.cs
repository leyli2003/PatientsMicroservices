using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class DoctorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class DoctorDeleteHandler : Service<Doctor>, IRequestHandler<DoctorDeleteRequest, CommandResponse>
    {
        public DoctorDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Doctor> DbSet()
        {
            return base.DbSet().Include(d => d.DoctorPatients);
        }

        public async Task<CommandResponse> Handle(DoctorDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSet().SingleOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Doctor not found!");

            Delete(entity.DoctorPatients.ToList());
            await DeleteAsync(entity, cancellationToken);
            return Success("Doctor deleted successfully.", entity.Id);
        }
    }
}