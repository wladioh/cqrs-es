using System;
using Domain.Base.Commands;

namespace Domain.Location.Commands
{
public class CreateLocationCommand : BaseCommand
{
    public readonly int LocationId;
    public readonly string StreetAddress;
    public readonly string City;
    public readonly string State;
    public readonly string PostalCode;

    public CreateLocationCommand(Guid id, int locationId, string streetAddress, string city, string state, string postalCode)
    {
        Id = id;
        LocationId = locationId;
        StreetAddress = streetAddress;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
}
}