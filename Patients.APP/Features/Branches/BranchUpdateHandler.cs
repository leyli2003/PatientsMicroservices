using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Branches
{
    public class BranchUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(200)]
        public string Title { get; set; }
    }

    public class BranchUpdateHandler : Service<Branch>, IRequestHandler<BranchUpdateRequest, CommandResponse>
    {
        public BranchUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await DbSet().AnyAsync(b => b.Id != request.Id && b.Title == request.Title.Trim(), cancellationToken))
                return Error($"Branch with the same title: \"{request.Title.Trim()}\" exists!");

            var entity = await DbSet().SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Branch not found!");

            entity.Title = request.Title?.Trim();

            await UpdateAsync(entity, cancellationToken);
            return Success($"Branch with title {request.Title.Trim()} updated successfully.", entity.Id);
        }
    }
}