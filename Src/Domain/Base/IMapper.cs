using System.Threading.Tasks;

namespace Domain.Base
{
    public interface IMapper
    {
        Task<TD> Map<TD>(object message);
    }
}