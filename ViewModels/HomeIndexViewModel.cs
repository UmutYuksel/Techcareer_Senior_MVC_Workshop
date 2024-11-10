using EfCore.Models;
using System.Collections.Generic;

namespace EfCore.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<User>? Users { get; set; }
        public List<Duty>? Duties { get; set; }
    }
}
