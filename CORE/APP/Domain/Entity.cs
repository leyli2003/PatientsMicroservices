using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.APP.Domain
{
    public class Entity
    {
        //this is encapsulation because can access fields through methods so more controlled
        //private int id; //fields

        //public int GetId() //behaviors
        //{
        //    return id;
        //}

        //public void SetId(int id)
        //{
        //    this.id = id;
        //}
        public int Id { get; set; } //prop top(shortcut) - properties
    }
}
