using Microsoft.EntityFrameworkCore;

namespace Patients.APP.Domain
{
    public class PatientsDb : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }

        public PatientsDb(DbContextOptions options) : base(options)
        {
        }

    }
}