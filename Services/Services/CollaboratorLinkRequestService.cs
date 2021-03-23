using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    ///     This is the interface of the collaborator link request
    /// </summary>
    public interface ICollaboratorLinkRequestService : IService<CollaboratorLinkRequest>
    {
        Task<CollaboratorLinkRequest> RegisterCollaboratorLinkRequest(Collaborator collaborator);
    }

    /// <summary>
    ///     This is the implementation of the collaborator link request
    /// </summary>
    public class CollaboratorLinkRequestService : Service<CollaboratorLinkRequest>, ICollaboratorLinkRequestService
    {
        /// <summary>
        ///     This is the collaborator link request service constructor
        /// </summary>
        /// <param name="repository"></param>
        public CollaboratorLinkRequestService(ICollaboratorLinkRequestRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new ICollaboratorLinkRequestRepository Repository => (ICollaboratorLinkRequestRepository) base.Repository;

        public async Task<CollaboratorLinkRequest> RegisterCollaboratorLinkRequest(Collaborator collaborator)
        {
            string newHash = Guid.NewGuid().ToString();

            CollaboratorLinkRequest collaboratorLinkRequest = new CollaboratorLinkRequest
            {
                Collaborator = collaborator,
                RequestHash = newHash
            };

            await Repository.AddAsync(collaboratorLinkRequest);

            return collaboratorLinkRequest;
        }
    }
}
