﻿using DragaliaAPI.Models.Dragalia.Responses;
using MessagePack;

namespace DragaliaAPI.Test.Integration.Dragalia
{
    public class ServiceStatusTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ServiceStatusTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task ServiceStatus_ReturnsCorrectJSON()
        {
            ServiceStatusResponse expectedResponse = new();
            // Corresponds to JSON: "{}"
            byte[] payload = new byte[] { 0x80 };
            HttpContent content = TestUtils.CreateMsgpackContent(payload);

            var response = await _client.PostAsync("tool/get_service_status", content);

            await TestUtils.CheckMsgpackResponse(response, expectedResponse);
        }
    }
}
