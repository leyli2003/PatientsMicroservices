using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Domain
{
    public class Doctor : Entity
    {
        public int BranchId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        public bool IsExpert { get; set; }

        public int? UserId { get; set; }

        // navigation property
        public Branch Branch { get; set; }

        public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
    }
}