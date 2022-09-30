﻿namespace DragaliaAPI.Test.Integration.Dragalia;

public class UrlListTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UrlListTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task UrlList_ReturnsCorrectList()
    {
        UrlListResponse expectedResponse = new(); // Constructor automatically adds all the URLs

        // Corresponds to JSON: "{}"
        byte[] payload = new byte[] { 0x80 };
        HttpContent content = TestUtils.CreateMsgpackContent(payload);

        HttpResponseMessage response = await _client.PostAsync("webview_version/url_list", content);

        await TestUtils.CheckMsgpackResponse(response, expectedResponse);
    }
}