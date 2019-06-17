using EventFlow.Core;

namespace DomainEF.Model.Location
{
    public class LocationId : Identity<LocationId>
    {
        public LocationId(string value) : base(value)
        {
        }
    }
}