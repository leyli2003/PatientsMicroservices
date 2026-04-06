using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Features.Branches
{
    public class BranchCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(200)]
        public string Title { get; set; }
    }

    public class BranchCreateHandler : Service<Branch>, IRequestHandler<BranchCreateRequest, CommandResponse>
    {
        public BranchCreateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var trimmedTitle = request.Title?.Trim();

            if (await DbSet().AnyAsync(b => b.Title == trimmedTitle, cancellationToken))
                return Error($"Branch with title \"{trimmedTitle}\" already exists!");

            var entity = new Branch
            {
                Title = trimmedTitle
            };

            await CreateAsync(entity, cancellationToken);
            return Success($"Branch with title {trimmedTitle} created successfully.", entity.Id);
        }
    }
}