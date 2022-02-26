using Core.Access.Identity.DB;
using Core.Access.Identity.DB.EntitySets;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public class ClientGenerationStrategy : AbstractStrategy<ClientModel, OnClientGenerationContext>
    {
        public ClientGenerationStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities) : base(repository, userManager, utilities)
        {
        }

        protected override async Task<bool> Validate()
        {
            if (Repository.FetchClientByFriendlyName(Model.Name).Evaluate(out Client clientEntity,
                OnInternalFailure: (op) => throw op.Exception,
                OnEntityNotFound: (op) =>
                {
                }))
            {
                Model.Errors.Add(Resource.ClientAlreadyRegistered);
                Result = new ViewableStrategyResult(Model);


                return await Task.FromResult(false);
            }


            return await Task.FromResult(true);
        }

        protected override async Task<StrategyResult> Process()
        {
            var client = new Client()
            {
                FriendlyName = Model.Name
            };

            client.ClientId = Guid.NewGuid().ToString();
            client.Password = Utilities.EncryptionHelper.EncryptString(Client.Concatenate(client.ClientId, client.FriendlyName, Model.Password));

            if (Repository.CreateClient(client).Evaluate(OnInternalFailure: (op) => throw op.Exception))
            {
                Result = new ViewableStrategyResult(Model, "~/Views/Shared/DisplayDetails.cshtml", clearModelState: true, new Dictionary<string, object>
                {
                    ["Id"] = client.ClientId,
                    ["Secret"] = client.Password,
                });
            }

            return await Task.FromResult(Result);
        }
    }
}
