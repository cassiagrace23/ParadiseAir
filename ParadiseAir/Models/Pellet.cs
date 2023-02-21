using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadiseAir.Models
{
        public class Pellet
        {
            private int PelletKey { get; set; }
            private string Manufacturer { get; set; }
            private string Model { get; set; }
            private decimal Caliber { get; set; }
            private decimal ActualSize { get; set; }
            private decimal Grains { get; set; }

        }

        public class Tin
        {
            private int TinKey { get; set; }
            private int PelletKey { get; set; }
            private int PelletsPerTin { get; set; }
            private int TinCount { get; set; }
            private int OpenCount { get; set; }
            private int PelletTotal { get; set; }
            private int CostPerPellet { get; set; }

        }

}
