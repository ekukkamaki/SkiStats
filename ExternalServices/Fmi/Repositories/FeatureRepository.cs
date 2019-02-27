using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ExternalServices.Fmi.Models;

namespace ExternalServices.Fmi.Repositories
{
    public class FeatureRepository : Repository
    {
        public FeatureRepository(FmiService service) : base(service) { }


        public FeatureCollection GetFeatures()
        {
            return Service.Get<FeatureCollection>(@"wfs/fin?service=WFS&version=2.0.0&request=GetFeature&storedquery_id=fmi::observations::weather::timevaluepair&place=Raahe&").Result;
        }

        private FeatureCollection GetSimpleQuery(string city, DateTime startTime, DateTime endTime, string[] returnParameters)
        {
            var request = $@"wfs?service=WFS
                    &version=2.0.0
                    &request=GetFeature
                    &storedquery_id=fmi::observations::weather::daily::simple
                    &place={city}
                    &timestep=3600
                    &starttime={startTime:s}
                    &endtime={endTime:s}
                    &maxlocations=3
                    &parameters={string.Join(",", returnParameters)}&";
            request = Regex.Replace(request, @"\s+", string.Empty);
            return Service.Get<FeatureCollection>(request).Result;
        }

        public IEnumerable<BsWfsElement> GetSnowDepthAndTemperature(string city, DateTime startTime, DateTime endTime)
        {
            var featureColl = GetSimpleQuery(city, startTime, endTime, new[] {"tday", "snow"});
            if (featureColl.Members.Count > 0)
            {
                return featureColl.Members.Select(s => s.BsWfsElement);
            }

            return null;
        }
    }
}
