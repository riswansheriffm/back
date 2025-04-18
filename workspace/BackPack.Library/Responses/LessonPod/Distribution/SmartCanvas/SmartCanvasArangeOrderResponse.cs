
namespace BackPack.Library.Responses.LessonPod.Distribution.SmartCanvas
{
    public class SmartCanvasArangeOrderResponse : BaseControlResponse
    {
        public List<OptionControlResponse> Options { get; set; } = [];

        public List<int> Answers { get; set; } = [];
    }
}
