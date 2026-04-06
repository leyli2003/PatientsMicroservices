using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.APP.Models
{
    public abstract class Response
    {
        public virtual int Id { get; set; }
        protected Response() { }
        public Response(int id) 
        { 
            Id = id;
        }
    }
}
