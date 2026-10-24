using BlogSite.Application.Contracts;
using BlogSite.Domain.Entities;
using BlogSite.Domain.Utilities;
using Cortex.Mediator.Commands;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSite.Application.Features.Librarys.Commands
{
    public class AddLibraryCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : ICommandHandler<AddLibraryCommand, Library>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Library> Handle(AddLibraryCommand command, 
            CancellationToken cancellationToken)
        {
            var library = _mapper.Map<Library>(command);
            library.Id = IdentityGenerator.NewSequentialGuid();

            await _applicationUnitOfWork.LibraryRepository.AddAsync(library, cancellationToken);
            await _applicationUnitOfWork.SaveAsync(cancellationToken);

            return library;
        }
    }
}
