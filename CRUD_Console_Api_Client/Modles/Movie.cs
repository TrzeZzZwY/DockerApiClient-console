using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Console_Api_Client.Modles
{
    public class Movie
    {
        public int Id { get; set; }
        required public string Title { get; set; }
        required public Director Director { get; set; }
        public DateTime? Date { get; set; }
    }
}
