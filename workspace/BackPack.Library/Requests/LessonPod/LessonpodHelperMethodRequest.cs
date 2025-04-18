
using Newtonsoft.Json.Linq;

namespace BackPack.Library.Requests.LessonPod
{
    public class LessonpodHelperMethodRequest
    {
        public required JObject Jobject { get; set; }

        public required string FirstAttribute { get; set; }

        public required string SecondAttribute { get; set; }

        public required string ThirdAttribute { get; set; }

        public required string FourthAttribute { get; set; }

        public required string FifthAttribute { get; set; }

        public required string SixthAttribute { get; set; }

        public required int Index { get; set; }
    }
}
