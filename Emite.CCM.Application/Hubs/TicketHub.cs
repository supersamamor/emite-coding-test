using Emite.CCM.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace Emite.CCM.Application.Hubs
{
    [Authorize] 
    public class TicketHub : Hub
    {
        private readonly ApplicationContext _context;

        public TicketHub(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> GetInitialTicketCount()
        {
            return await _context.Ticket.CountAsync();
        }
    }
}
