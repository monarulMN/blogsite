using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Categories.Commands
{
    public class CategoryAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper) : 
        ICommandHandler<CategoryAddCommand, Category>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Category> Handle(CategoryAddCommand command, 
            CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(command);
            category.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.CategoryRepository.AddAsync(category,cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return category;
        }
    }
}
