using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Reports.Commands
{
    public class ReportDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<ReportDeleteCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<Guid> Handle(ReportDeleteCommand command, CancellationToken cancellationToken)
        {
            var report = await _applicationUnitOfWork.ReportRepository.GetByIdAsync(command.Id, cancellationToken);
            report.Status = Status.Inactive;

            await _applicationUnitOfWork.ReportRepository.EditAsync(report, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.Id;
        }
    }
}
