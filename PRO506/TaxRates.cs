using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO506
{
    /// <summary>
    /// Represents threasholds
    /// </summary>
    public class TaxRates
    {
        /// <summary>
        /// Tax value for up to 14000
        /// </summary>
        public double UpTo14000 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Over14000UpTo48000 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Over48000UpTo70000 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Over70000UpTo180000 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Over180000 { get; set; }

    }
}
