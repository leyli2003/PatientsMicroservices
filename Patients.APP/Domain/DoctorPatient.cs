using CORE.APP.Domain;

namespace Patients.APP.Domain
{
    public class DoctorPatient : Entity
    {
        public int DoctorId { get; set; }

        // navigation property
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }

        // navigation property
        public Patient Patient { get; set; }
    }
}