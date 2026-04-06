using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Domain
{
    public class Patient : Entity
    {
        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public int? UserId { get; set; }

        public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
    }
}