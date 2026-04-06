using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class BranchDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class BranchDeleteHandler : Service<Branch>, IRequestHandler<BranchDeleteRequest, CommandResponse>
    {
        public BranchDeleteHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Branch> DbSet()
        {
            return base.DbSet().Include(b => b.Doctors);
        }

        public async Task<CommandResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await DbSet().SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Branch not found!");

            if (entity.Doctors != null && entity.Doctors.Any())
                return Error("Branch cannot be deleted because it has related doctors!");

            await DeleteAsync(entity, cancellationToken);
            return Success("Branch deleted successfully.", entity.Id);
        }
    }
}