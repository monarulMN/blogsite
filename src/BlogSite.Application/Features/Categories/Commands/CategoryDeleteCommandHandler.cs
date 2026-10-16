using BlogSite.Application.Contracts;
using BlogSite.Domain.Enums;
using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Commands
{
    public class CategoryDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) : 
        ICommandHandler<CategoryDeleteCommand, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Guid> Handle(CategoryDeleteCommand command, 
            CancellationToken cancellationToken)
        {
            var category = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(command.Id,
                cancellationToken);
            category.Status = Status.Inactive;

            await _applicationUnitOfWork.CategoryRepository.RemoveAsync(category, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return command.Id;
        }
    }
}
