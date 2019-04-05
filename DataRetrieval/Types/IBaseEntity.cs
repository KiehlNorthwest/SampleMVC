using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVC.Data.Entities
{
    public interface IBaseEntity<T>
    {
        T Id { get; set; }
    }
}
