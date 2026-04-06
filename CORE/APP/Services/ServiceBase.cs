using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Models;

namespace CORE.APP.Services
{
    //to change regional setting of the application
     public abstract class ServiceBase
    {
        private CultureInfo _cultureInfo;//field
        protected CultureInfo CultureInfo {  
            get {
                return _cultureInfo; 
            }
            set {  
                _cultureInfo = value; 
                Thread.CurrentThread.CurrentCulture = _cultureInfo;
                Thread.CurrentThread.CurrentUICulture = _cultureInfo;
            }
        }
        protected ServiceBase()
        {
            CultureInfo = new CultureInfo("en-US"); //tr-TR
        }
        //if needed, culture can be changed in a child class as below:
        //CultureInfo = new CultureInfo("tr-TR")
        protected CommandResponse Success (string message, int id) => new CommandResponse
            (true, message, id); //behavior
        protected CommandResponse Error (string message) => new CommandResponse(false, message);
    }
}
