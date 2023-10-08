using ClinicService.Controllers;
using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServiceTests
{
    public class ClientControllerTests
    {
        private ClientController _clientController;
        private Mock<IClientRepository> _mocClientRepository;

        public ClientControllerTests()
        {
            _mocClientRepository = new Mock<IClientRepository>();
            _clientController = new ClientController(_mocClientRepository.Object);
        }

        [Fact]
        public void GetAllClientsTest()
        {
            // [1.1] Подготовка данных для тестирования

            // [1.2]

            List<Client> list = new List<Client>();
            list.Add(new Client());
            list.Add(new Client());
            list.Add(new Client());


            _mocClientRepository.Setup(repository =>
                repository.GetAll()).Returns(list);

            // [2] Исполнение тестируемого метода
            var operationResult = _clientController.GetAll();

            // [3] Подготовка эталонного результата, проверка результата
            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<List<Client>>(((OkObjectResult)operationResult.Result).Value);

            _mocClientRepository.Verify(repository =>
                repository.GetAll(), Times.AtLeastOnce());
        }

        public static readonly object[][] CorrectCreateClientData =
        {
            new object[] { new DateTime(1985, 5, 20), "123 1234", "Иванов", "Андрей", "Сергеевич" },
            new object[] { new DateTime(1987, 2, 18), "123 2222", "Иванов", "Андрей", "Сергеевич" },
            new object[] { new DateTime(1979, 1, 22), "123 4321", "Иванов", "Андрей", "Сергеевич" },
        };

        [Theory]
        [MemberData(nameof(CorrectCreateClientData))]
        public void CreateClientTest(DateTime birthday, string document, string surName, string firstName, string patronymic)
        {
            _mocClientRepository.Setup(repository =>
            repository
                .Create(It.IsNotNull<Client>()))
                .Returns(1).Verifiable();

            var operationResult = _clientController.Create(new CreateClientRequest
            {
                Birthday = birthday,
                Document = document,
                SurName = surName,
                FirstName = firstName,
                Patronymic = patronymic
            });

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<int>(((OkObjectResult)operationResult.Result).Value);
            _mocClientRepository.Verify(repository =>
                repository.Create(It.IsNotNull<Client>()), Times.AtLeastOnce());

        }

        [Theory]
        [InlineData(8)]
        public void GetByIDClientTest(int clientId)
        {
            Client client = new Client();
            client.ClientId = clientId;

            _mocClientRepository.Setup(repository =>
            repository.GetById(clientId)).Returns(client);

            var operationResult = _clientController.GetById(clientId);

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<Client>(((OkObjectResult)operationResult.Result).Value);

            _mocClientRepository.Verify(repository =>
            repository.GetById(clientId), Times.AtLeastOnce());

        }

        [Theory]
        [InlineData(4)]
        public void DeleteClientTest(int clientId)
        {
            Client client = new Client();
            client.ClientId = clientId;

            _mocClientRepository.Setup(repository =>
            repository.Delete(clientId)).Returns(1);

            var operationResult = _clientController.Delete(clientId);

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<int>(((OkObjectResult)operationResult.Result).Value);

            _mocClientRepository.Verify(repository =>
            repository.Delete(clientId), Times.AtLeastOnce());

        }

        public static readonly object[][] CorrectUpdateClientData =
        {
            new object[] { new DateTime(1985, 5, 20), "123 1234", "Иванов", "Андрей", "Сергеевич", 1 },
            new object[] { new DateTime(1987, 2, 18), "123 2222", "Иванов", "Андрей", "Сергеевич", 2 },
            new object[] { new DateTime(1979, 1, 22), "123 4321", "Иванов", "Андрей", "Сергеевич", 3 },
        };

        [Theory]
        [MemberData(nameof(CorrectUpdateClientData))]
        public void UpdateClientTest(DateTime birthday, string document, string surName, string firstName, string patronymic,
            int clientId)
        {
            _mocClientRepository.Setup(repository =>
            repository
                .Update(It.IsNotNull<Client>()))
                .Returns(1);

            var operationResult = _clientController.Update(new UpdateClientRequest
            {
                Birthday = birthday,
                Document = document,
                SurName = surName,
                FirstName = firstName,
                Patronymic = patronymic,
                ClientId = clientId
            });

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<int>(((OkObjectResult)operationResult.Result).Value);
            _mocClientRepository.Verify(repository =>
                repository.Update(It.IsNotNull<Client>()), Times.AtLeastOnce());

        }
    }
}
