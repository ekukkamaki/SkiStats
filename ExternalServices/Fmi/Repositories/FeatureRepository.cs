using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.Fmi.Models;

namespace ExternalServices.Fmi.Repositories
{
    public class FeatureRepository : Repository
    {
        public FeatureRepository(FmiService service) : base(service) { }


        public string GetFeatures()
        {
            return Service.Get<string>(@"wfs/fin?service=WFS&version=2.0.0&request=GetFeature&storedquery_id=fmi::observations::weather::timevaluepair&place=Raahe&").Result;
        }
    }
}
