using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalServices.Fmi.Repositories
{
    public class FeatureRepository : Repository
    {
        public FeatureRepository(FmiService service) : base(service) { }


        public List<Dictionary<string, Object>> GetFeatures()
        {
            return Service.Get<List<Dictionary<string, Object>>>("/test").Result;
        }
    }
}
