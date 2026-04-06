using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Branches
{
    public class BranchQueryRequest : Request, IRequest<IQueryable<BranchQueryResponse>>
    {
    }

    public class BranchQueryResponse : Response
    {
        public string Title { get; set; }

        public List<int> DoctorIds { get; set; }
    }

    public class BranchQueryHandler : Service<Branch>, IRequestHandler<BranchQueryRequest, IQueryable<BranchQueryResponse>>
    {
        public BranchQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Branch> DbSet()
        {
            return base.DbSet()
                .Include(b => b.Doctors)
                .OrderBy(b => b.Title);
        }

        public Task<IQueryable<BranchQueryResponse>> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(b => new BranchQueryResponse
            {
                Id = b.Id,
                Title = b.Title,
                DoctorIds = b.Doctors.Select(d => d.Id).ToList()
            });

            return Task.FromResult(query);
        }
    }
}