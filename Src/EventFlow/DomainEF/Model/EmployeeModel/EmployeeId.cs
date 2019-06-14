using EventFlow.Core;

namespace DomainEF.Model.EmployeeModel
{
    public class EmployeeId : Identity<EmployeeId>
    {
        public EmployeeId(string value) : base(value)
        {
        }
    }
}