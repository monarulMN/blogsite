using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Reports.Commands
{
    public class AddReportCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper) : ICommandHandler<AddReportCommand, Report>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Report> Handle(AddReportCommand command, CancellationToken cancellationToken)
        {
            var report = _mapper.Map<Report>(command);
            report.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.ReportRepository.AddAsync(report, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return report;
        }
    }
}
