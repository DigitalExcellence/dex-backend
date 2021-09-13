using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public interface IProjectTransferService : IService<ProjectTransferRequest>
    {

    }
    public class ProjectTransferService : Service<ProjectTransferRequest>, IProjectTransferService
    {
        public ProjectTransferService(ProjectTransferRepository repository) :
            base(repository)
        { }
    }


}
