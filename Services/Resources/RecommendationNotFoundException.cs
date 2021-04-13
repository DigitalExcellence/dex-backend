using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Resources
{
    public class RecommendationNotFoundException : Exception
    {
        
        public RecommendationNotFoundException(string error) : base("Something went wrong while finding recommendations: " + error) { }

    }
}
