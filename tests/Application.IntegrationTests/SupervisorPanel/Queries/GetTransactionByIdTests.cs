using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.DTOs;

namespace Application.IntegrationTests.SupervisorPanel.Queries
{
    public class GetTransactionByIdTests: IntegrationTest
    {
        private readonly IObjectDeserializer<TransactionObject> _transactionObjectDeserializer;

        public GetTransactionByIdTests()
        {
            _transactionObjectDeserializer = new TransactionObjectDeserializer();
        }
        
    }
}