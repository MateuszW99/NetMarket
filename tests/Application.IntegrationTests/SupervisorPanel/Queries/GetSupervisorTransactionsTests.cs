using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.DTOs;

namespace Application.IntegrationTests.SupervisorPanel.Queries
{
    public class GetSupervisorTransactionsTests: IntegrationTest
    {
        private readonly IObjectDeserializer<TransactionObject> _transactionObjectDeserializer;

        public GetSupervisorTransactionsTests()
        {
            _transactionObjectDeserializer = new TransactionObjectDeserializer();
        }
        
    }
}