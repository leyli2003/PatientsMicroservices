using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Patients.APP.Domain
{
    public class Branch : Entity
    {
        [Required, StringLength(200)]
        public string Title { get; set; }

        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}