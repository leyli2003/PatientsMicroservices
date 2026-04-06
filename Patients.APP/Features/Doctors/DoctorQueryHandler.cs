using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;

namespace Patients.APP.Features.Doctors
{
    public class DoctorsQueryRequest : Request, IRequest<IQueryable<DoctorsQueryResponse>>
    {
    }

    public class DoctorsQueryResponse : Response
    {
        public int BranchId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsExpert { get; set; }

        public int? UserId { get; set; }

        public List<int> PatientIds { get; set; }

        public string BranchTitle { get; set; }
    }

    public class DoctorQueryHandler : Service<Doctor>, IRequestHandler<DoctorsQueryRequest, IQueryable<DoctorsQueryResponse>>
    {
        public DoctorQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Doctor> DbSet()
        {
            return base.DbSet()
                .Include(d => d.Branch)
                .Include(d => d.DoctorPatients)
                .OrderBy(d => d.FirstName)
                .ThenBy(d => d.LastName);
        }

        public Task<IQueryable<DoctorsQueryResponse>> Handle(DoctorsQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(d => new DoctorsQueryResponse
            {
                Id = d.Id,
                BranchId = d.BranchId,
                FirstName = d.FirstName,
                LastName = d.LastName,
                IsExpert = d.IsExpert,
                UserId = d.UserId,
                PatientIds = d.DoctorPatients.Select(dp => dp.PatientId).ToList(),
                BranchTitle = d.Branch != null ? d.Branch.Title : string.Empty
            });

            return Task.FromResult(query);
        }
    }
}