using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Commands
{
    public class CategoryUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper) :
        ICommandHandler<CategoryUpdateCommand, Category>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Category> Handle(CategoryUpdateCommand command, 
            CancellationToken cancellationToken)
        {
            var category = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(command.Id, 
                cancellationToken);

            _mapper.Map(command, category);

            await _applicationUnitOfWork.CategoryRepository.EditAsync(category, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return category;
        }
    }
}
