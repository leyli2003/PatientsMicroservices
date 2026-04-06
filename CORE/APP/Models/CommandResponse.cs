using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.APP.Models
{
    //CommandResponse cr = new CommandResponse(); //false, null
    public class CommandResponse: Response //insert, update, delete
    {
        public bool? IsSuccessful { get; set; }
        public string Message { get; set; }
        public CommandResponse(bool isSuccessful, string message = "", int id = 0): base (id) 
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}
