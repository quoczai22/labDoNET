using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07_Bai3_Feedback.Models
{
    public class FeedbackResponse
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string ServiceAnswer { get; set; }
        public string ProductAnswer { get; set; }
        public string AttitudeAnswer { get; set; }
    }
}
