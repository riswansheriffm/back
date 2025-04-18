using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.Master.Group
{
    public class ListGroupResponse : ReadBaseResponse
    {
        public ListGroupDataResult Data { get; set; } = new();
    }

    public class ListGroupDataResult
    {
        public List<ListGroupData> GetAllGroupsResult { get; set; } = [];
    }

    public class ListGroupData
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string GroupDesc { get; set; } = string.Empty;
    }
}
