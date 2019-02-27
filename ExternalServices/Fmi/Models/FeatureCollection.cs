using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ExternalServices.Fmi.Models
{
    [XmlRoot("FeatureCollection", Namespace = "http://www.opengis.net/wfs/2.0",
        IsNullable = false)]
    public class FeatureCollection
    {
        [XmlElement(ElementName = "member", Namespace = "http://www.opengis.net/wfs/2.0")]
        public List<Member> Members { get; set; }
    }
    
    public class Member
    {
        [XmlElement(Namespace = "http://inspire.ec.europa.eu/schemas/omso/3.0")]
        public PointTimeSeriesObservation PointTimeSeriesObservation  { get; set; }
        [XmlElement(Namespace = "http://xml.fmi.fi/schema/wfs/2.0")]
        public BsWfsElement BsWfsElement { get; set; }
    }

    public class BsWfsElement
    {
        [XmlElement(Namespace = "http://xml.fmi.fi/schema/wfs/2.0")]
        public Location Location { get; set; }

        public DateTime Time { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
    }

    public class Location
    {
        [XmlElement("Point", Namespace = "http://www.opengis.net/gml/3.2")]
        public Point Point { get; set; }
    }

    public class PointTimeSeriesObservation
    {
        [XmlElement("result", Namespace = "http://www.opengis.net/om/2.0")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [XmlElement("MeasurementTimeseries", Namespace = "http://www.opengis.net/waterml/2.0")]
        public MeasurementTimeseries MeasurementTimeseries { get; set; }
    }

    public class MeasurementTimeseries
    {
        [XmlElement("point")]
        public Point Point { get; set; }
    }

    public class Point
    {
        [XmlElement("MeasurementTVP", Namespace = "http://www.opengis.net/waterml/2.0")]
        public MeasurementTVP[] MeasurementTVP { get; set; }

        [XmlElement("pos")]
        public string Pos { get; set; }
    }

    
    public class MeasurementTVP
    {
        [XmlElement("time")]
        public DateTime Time { get; set; }
        [XmlElement("value")]
        public double Value { get; set; }
    }
}
