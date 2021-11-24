using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Core.Areas.Dashboard.Entities;
using StarterApp.Core.Common.Mappings;
using StarterApp.Core.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Dashboard.Commands
{
    public class LogDashboardCommand : IRequest<List<string>>, IMapTo<LogDashboard>
    {
        public string LogDateTimeZone { get; set; }
    }

    public class LogDashboardCommandHandler : BaseHandler<LogDashboard>, IRequestHandler<LogDashboardCommand, List<string>>
    {
        public async Task<List<string>> Handle(LogDashboardCommand request, CancellationToken cancellationToken)
        {
            if (request.LogDateTimeZone != null)
            {
                var entity = new LogDashboard(request.LogDateTimeZone);
                _repository.Insert(entity);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            var result = await _repository.Query
                .OrderByDescending(x => x.Created)
                .Select(x => x.LogDateTimeZone)
                .ToListAsync();
            return result;
        }
    }
}
