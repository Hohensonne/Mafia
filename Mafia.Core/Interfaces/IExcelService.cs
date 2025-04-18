using Mafia.Core.Models;

namespace Mafia.Core.Interfaces
{
    public interface IExcelService
    {
        Task SaveDeliveredOrderToExcel(Order order);
    }
} 