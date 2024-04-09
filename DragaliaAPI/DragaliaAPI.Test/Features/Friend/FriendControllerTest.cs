using AutoMapper;
using DragaliaAPI.Controllers.Dragalia;
using DragaliaAPI.Features.Friend;
using DragaliaAPI.Models.Generated;
using DragaliaAPI.Services;
using DragaliaAPI.Services.Game;
using DragaliaAPI.Test.Utils;

namespace DragaliaAPI.Test.Features.Friend;

public class FriendControllerTest
{
    private readonly FriendController friendController;
    private readonly Mock<IFriendRepository> mockFriendRepository;
    private readonly Mock<IFriendService> mockFriendService;
    private readonly Mock<IHelperService> mockHelperService;
    private readonly Mock<IBonusService> mockBonusService;
    private readonly Mock<IUpdateDataService> mockUpdateDataService;
    private readonly IMapper mapper;

    public FriendControllerTest()
    {
        this.mockFriendRepository = new(MockBehavior.Strict);
        this.mockFriendService = new(MockBehavior.Strict);
        this.mockHelperService = new(MockBehavior.Strict);
        this.mockBonusService = new(MockBehavior.Strict);
        this.mockUpdateDataService = new(MockBehavior.Strict);

        this.mapper = new MapperConfiguration(cfg =>
            cfg.AddMaps(typeof(Program).Assembly)
        ).CreateMapper();

        this.mockBonusService.Setup(x => x.GetBonusList()).ReturnsAsync(new FortBonusList());

        this.friendController = new FriendController(
            mockFriendRepository.Object,
            mockFriendService.Object,
            mockHelperService.Object,
            mockBonusService.Object,
            mockUpdateDataService.Object,
            mapper
        );

        this.friendController.SetupMockContext();
    }

    [Fact]
    public async Task GetSupportCharaDetailContainsCorrectInformationWhenFound()
    {
        this.mockHelperService.Setup(x => x.GetHelpers())
            .ReturnsAsync(
                new QuestGetSupportUserListResponse()
                {
                    SupportUserList = new List<UserSupportList>() { TestData.SupportListEuden },
                    SupportUserDetailList = new List<AtgenSupportUserDetailList>()
                    {
                        new() { ViewerId = 1000, IsFriend = true, },
                    }
                }
            );

        DragaliaResult response = await this.friendController.GetSupportCharaDetail(
            new FriendGetSupportCharaDetailRequest() { SupportViewerId = 1000 }
        );

        FriendGetSupportCharaDetailResponse? data =
            response.GetData<FriendGetSupportCharaDetailResponse>();
        data.Should().NotBeNull();

        data!
            .SupportUserDataDetail.UserSupportData.Should()
            .BeEquivalentTo(TestData.SupportListEuden);
        data!.SupportUserDataDetail.IsFriend.Should().Be(true);

        this.mockHelperService.VerifyAll();
        this.mockBonusService.VerifyAll();
    }

    [Fact]
    public async Task GetSupportCharaDetailContainsDefaultInformationWhenNotFound()
    {
        this.mockHelperService.Setup(x => x.GetHelpers())
            .ReturnsAsync(
                new QuestGetSupportUserListResponse()
                {
                    SupportUserList = new List<UserSupportList>() { TestData.SupportListEuden },
                    SupportUserDetailList = new List<AtgenSupportUserDetailList>()
                    {
                        new() { ViewerId = 1000, IsFriend = true },
                    }
                }
            );

        DragaliaResult response = await this.friendController.GetSupportCharaDetail(
            new FriendGetSupportCharaDetailRequest() { SupportViewerId = 0 }
        );

        FriendGetSupportCharaDetailResponse? data =
            response.GetData<FriendGetSupportCharaDetailResponse>();
        data.Should().NotBeNull();

        data!
            .SupportUserDataDetail.UserSupportData.Should()
            .BeEquivalentTo(HelperService.StubData.SupportListData.SupportUserList.First());

        data!.SupportUserDataDetail.IsFriend.Should().Be(false);

        this.mockHelperService.VerifyAll();
        this.mockBonusService.VerifyAll();
    }
}
