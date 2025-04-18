using BackPack.Dependency.Library.Responses;

namespace BackPack.Library.Responses.LessonPod
{
    public class AllSlideTemplateResponse : ReadBaseResponse
    {
        public AllSlideTemplateResult Data { get; set; } = new();
    }

    public class AllSlideTemplateResult
    {
        public AllSlideTemplateData GetAllStudioSlideTemplatesResult { get; set; } = new();
    }

    public class AllSlideTemplateData
    {
        public List<SlideTemplateData> Themes { get; set; } = [];
        public List<SlideTemplateData> Gallery { get; set; } = [];
    }

    public class SlideTemplateData
    {
        public int SlideTemplateID { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string QuestionColor { get; set; } = string.Empty;
        public string AnswerColor { get; set; } = string.Empty;
        public string FontStyle { get; set; } = string.Empty;
        public string QuestionFontSize { get; set; } = string.Empty;
        public string AnswerFontSize { get; set; } = string.Empty;
        public string Opacity { get; set; } = string.Empty;
        public string SelectionColor { get; set; } = string.Empty;
        public string AnswerFillColor { get; set; } = string.Empty;
        public string QuestionFillColor { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
    }
}
