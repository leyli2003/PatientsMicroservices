using CORE.APP.Models;
using CORE.APP.Services;
using Patients.APP.Features.Doctors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Patients.APP.Domain;


namespace Patients.APP.Features.Patients
{
    public class PatientQueryRequest : Request, IRequest<IQueryable<PatientQueryResponse>>
    {
    }

    public class PatientQueryResponse : Response
    {
        // entity properties
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int? UserId { get; set; }
        public List<int> DoctorIds { get; set; }

        // custom properties
        public string FullName { get; set; }
        public string GenderF { get; set; }
        public string HeightF { get; set; }
        public string WeightF { get; set; }
        public string BirthDateF { get; set; }

        public string DoctorsF { get; set; }
        public List<DoctorsQueryResponse> Doctors { get; set; }
    }

    public class PatientsQueryHandler : Service<Patient>, IRequestHandler<PatientQueryRequest, IQueryable<PatientQueryResponse>>
    {
        public PatientsQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Patient> DbSet()
        {
            return base.DbSet()
                .Include(p => p.DoctorPatients)
                    .ThenInclude(dp => dp.Doctor)
                .OrderBy(p => p.FirstName)
                .ThenBy(p => p.LastName);
        }

        public Task<IQueryable<PatientQueryResponse>> Handle(PatientQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbSet().Select(p => new PatientQueryResponse
            {
                Id = p.Id,
                Height = p.Height,
                Weight = p.Weight,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = (int)p.Gender,
                BirthDate = p.BirthDate,
                UserId = p.UserId,
                DoctorIds = p.DoctorPatients.Select(dp => dp.DoctorId).ToList(),

                FullName = p.FirstName + " " + p.LastName,
                GenderF = p.Gender.ToString(),
                HeightF = p.Height.HasValue ? p.Height.Value.ToString() + " cm" : string.Empty,
                WeightF = p.Weight.HasValue ? p.Weight.Value.ToString() + " kg" : string.Empty,
                BirthDateF = p.BirthDate.ToString("MM/dd/yyyy HH:mm:ss"),

                DoctorsF = string.Join(", ", p.DoctorPatients.Select(dp => dp.Doctor.FirstName + " " + dp.Doctor.LastName)),

                Doctors = p.DoctorPatients.Select(dp => new DoctorsQueryResponse
                {
                    Id = dp.Doctor.Id,
                    FirstName = dp.Doctor.FirstName,
                    LastName = dp.Doctor.LastName
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}