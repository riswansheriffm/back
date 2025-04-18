using BackPack.Library.Constants;
using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod.Distribution;
using BackPack.Library.Responses.LessonPod.Distribution.SmartCanvas;
using BackPack.Library.Responses.LessonPod.Distribution.SmartPaper;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class SmartPaperHelper
    {
        private static int TotalFileSize;
        private static float SlideTotalPoints;
        private static float StudioEditorWidth;
        private static float StudioEditorHeight;
        private static int ControlID;

        #region SmartPaperJson
        public static LessonPodSlideResponse SmartPaperJson(JObject lessonJson, string slideId)
        {
            #region Declaration
            TotalFileSize = 0;
            SlideTotalPoints = 0;
            LessonPodSlideResponse response = new();
            SmartPaperResponse smartPaperResponse = new();
            JsonSerializerOptions jso = new();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            List<object> listObject = [];
            List<SmartPaperStrokeResponse> listSmartPaperStroke = [];
            List<SmartPaperStrokeActionResponse> listSmartPaperStrokeAction = [];
            List<SmartPaperReplayRectangleResponse> listSmartPaperReplayRectangle = [];
            List<SmartPaperReplayRectangleReportResponse> listSmartPaperReplayRectangleReport = [];
            List<SmartPaperInputControlResponse> listSmartPaperInputControl = [];
            #endregion
            
            #region Slide properties            
            JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);
            JObject viewToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.ViewToolbarsById, thirdAttribute: slideId);

            StudioEditorWidth = DistributionCommonHelper.JObjectToFloat(navItemsById, LessonpodConstant.EditorWidth);
            StudioEditorHeight = DistributionCommonHelper.JObjectToFloat(navItemsById, LessonpodConstant.EditorHeight);
            smartPaperResponse.Back = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.Back);
            smartPaperResponse.Stop = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.Stop);
            smartPaperResponse.Skip = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.Skip);
            smartPaperResponse.ChatBot = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.ChatBot);
            JObject objChatBot = DistributionCommonHelper.JObjectToJObject(jObject: navItemsById, firstAttribute: LessonpodConstant.ChatBotSettings);
            smartPaperResponse.ChatBotSettings = objChatBot.ToString(Newtonsoft.Json.Formatting.None);
            smartPaperResponse.IsCanvas = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.IsCanvas);
            response.IsCanvas = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.IsCanvas);
            JObject objTemplate = DistributionCommonHelper.JObjectToJObject(jObject: navItemsById, firstAttribute: LessonpodConstant.TemplateSettings);
            smartPaperResponse.TemplateSettings = (objTemplate != null) ? JsonSerializer.Deserialize<object>(objTemplate.ToString()) : new();
            smartPaperResponse.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.SpeechAudioData);
            #endregion

            #region Sketch history
            JArray sketchHistory = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.SketchHistory);
            AddSketchHistory(slideId: slideId, sketchHistory: sketchHistory, lessonJson: lessonJson, listSmartPaperStroke: listSmartPaperStroke, listSmartPaperStrokeAction: listSmartPaperStrokeAction);
            #endregion

            #region Replay rectangles
            ControlID = 0;
            JArray slideBoxes = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Boxes);
            foreach (var box in slideBoxes)
            {
                #region Declaration
                ControlID++;
                string boxId = box.ToString();
                JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
                JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
                string pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                #endregion

                #region AssesmentBlock 
                if (pluginId == ServiceConstant.ComAssesmentBlock)
                {
                    JObject sortableContainer = DistributionCommonHelper.JObjectToJObject(jObject: boxesById, firstAttribute: LessonpodConstant.SortableContainers);
                    foreach (var item in sortableContainer)
                    {
                        boxId = item.Value![LessonpodConstant.Children]![0]!.ToString();
                    }
                    
                    if (boxId != "")
                    {
                        pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
                        boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
                        pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                    }
                }
                #endregion

                #region Plugins
                switch (pluginId)
                {
                    case ServiceConstant.ComReplayRectangle:
                    case ServiceConstant.ComSCReplayRectangle:
                        AddReplayRectangle(pluginId: pluginId, pluginToolbarsById: pluginToolbarsById, boxesById: boxesById, listSmartPaperReplayRectangle: listSmartPaperReplayRectangle, listSmartPaperReplayRectangleReport: listSmartPaperReplayRectangleReport);
                        break;
                    case ServiceConstant.ComTextArea:
                    case ServiceConstant.ComSCTextArea:
                    case ServiceConstant.ComSCTitle:
                        AddTextArea(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComTextInk:
                        AddTextInk(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComRichText:
                    case ServiceConstant.ComSCRichText:
                        AddRichText(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComPopupSlide:
                        AddPopupSlide(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComHotspotImages:
                    case ServiceConstant.ComSCHotspotImages:
                        AddHotspotImages(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComEnrichedAudio:
                        AddEnrichedAudio(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComEnrichedPlayer:
                        AddEnrichedPlayer(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComEnrichedPDF:
                        AddEnrichedPDF(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComVisor3D:
                        AddVisor3D(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComAGTextList:
                        AddAgTextList(lessonJson: lessonJson, boxId: boxId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComAGInput:
                        AddAgInput(lessonJson: lessonJson, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSSMultipleChoice:
                    case ServiceConstant.ComSCMultipleChoice:
                        AddMultipleChoice(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSSMultipleAnswer:
                    case ServiceConstant.ComSCMultipleAnswer:
                        AddMultipleAnswer(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSSTrueFalse:
                    case ServiceConstant.ComSCTrueFalse:
                        AddTrueFalse(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSSRotatingDial:
                        AddRotatingDial(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSSFillinTheBlanks:
                        AddFillinTheBlanks(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSPHotspot:
                        AddHotspot(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComFraction:
                        AddFraction(lessonJson: lessonJson, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComMatchTable:
                        AddMatchTable(lessonJson: lessonJson, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComH5P:
                        AddH5P(lessonJson: lessonJson, boxId: boxId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComWebpage:
                        AddWebpage(lessonJson: lessonJson, boxId: boxId, listObject: listObject);
                        break;
                    case ServiceConstant.ComAGInkList:
                        AddAgInkList(lessonJson: lessonJson, boxId: boxId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComDropdown:
                        AddDropdown(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComInputAnswer:
                        AddInputAnswer(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSCArrangeOrder:
                        AddScArrangeOrder(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    case ServiceConstant.ComSCAnswerPad:
                        AddScAnswerPad(lessonJson: lessonJson, pluginId: pluginId, boxId: boxId, slideId: slideId, listObject: listObject, listSmartPaperInputControl: listSmartPaperInputControl);
                        break;
                    default:
                        break;
                }
                #endregion
            }
            #endregion

            #region Paper properties
            smartPaperResponse.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.TextToSpeech));
            smartPaperResponse.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.Seconds));
            smartPaperResponse.Title = DistributionCommonHelper.JObjectToString(viewToolbarsById, LessonpodConstant.ViewName);
            smartPaperResponse.EditorImageWidth = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.EditorWidth);
            smartPaperResponse.EditorImageHeight = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.EditorHeight);
            smartPaperResponse.BackgroundImage = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.ImageInUrl);
            smartPaperResponse.Opacity = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.Opacity);
            smartPaperResponse.TotalScore = SlideTotalPoints;
            smartPaperResponse.InputRows = listObject;
            smartPaperResponse.Strokes = listSmartPaperStroke;
            smartPaperResponse.StrokeActions = listSmartPaperStrokeAction;
            smartPaperResponse.ReplayRectangles = listSmartPaperReplayRectangle;
            #endregion

            #region Response
            response.IsReadonly = DistributionCommonHelper.JObjectToBoolean(navItemsById, LessonpodConstant.ReadOnly);
            response.SearchTag = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.SearchTag);
            response.SmartPaperInputControls = listSmartPaperInputControl;
            response.SmartPaperReplayRectangleReports = listSmartPaperReplayRectangleReport;
            response.SmartPaperStrokeReports = listSmartPaperStroke;
            response.FileSize = TotalFileSize;
            response.TotalPoints = SlideTotalPoints;
            string SmartPaperJson = JsonSerializer.Serialize(smartPaperResponse, jso);
            response.SlideJson = SmartPaperJson;
            #endregion

            return response;
        }
        #endregion

        #region Components

        #region AddSketchHistory
        private static void AddSketchHistory(string slideId, JArray sketchHistory, JObject lessonJson, List<SmartPaperStrokeResponse> listSmartPaperStroke, List<SmartPaperStrokeActionResponse> listSmartPaperStrokeAction)
        {
            foreach (var sketch in sketchHistory)
            {
                string sketchType = (string)sketch[LessonpodConstant.Type]!;
                string strokeAction = (string)sketch[LessonpodConstant.Action]!;
                if (sketchType == LessonpodConstant.Path || sketchType == LessonpodConstant.Line || sketchType == LessonpodConstant.Circle || sketchType == LessonpodConstant.Rect)
                {
                    AddStorkes(slideId: slideId, sketchType: sketchType, strokeAction: strokeAction, sketch: sketch, lessonJson: lessonJson, listSmartPaperStroke: listSmartPaperStroke, listSmartPaperStrokeAction: listSmartPaperStrokeAction);
                }
            }
        }
        #endregion

        #region AddStorkes
        private static void AddStorkes(string slideId, string sketchType, string strokeAction, JToken sketch, JObject lessonJson, List<SmartPaperStrokeResponse> listSmartPaperStroke, List<SmartPaperStrokeActionResponse> listSmartPaperStrokeAction)
        {
            var smartPaperStroke = AddStrokeResponse(sketchType, strokeAction, sketch, lessonJson, slideId);
            listSmartPaperStroke.Add(smartPaperStroke);

            var smartPaperStrokeAction = AddStrokeActionResponse(strokeAction, smartPaperStroke.ID);
            listSmartPaperStrokeAction.Add(smartPaperStrokeAction);
        }
        #endregion

        #region AddStrokeResponse
        private static SmartPaperStrokeResponse AddStrokeResponse(string sketchType, string strokeAction, JToken sketch, JObject lessonJson, string slideId)
        {
            var smartPaperStroke = new SmartPaperStrokeResponse();
            List<List<float>> listPath = [];
            AddStrokePath(sketchType: sketchType, sketch: sketch, listPath: listPath);

            int strokeID = AddStrokeId(slideId: slideId, strokeAction: strokeAction, lessonJson: lessonJson, sketch: sketch);

            smartPaperStroke.Data = listPath;
            smartPaperStroke.IsDeleted = (strokeAction != LessonpodConstant.Add);
            smartPaperStroke.ID = strokeID;
            smartPaperStroke.Color = (string)sketch[LessonpodConstant.Stroke]!;
            smartPaperStroke.Width = (float)sketch[LessonpodConstant.StrokeWidth]!;
            smartPaperStroke.ShapeType = sketchType;
            smartPaperStroke.ShapeWidth = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Width);
            smartPaperStroke.ShapeHeight = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Height);
            smartPaperStroke.ShapeTop = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Top);
            smartPaperStroke.ShapeLeft = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Left);
            smartPaperStroke.ShapeRadius = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Radius);
            smartPaperStroke.ShapeRotation = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.Angle);
            smartPaperStroke.ScaleX = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.ScaleX);
            smartPaperStroke.ScaleY = DistributionCommonHelper.JTokenToFloat(sketch, LessonpodConstant.ScaleY);

            return smartPaperStroke;
        }
        #endregion

        #region AddStrokeActionResponse
        private static SmartPaperStrokeActionResponse AddStrokeActionResponse(string strokeAction, int strokeID)
        {
            var smartPaperStrokeAction = new SmartPaperStrokeActionResponse
            {
                StrokeID = strokeID,
                Action = strokeAction
            };

            return smartPaperStrokeAction;
        }
        #endregion

        #region AddStrokePath
        private static void AddStrokePath(string sketchType, JToken sketch, List<List<float>> listPath)
        {
            if (sketchType == LessonpodConstant.Path)
            {
                JArray strokePath = (JArray)sketch[LessonpodConstant.Path]!;
                foreach (var path in strokePath)
                {
                    List<float> listStroke = [];
                    JArray pathStroke = (JArray)path;
                    int strokeIndex = 0;
                    foreach (var stroke in pathStroke)
                    {
                        if (strokeIndex == 1 || strokeIndex == 2)
                        {
                            listStroke.Add((float)stroke);
                        }
                        strokeIndex++;
                    }
                    listPath.Add(listStroke);
                }
            }
        }
        #endregion

        #region AddStrokeId
        private static int AddStrokeId(string slideId, string strokeAction, JObject lessonJson, JToken sketch)
        {
            int strokeId = 0;
            if (strokeAction == LessonpodConstant.Remove)
            {
                JArray deletedSketchHistory = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.SketchHistory);

                foreach (var stroke in deletedSketchHistory)
                {
                    string StrokeSketchType = (string)stroke[LessonpodConstant.Type]!;
                    bool isValidStrokeType = StrokeSketchType == LessonpodConstant.Path ||
                        StrokeSketchType == LessonpodConstant.Line ||
                        StrokeSketchType == LessonpodConstant.Circle ||
                        StrokeSketchType == LessonpodConstant.Rect;
                    bool isStrokeAndSketchMatching = stroke[LessonpodConstant.Width]!.ToString() == sketch[LessonpodConstant.Width]!.ToString() &&
                        stroke[LessonpodConstant.Height]!.ToString() == sketch[LessonpodConstant.Height]!.ToString() &&
                        stroke[LessonpodConstant.Left]!.ToString() == sketch[LessonpodConstant.Left]!.ToString() &&
                        stroke[LessonpodConstant.Top]!.ToString() == sketch[LessonpodConstant.Top]!.ToString();

                    if ((isValidStrokeType && isStrokeAndSketchMatching))
                    {
                        strokeId = (int)stroke[LessonpodConstant.Id]!;
                        break;
                    }
                }
            }
            else
            {
                strokeId = (int)sketch[LessonpodConstant.Id]!;
            }

            return strokeId;
        }
        #endregion

        #region AddReplayRectangle
        private static void AddReplayRectangle(string pluginId, JObject pluginToolbarsById, JObject boxesById, List<SmartPaperReplayRectangleResponse> listSmartPaperReplayRectangle, List<SmartPaperReplayRectangleReportResponse> listSmartPaperReplayRectangleReport)
        {
            #region Replay rectangle properties                    
            SmartPaperReplayRectangleResponse smartPaperReplayRectangle = new()
            {
                ControlID = ControlID,
                Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority),
                Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay),
                BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition),

                Expand = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Expand),

                XAxis = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X),
                YAxis = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y),
                Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width),
                Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height),
                ReplaySpeed = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ReplaySpeed),
                AudioUrl = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AudioUrl),
                StartInterval = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.StartInterval),

                SpeechData = DistributionCommonHelper.JArrayToStringList((JArray)pluginToolbarsById[LessonpodConstant.State]![LessonpodConstant.SpeechData]!),

                SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData),
                OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData),
                FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData)
            };

            listSmartPaperReplayRectangle.Add(smartPaperReplayRectangle);
            #endregion

            #region Replay rectangle report
            SmartPaperReplayRectangleReportResponse smartPaperReplayRectangleReport = new();
            smartPaperReplayRectangleReport.ControlID = ControlID;
            smartPaperReplayRectangleReport.PluginName = pluginId;
            smartPaperReplayRectangleReport.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            smartPaperReplayRectangleReport.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            smartPaperReplayRectangleReport.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            smartPaperReplayRectangleReport.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            smartPaperReplayRectangleReport.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            smartPaperReplayRectangleReport.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            float.TryParse(smartPaperReplayRectangleReport.X.ToString().Replace(LessonpodConstant.Percentage, ""), out float ReplayX);
            float.TryParse(smartPaperReplayRectangleReport.Y.ToString().Replace(LessonpodConstant.Percentage, ""), out float ReplayY);
            float.TryParse(pluginToolbarsById[LessonpodConstant.Structure]![LessonpodConstant.Width]!.ToString().Replace(LessonpodConstant.Percentage, ""), out float ReplayWidth);
            float.TryParse(pluginToolbarsById[LessonpodConstant.Structure]![LessonpodConstant.Height]!.ToString().Replace(LessonpodConstant.Percentage, ""), out float ReplayHeight);
            float XLeft = (StudioEditorWidth * ReplayX) / 100;
            float YTop = (StudioEditorHeight * ReplayY) / 100;
            smartPaperReplayRectangleReport.XLeft = XLeft;
            smartPaperReplayRectangleReport.XRight = XLeft + ((StudioEditorWidth * ReplayWidth) / 100);
            smartPaperReplayRectangleReport.YTop = YTop;
            smartPaperReplayRectangleReport.YBottom = YTop + ((StudioEditorHeight * ReplayHeight) / 100);

            listSmartPaperReplayRectangleReport.Add(smartPaperReplayRectangleReport);
            #endregion
        }
        #endregion

        #region AddTextArea
        private static void AddTextArea(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = TextAreaJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddTextInk
        private static void AddTextInk(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = TextInkJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddRichText
        private static void AddRichText(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = RichTextJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddPopupSlide
        private static void AddPopupSlide(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            SmartPaperPopupSlideResponse smartPaperPopupSlide = PopupSlideJson(lessonJson, boxId);
            smartPaperPopupSlide.ControlID = ControlID;
            smartPaperPopupSlide.PluginId = pluginId;
            listObject.Add(smartPaperPopupSlide);
        }
        #endregion

        #region AddHotspotImages
        private static void AddHotspotImages(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = HotspotImagesJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddEnrichedAudio
        private static void AddEnrichedAudio(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = EnrichedAudioJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddEnrichedPlayer
        private static void AddEnrichedPlayer(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = EnrichedPlayerJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;

            #region Player mark
            List<PlayerMarkResponse> listPlayerMark = PlayerMarkJson(lessonJson, boxId);
            baseControlResponse.Mark = listPlayerMark;
            #endregion

            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddEnrichedPDF
        private static void AddEnrichedPDF(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = EnrichedPDFJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddVisor3D
        private static void AddVisor3D(JObject lessonJson, string pluginId, string boxId, List<object> listObject)
        {
            BaseControlResponse? baseControlResponse = Visor3DJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginId = pluginId;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddAgTextList
        private static void AddAgTextList(JObject lessonJson, string boxId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            BaseControlResponse baseControlResponse = AGTextListJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = TextListControlJson(lessonJson, boxId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddAgInput
        private static void AddAgInput(JObject lessonJson, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            BaseControlResponse baseControlResponse = AGInputJson(lessonJson, boxId, slideId);
            baseControlResponse.ControlID = ControlID;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = InputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddMultipleChoice
        private static void AddMultipleChoice(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperMultipleChoiceResponse smartPaperMultipleChoice = MultipleChoiceJson(lessonJson, boxId, slideId);
            smartPaperMultipleChoice.ControlID = ControlID;
            smartPaperMultipleChoice.PluginName = pluginId;
            listObject.Add(smartPaperMultipleChoice);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddMultipleAnswer
        private static void AddMultipleAnswer(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperMultipleAnswerResponse smartPaperMultipleAnswer = MultipleAnswerJson(lessonJson, boxId, slideId);
            smartPaperMultipleAnswer.ControlID = ControlID;
            smartPaperMultipleAnswer.PluginName = pluginId;
            listObject.Add(smartPaperMultipleAnswer);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddTrueFalse
        private static void AddTrueFalse(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperTrueFalseResponse baseControlResponse = TrueFalseJson(lessonJson, boxId, slideId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginName = pluginId;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddRotatingDial
        private static void AddRotatingDial(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperRotatingDialResponse baseControlResponse = RotatingDialJson(lessonJson, boxId, slideId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginName = pluginId;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddFillinTheBlanks
        private static void AddFillinTheBlanks(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperFillinTheBlanksResponse smartPaperFillinTheBlank = FillinTheBlanksJson(lessonJson, boxId, slideId);
            smartPaperFillinTheBlank.ControlID = ControlID;
            smartPaperFillinTheBlank.PluginName = pluginId;
            listObject.Add(smartPaperFillinTheBlank);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddHotspot
        private static void AddHotspot(JObject lessonJson, string pluginId, string boxId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperHotspotResponse baseControlResponse = HotspotJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginName = pluginId;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = HotspotControlJson(lessonJson, boxId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddFraction
        private static void AddFraction(JObject lessonJson, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperFractionResponsecs baseControlResponse = FractionJson(lessonJson, boxId, slideId);
            baseControlResponse.ControlID = ControlID;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddMatchTable
        private static void AddMatchTable(JObject lessonJson, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartPaperMatchTableResponse smartPaperMatchTable = MatchTableJson(lessonJson, boxId, slideId);
            smartPaperMatchTable.ControlID = ControlID;
            listObject.Add(smartPaperMatchTable);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddH5P
        private static void AddH5P(JObject lessonJson, string boxId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            BaseControlResponse baseControlResponse = H5PJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = H5PControlJson(lessonJson, boxId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddWebpage
        private static void AddWebpage(JObject lessonJson, string boxId, List<object> listObject)
        {
            BaseControlResponse baseControlResponse = WebpageJson(lessonJson, boxId);
            baseControlResponse.ControlID = ControlID;
            listObject.Add(baseControlResponse);
        }
        #endregion

        #region AddAgInkList
        private static void AddAgInkList(JObject lessonJson, string boxId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            BaseControlResponse? baseControlResponse = AGInkListJson(lessonJson, boxId);
            if (baseControlResponse != null)
            {
                baseControlResponse.ControlID = ControlID;
                listObject.Add(baseControlResponse);

                SmartPaperInputControlResponse smartPaperInputControl = InkListControlJson(lessonJson, boxId);
                smartPaperInputControl.ControlID = ControlID;
                listSmartPaperInputControl.Add(smartPaperInputControl);
            }
            else
            {
                ControlID--;
            }
        }
        #endregion

        #region AddDropdown
        private static void AddDropdown(JObject lessonJson, string pluginId, string boxId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartCanvasDropdownResponse smartCanvasDropdown = DropdownJson(lessonJson, boxId);
            smartCanvasDropdown.ControlID = ControlID;
            smartCanvasDropdown.PluginName = pluginId;
            listObject.Add(smartCanvasDropdown);

            SmartPaperInputControlResponse smartPaperInputControl = DropdownControlJson(lessonJson, boxId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddInputAnswer
        private static void AddInputAnswer(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartCanvasInputAnswerResponse smartCanvasInputAnswer = InputAnswerJson(lessonJson, boxId, slideId);
            smartCanvasInputAnswer.ControlID = ControlID;
            smartCanvasInputAnswer.PluginName = pluginId;
            listObject.Add(smartCanvasInputAnswer);

            SmartPaperInputControlResponse smartPaperInputControl = InputAnswerControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddScAnswerPad
        private static void AddScArrangeOrder(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            SmartCanvasArangeOrderResponse smartCanvasArangeOrder = ArrangeOrderJson(lessonJson, boxId, slideId);
            smartCanvasArangeOrder.ControlID = ControlID;
            smartCanvasArangeOrder.PluginName = pluginId;
            listObject.Add(smartCanvasArangeOrder);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #region AddScAnswerPad
        private static void AddScAnswerPad(JObject lessonJson, string pluginId, string boxId, string slideId, List<object> listObject, List<SmartPaperInputControlResponse> listSmartPaperInputControl)
        {
            BaseControlResponse baseControlResponse = AnswerPadJson(lessonJson, boxId, slideId);
            baseControlResponse.ControlID = ControlID;
            baseControlResponse.PluginName = pluginId;
            listObject.Add(baseControlResponse);

            SmartPaperInputControlResponse smartPaperInputControl = SmartPaperInputControlJson(lessonJson, boxId, slideId);
            smartPaperInputControl.ControlID = ControlID;
            listSmartPaperInputControl.Add(smartPaperInputControl);
        }
        #endregion

        #endregion

        #region Component Json

        #region TextAreaJson
        private static BaseControlResponse TextAreaJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Value = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontFamily = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.FillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            response.TextType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextType);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region TextInkJson
        private static BaseControlResponse TextInkJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.FillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.Color = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextInkType);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            #endregion

            return response;
        }
        #endregion

        #region RichTextJson
        private static BaseControlResponse RichTextJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Value = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DecodeText);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.Editable = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Edit);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region PopupSlideJson
        private static SmartPaperPopupSlideResponse PopupSlideJson(JObject lessonJson, string boxId)
        {
            #region Declaration            
            SmartPaperPopupSlideResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value 
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            SmartPaperPopupSlideMarkResponse smartPaperPopupSlideMark = new()
            {
                MarkerName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Mark, LessonpodConstant.Title),
                MarkerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Mark, LessonpodConstant.ConnectMode),
                MarkerValue = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Mark, LessonpodConstant.Connection)
            };
            
            response.MarkData = smartPaperPopupSlideMark;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region HotspotImagesJson
        private static BaseControlResponse HotspotImagesJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);
            TotalFileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FileSize);

            response.IsVideo = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IsVideo);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.Text = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
            response.AllowDeformed = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AllowDeformed);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            #endregion

            return response;
        }
        #endregion

        #region EnrichedAudioJson
        private static BaseControlResponse EnrichedAudioJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.BarWidth = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BarWidth);
            TotalFileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FileSize);

            response.AutoPlay = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Autoplay);
            response.Controls = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Controls);
            response.Waves = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Waves);
            response.Scroll = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Scroll);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.ProgressColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ProgressColor);
            response.WaveColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.WaveColor);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            #endregion

            return response;
        }
        #endregion

        #region EnrichedPlayerJson
        private static BaseControlResponse EnrichedPlayerJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            TotalFileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FileSize);

            response.Controls = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Controls);
            response.VideoSkip = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.VideoSkip);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.VideoDuration = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DurationFormated);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region PlayerMarkJson
        private static List<PlayerMarkResponse> PlayerMarkJson(JObject lessonJson, string boxId)
        {
            List<PlayerMarkResponse> response = [];
            JObject marksById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById);

            foreach (var markItem in marksById)
            {
                string marker = markItem.ToString();
                int index = marker.IndexOf(',');
                string markId = marker.Substring(1, index - 1).ToString();
                JObject mark = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById, thirdAttribute: markId);
                string origin = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Origin);
                if (origin == boxId)
                {
                    PlayerMarkResponse playerMark = new()
                    {
                        MarkerName = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Title),
                        Seconds = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Value),
                        MarkerType = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.ConnectMode),
                        MarkerValue = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Connection)
                    };                    

                    response.Add(playerMark);
                }
            }

            return response;
        }
        #endregion

        #region EnrichedPDFJson
        private static BaseControlResponse EnrichedPDFJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.PageNumber = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.PageNumber);
            TotalFileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FileSize);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.NumPages = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NumPages);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            #endregion

            return response;
        }
        #endregion

        #region Visor3DJson
        private static BaseControlResponse Visor3DJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.ObjectUrl = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.TextureUrl = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ImgUrl);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            #endregion

            return response;
        }
        #endregion

        #region AGTextListJson
        private static BaseControlResponse AGTextListJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            List<SmartPaperListDataResponse> listData = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region List data
            int ExerciseWeight = 0;
            JArray data = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Data);
            foreach (var item in data)
            {
                SmartPaperListDataResponse smartPaperListData = new()
                {
                    Answer = item[0]!.ToString(),
                    Value = item[1]!.ToString()
                };
                
                int ExerciseValue = (int)item[1]!;
                if (ExerciseWeight < ExerciseValue)
                {
                    ExerciseWeight = ExerciseValue;
                }

                listData.Add(smartPaperListData);
            }

            List<string> listKey =
            [
                LessonpodConstant.KeyAnswer,
                LessonpodConstant.KeyValue
            ];
            #endregion

            #region Set attributes value
            SlideTotalPoints += ExerciseWeight;
            response.Data = listData;
            response.Keys = listKey;

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.WidthUnit = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.WidthUnit);
            response.heightUnit = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.HeightUnit);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontFamily = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.FillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region AGInputJson
        private static BaseControlResponse AGInputJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Weight = Weight;

            response.CaseSensitive = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Characters);
            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.Type = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Type);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.CorrectAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.MinValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MinValue);
            response.MaxValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MaxValue);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontFamily = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.FillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.FontColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region MultipleChoiceJson
        private static SmartPaperMultipleChoiceResponse MultipleChoiceJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperMultipleChoiceResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            List<OptionControlResponse> listOption = new();
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    OptionControlResponse optionControl = new();
                    optionControl.Option = (answerType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                    listOption.Add(optionControl);
                }
            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);            
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionHeight);            
            response.CategoryTitle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CategoryTitle);
            response.NumberOfOptions = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfOption);
            response.AnswerUI = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerUi);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            response.LayoutType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LayoutType);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            
            List<int> listAnswer =
            [
                DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.CorrectAnswer)
            ];

            response.Answers = listAnswer;

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));
            response.FeedbackOptions = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Options));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            response.ExtraPracticeComponents = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.ExtraPracticeComponents);

            SlideTotalPoints += Weight;
            #endregion

            return response;
        }
        #endregion

        #region MultipleAnswerJson
        private static SmartPaperMultipleAnswerResponse MultipleAnswerJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperMultipleAnswerResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            List<OptionControlResponse> listOption = new();
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    OptionControlResponse optionControl = new()
                    {
                        Option = (answerType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url)
                    };
                    
                    listOption.Add(optionControl);

                }
            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionHeight);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            response.LayoutType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LayoutType);

            response.Answers = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));
            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));
            response.FeedbackOptions = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Options));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

            SlideTotalPoints += Weight;
            #endregion

            return response;
        }
        #endregion

        #region TrueFalseJson
        private static SmartPaperTrueFalseResponse TrueFalseJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperTrueFalseResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            List<string> listOption = new();
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else listOption.Add(DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text));
            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionHeight);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));
            response.Answers = DistributionCommonHelper.JArrayToBooleanList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

            SlideTotalPoints += Weight;
            #endregion

            return response;
        }
        #endregion

        #region RotatingDialJson
        private static SmartPaperRotatingDialResponse RotatingDialJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperRotatingDialResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Type);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            response.Type = answerType;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.MinRange = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MinValue);
            response.MaxRange = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MaxValue);
            response.MinValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.InitialValue);
            response.MaxValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FinalValue);
            response.LabelStep = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LabelStep);
            response.StepCount = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.StepCount);
            response.StartValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.StartValue);
            response.Answers = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.TypeAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region FillinTheBlanksJson
        private static SmartPaperFillinTheBlanksResponse FillinTheBlanksJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperFillinTheBlanksResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == LessonpodConstant.QuestionTypeText) ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            #endregion

            #region Blank array
            JArray objBlankArray = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.BlankArray);
            List<FillinTheBlanksBlankArrayResponse> listFillinTheBlanksBlankArray = [];
            foreach (var blankArray in objBlankArray)
            {
                FillinTheBlanksBlankArrayResponse fillinTheBlanksBlankArray = new();
                JObject blank = (JObject)blankArray;

                int blankValue = DistributionCommonHelper.JObjectToInteger(blank, LessonpodConstant.Blank);
                fillinTheBlanksBlankArray.Blank = blankValue;
                fillinTheBlanksBlankArray.Option = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: blank, firstAttribute: LessonpodConstant.Option));
                listFillinTheBlanksBlankArray.Add(fillinTheBlanksBlankArray);
            }

            response.BlankArray = listFillinTheBlanksBlankArray;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);

            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            
            response.Answers = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.ValueArray));
            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region HotspotJson
        private static SmartPaperHotspotResponse HotspotJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            SmartPaperHotspotResponse response = new();
            List<HotSpotArrayResponse> listHotSpotArray = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value

            #region Hot spot array
            int Score = 0;
            bool IsMultipleQuestion = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IsMultipleQuestion);
            JObject marksById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById);
            foreach (var markItem in marksById)
            {
                string marker = markItem.ToString();
                int index = marker.IndexOf(',');
                string markId = marker.Substring(1, index - 1).ToString();
                JObject mark = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById, thirdAttribute: markId);
                string origin = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Origin);

                if (origin == boxId)
                {
                    HotSpotArrayResponse hotSpotArray = new();
                    string positions = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Value);
                    if (positions != "")
                    {
                        string[] position = positions.Split(',');
                        hotSpotArray.X = position[0].ToString();
                        hotSpotArray.Y = position[1].ToString();
                    }

                    hotSpotArray.ID = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Id);
                    hotSpotArray.Question = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Question);
                    hotSpotArray.AnswerLabel = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Answer);
                    hotSpotArray.Color = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Color);
                    hotSpotArray.Width = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Width);
                    hotSpotArray.Height = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Height);
                    int HotSpotScore = DistributionCommonHelper.JObjectToInteger(mark, LessonpodConstant.Score);
                    hotSpotArray.Score = HotSpotScore;

                    if (IsMultipleQuestion)
                    {
                        Score += HotSpotScore;
                    }
                    else
                    {
                        Score = HotSpotScore;
                    }

                    listHotSpotArray.Add(hotSpotArray);
                }
            }

            response.HotSpot = listHotSpotArray;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Score = Score;

            response.IsMultipleQuestion = IsMultipleQuestion;
            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.EditorImageWidth = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Width);
            response.EditorImageHeight = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Height);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Display);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.BackgroundOverlay = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BackgroundOverlay);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Score;
            #endregion

            return response;
        }
        #endregion

        #region FractionJson
        private static SmartPaperFractionResponsecs FractionJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperFractionResponsecs response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontFamily = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.BorderColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BorderColor);
            response.LayoutType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Type);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.Answers = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region MatchTableJson
        private static SmartPaperMatchTableResponse MatchTableJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration            
            SmartPaperMatchTableResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value             
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);            
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontFamily = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);            
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.BorderColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BorderColor);
            response.QuestionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.Type = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Type);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            
            response.ColQuestion = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.ColQuestion));
            response.RowAnswer = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.RowAnswer));
            response.Answers = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Answer));
            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region H5PJson
        private static BaseControlResponse H5PJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);

            float Score = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Score);
            response.Score = Score;

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.LibraryName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LibraryName);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            SlideTotalPoints += Score;
            #endregion

            return response;
        }
        #endregion

        #region WebpageJson
        private static BaseControlResponse WebpageJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region AGInkListJson
        private static BaseControlResponse? AGInkListJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            List<SmartPaperListDataResponse> listData = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region List data
            int ExerciseWeight = 0;
            bool IsAddObject = false;
            JArray objData = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Data);
            foreach (var item in objData)
            {
                if (item.ToString() != "[]")
                {
                    IsAddObject = true;
                    SmartPaperListDataResponse smartPaperListData = new()
                    {
                        Answer = item[0]!.ToString(),
                        Value = item[1]!.ToString()
                    };
                    int ExerciseValue = (int)item[1]!;
                    if (ExerciseWeight < ExerciseValue)
                    {
                        ExerciseWeight = ExerciseValue;
                    }

                    listData.Add(smartPaperListData);
                }
            }

            List<string> listKey =
            [
                LessonpodConstant.KeyAnswer,
                LessonpodConstant.KeyValue
            ];
            #endregion

            #region Set attributes value
            SlideTotalPoints += ExerciseWeight;
            response.Data = listData;
            response.Keys = listKey;
            response.Score = ExerciseWeight;

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);
            response.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Rotation);
            response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.PluginId = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.WidthUnit = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.WidthUnit);
            response.heightUnit = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.HeightUnit);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.Color = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.FontName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AgListType);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));
            #endregion

            return IsAddObject ? response : new();
        }
        #endregion

        #region DropdownJson
        private static SmartCanvasDropdownResponse DropdownJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            SmartCanvasDropdownResponse response = new();
            List<SmartCanvasDropdownOption> listOption = [];
            int totalScore = 0;
            JObject parentPluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            
            #endregion

            #region Set attributes value            
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            foreach (var children in boxesByIdChildren)
            {
                SmartCanvasDropdownOption smartCanvasDropdownOption = new();
                int exerciseWeight = 0;
                string pageName = children.ToString();
                LessonpodHelperMethodRequest methodRequest = new()
                {
                    Jobject = lessonJson,
                    FirstAttribute = LessonpodConstant.Present,
                    SecondAttribute = LessonpodConstant.BoxesById,
                    ThirdAttribute = boxId,
                    FourthAttribute = LessonpodConstant.SortableContainers,
                    FifthAttribute = pageName,
                    SixthAttribute = LessonpodConstant.Children,
                    Index = 0
                };
                string pageId = DistributionCommonHelper.JObjectToString(methodRequest);
                JArray boxesByIdPage = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Children);
                foreach (var page in boxesByIdPage)
                {
                    string boxPageName = page.ToString();
                    LessonpodHelperMethodRequest boxPageRequest = new()
                    {
                        Jobject = lessonJson,
                        FirstAttribute = LessonpodConstant.Present,
                        SecondAttribute = LessonpodConstant.BoxesById,
                        ThirdAttribute = pageId,
                        FourthAttribute = LessonpodConstant.SortableContainers,
                        FifthAttribute = boxPageName,
                        SixthAttribute = LessonpodConstant.Children,
                        Index = 0
                    };
                    string boxPageId = DistributionCommonHelper.JObjectToString(boxPageRequest);
                    JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxPageId);
                    string pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                    if (pluginId == ServiceConstant.ComSCBasicText)
                    {
                        string questionText = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
                        smartCanvasDropdownOption.QuestionText = questionText;
                    }
                    if (pluginId == ServiceConstant.ComSCAGTextList)
                    {
                        JArray optionData = (JArray)pluginToolbarsById[LessonpodConstant.State]![LessonpodConstant.Data]!;
                        var answerData = DropdownJsonAnswerList(optionData, exerciseWeight);
                        exerciseWeight = answerData.Item2;
                        smartCanvasDropdownOption.Data = answerData.Item1;
                        totalScore += exerciseWeight;
                        smartCanvasDropdownOption.HighScore = exerciseWeight;
                        JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Position);
                        smartCanvasDropdownOption.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Priority);                                                
                    }
                }
                JObject slidePluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pageId);
                smartCanvasDropdownOption.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
                smartCanvasDropdownOption.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
                smartCanvasDropdownOption.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
                smartCanvasDropdownOption.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
                smartCanvasDropdownOption.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

                smartCanvasDropdownOption.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
                smartCanvasDropdownOption.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
                smartCanvasDropdownOption.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

                listOption.Add(smartCanvasDropdownOption);
            }
            response.Score = totalScore;
            response.LayoutType = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LayoutType);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);
            response.Options = listOption;            
            SlideTotalPoints += totalScore;
            #endregion

            return response;
        }
        #endregion

        #region DropdownJsonAnswerList
        private static (List<SmartPaperListDataResponse>, int) DropdownJsonAnswerList(JArray optionData, int exerciseWeight)
        {
            List<SmartPaperListDataResponse> listData = [];
            foreach (var data in optionData)
            {
                SmartPaperListDataResponse smartPaperListData = new()
                {
                    Answer = data[0]!.ToString(),
                    Value = data[1]!.ToString()
                };

                listData.Add(smartPaperListData);
                int exerciseValue = (int)data[1]!;
                if (exerciseWeight < exerciseValue)
                {
                    exerciseWeight = exerciseValue;
                }
            }

            return (listData, exerciseWeight);
        }
        #endregion

        #region InputAnswerJson
        private static SmartCanvasInputAnswerResponse InputAnswerJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartCanvasInputAnswerResponse response = new();
            List<SmartCanvasInputAnswerOption> listOption = [];
            JObject parentPluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            #endregion
            
            #region Set attributes value
            int totalScore = 0;
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            foreach (var children in boxesByIdChildren)
            {
                SmartCanvasInputAnswerOption smartCanvasInputAnswerOption = new();
                string pageName = children.ToString();
                LessonpodHelperMethodRequest methodRequest = new()
                {
                    Jobject = lessonJson,
                    FirstAttribute = LessonpodConstant.Present,
                    SecondAttribute = LessonpodConstant.BoxesById,
                    ThirdAttribute = boxId,
                    FourthAttribute = LessonpodConstant.SortableContainers,
                    FifthAttribute = pageName,
                    SixthAttribute = LessonpodConstant.Children,
                    Index = 0
                };
                
                string pageId = DistributionCommonHelper.JObjectToString(methodRequest);
                JObject slidePluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pageId);
                string inputType = DistributionCommonHelper.JObjectToString(slidePluginToolbarsById, LessonpodConstant.State, LessonpodConstant.InputType);
                smartCanvasInputAnswerOption.InputType = inputType;
                JArray boxesByIdPage = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Children);
                int exerciseWeight = 0;
                foreach (var page in boxesByIdPage)
                {
                    string boxPageName = page.ToString();
                    LessonpodHelperMethodRequest boxMethodRequest = new()
                    {
                        Jobject = lessonJson,
                        FirstAttribute = LessonpodConstant.Present,
                        SecondAttribute = LessonpodConstant.BoxesById,
                        ThirdAttribute = pageId,
                        FourthAttribute = LessonpodConstant.SortableContainers,
                        FifthAttribute = boxPageName,
                        SixthAttribute = LessonpodConstant.Children,
                        Index = 0
                    };
                    string boxPageId = DistributionCommonHelper.JObjectToString(boxMethodRequest);
                    JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxPageId);
                    string pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                    if (pluginId == ServiceConstant.ComSCBasicText)
                    {
                        smartCanvasInputAnswerOption.QuestionText = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
                    }
                    if (pluginId == ServiceConstant.ComSBasicText)
                    {
                        switch (inputType)
                        {
                            case LessonpodConstant.InputTypeDropdown:
                                JArray optionData = (JArray)pluginToolbarsById[LessonpodConstant.State]![LessonpodConstant.Data]!;
                                var answerData = DropdownJsonAnswerList(optionData, exerciseWeight);
                                exerciseWeight = answerData.Item2;
                                smartCanvasInputAnswerOption.Data = answerData.Item1;
                                totalScore += exerciseWeight;
                                smartCanvasInputAnswerOption.Score = exerciseWeight;
                                break;
                            case LessonpodConstant.InputTypeInput:
                                JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxPageId);
                                smartCanvasInputAnswerOption.CorrectAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
                                smartCanvasInputAnswerOption.CaseSensitive = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Characters);
                                int score = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.Weight);
                                smartCanvasInputAnswerOption.Score = score;
                                totalScore += score;
                                break;
                            default:
                                break;
                        }
                        
                        string textInkType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextInkType);
                        
                        SmartCanvasInputAnswerLabel(inputType, textInkType, pluginToolbarsById, ref smartCanvasInputAnswerOption);
                        
                        JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Position);
                        smartCanvasInputAnswerOption.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Priority);                        
                    }                    
                }
                smartCanvasInputAnswerOption.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
                smartCanvasInputAnswerOption.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
                smartCanvasInputAnswerOption.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
                smartCanvasInputAnswerOption.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
                smartCanvasInputAnswerOption.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

                smartCanvasInputAnswerOption.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
                smartCanvasInputAnswerOption.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
                smartCanvasInputAnswerOption.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: slidePluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

                listOption.Add(smartCanvasInputAnswerOption);
            }
            response.Score = totalScore;
            response.LayoutType = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LayoutType);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);
            response.Options = listOption;            

            SlideTotalPoints += totalScore;
            #endregion

            return response;
        }
        #endregion

        #region SmartCanvasInputAnswerLabel
        private static void SmartCanvasInputAnswerLabel(string inputType, string textInkType, JObject pluginToolbarsById, ref SmartCanvasInputAnswerOption smartCanvasInputAnswerOption)
        {
            if (inputType == LessonpodConstant.InputTypeLabel && textInkType == LessonpodConstant.TextInkTypeText)
            {
                smartCanvasInputAnswerOption.TextInkType = textInkType;
                smartCanvasInputAnswerOption.CorrectAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Text);
                smartCanvasInputAnswerOption.CaseSensitive = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Characters);
            }

            if (inputType == LessonpodConstant.InputTypeLabel && textInkType == LessonpodConstant.TextInkTypeInkToText)
            {
                smartCanvasInputAnswerOption.TextInkType = textInkType;
                smartCanvasInputAnswerOption.CorrectAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
                smartCanvasInputAnswerOption.CaseSensitive = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Characters);
            }
        }
        #endregion

        #region ArrangeOrderJson
        private static SmartCanvasArangeOrderResponse ArrangeOrderJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartCanvasArangeOrderResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            List<OptionControlResponse> listOption = [];
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    OptionControlResponse optionControl = new()
                    {
                        Option = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text)
                    };
                    listOption.Add(optionControl);
                }
            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.Delay = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Delay);            
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.BoxPosition);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionHeight);

            string parentBoxId = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Parent);
            if (parentBoxId != "")
            {
                JObject parentPluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: parentBoxId);
                JObject parentBoxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: parentBoxId);
                
                response.X = DistributionCommonHelper.JObjectToString(parentBoxesById, LessonpodConstant.Position, LessonpodConstant.X);
                response.Y = DistributionCommonHelper.JObjectToString(parentBoxesById, LessonpodConstant.Position, LessonpodConstant.Y);
                response.Height = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
                response.Width = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
                response.Priority = DistributionCommonHelper.JObjectToInteger(parentBoxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            }
            else
            {
                response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
                response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
                response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
                response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
                response.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Position, LessonpodConstant.Priority);
            }            
            
            response.CategoryTitle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CategoryTitle);
            response.NumberOfOptions = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfOption);
            response.AnswerUI = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerUi);
            response.FailureExternalLink1 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink1);
            response.FailureExternalLink2 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink2);
            response.FailureExternalLink3 = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FailureExternalLink3);
            response.TextAlign = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.TextAlign);
            response.LayoutType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.LayoutType);

            List<int> listAnswer =
            [
                DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.CorrectAnswer)
            ];

            response.Answers = listAnswer;

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);

            SlideTotalPoints += Weight;
            #endregion


            return response;
        }
        #endregion

        #region AnswerPadJson
        private static BaseControlResponse AnswerPadJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes
            response.Priority = DistributionCommonHelper.JObjectToInteger(jObject: boxesById, firstAttribute: LessonpodConstant.Position, secondAttribute: LessonpodConstant.Priority);
            response.BoxPosition = DistributionCommonHelper.JObjectToInteger(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.BoxPosition);

            float Weight = DistributionCommonHelper.JObjectToFloat(jObject: exercises, attribute: LessonpodConstant.Weight);
            response.Score = Weight;

            response.CorrectAnswer = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Url);

            response.SpeechData = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechData));
            response.Success = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Success));
            response.Failure1 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure1));
            response.Failure2 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure2));
            response.Failure3 = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Failure3));
            response.FeedbackOptions = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Options));

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            SlideTotalPoints += Weight;

            return response;
        }
        #endregion

        #endregion

        #region Control Json

        #region TextListControlJson
        private static SmartPaperInputControlResponse TextListControlJson(JObject lessonJson, string boxId)
        {
            #region Declaration            
            SmartPaperInputControlResponse response = new();
            List<SmartPaperInputControlDetailsResponse> listSmartPaperInputControlDetails = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            #endregion

            #region Set values
            int optionValue = 0;
            int exerciseWeight = 0;
            JArray objData = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Data);
            foreach (var item in objData)
            {
                if ((item[1] != null && item[1]!.ToString() != ""))
                {
                    int.TryParse(item[1]!.ToString(), out optionValue);
                }
                int exerciseValue = optionValue;
                if (exerciseWeight < exerciseValue)
                {
                    exerciseWeight = exerciseValue;
                }

                SmartPaperInputControlDetailsResponse smartPaperInputControlDetails = new()
                {
                    OptionData = (item[0] != null && item[0]!.ToString() != "") ? item[0]!.ToString() : "",
                    OptionValue = optionValue
                };
                
                listSmartPaperInputControlDetails.Add(smartPaperInputControlDetails);
            }

            response.InputControlDetails = listSmartPaperInputControlDetails;
            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.TotalPoints = exerciseWeight;
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            #endregion

            return response;
        }
        #endregion

        #region InputControlJson
        private static SmartPaperInputControlResponse InputControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartPaperInputControlResponse response = new();
            List<SmartPaperInputControlDetailsResponse> listSmartPaperInputControlDetails = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set control details value
            SmartPaperInputControlDetailsResponse smartPaperInputControlDetails = new()
            {
                ControlType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Type),
                CorrectValue = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value),
                MinValue = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MinValue),
                MaxValue = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.MaxValue)
            };
            listSmartPaperInputControlDetails.Add(smartPaperInputControlDetails);
            #endregion

            #region Set control values
            float.TryParse(DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X).Replace(LessonpodConstant.Percentage, ""), out float X);
            float.TryParse(DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y).Replace(LessonpodConstant.Percentage, ""), out float Y);
            float Left = (StudioEditorWidth * X) / 100;
            float Top = (StudioEditorHeight * Y) / 100;

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Left = Left;
            response.Top = Top;
            response.InputControlDetails = listSmartPaperInputControlDetails;
            #endregion

            return response;
        }
        #endregion

        #region SmartPaperInputControlJson        
        private static SmartPaperInputControlResponse SmartPaperInputControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Set control values
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight)
            };
            #endregion

            return response;
        }
        #endregion

        #region HotspotControlJson
        private static SmartPaperInputControlResponse HotspotControlJson(JObject lessonJson, string boxId)
        {
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            #region Score
            int totalPoints = 0;
            bool isMultipleQuestion = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IsMultipleQuestion);
            JObject marksById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById);
            foreach (var markItem in marksById)
            {
                string marker = markItem.ToString();
                int index = marker.IndexOf(',');
                string markId = marker.Substring(1, index - 1).ToString();
                JObject mark = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById, thirdAttribute: markId);
                string origin = DistributionCommonHelper.JObjectToString(jObject: mark, attribute: LessonpodConstant.Origin);
                if (origin == boxId)
                {
                    int hotSpotScore = DistributionCommonHelper.JObjectToInteger(mark, LessonpodConstant.Score);
                    if (isMultipleQuestion)
                    {
                        totalPoints += hotSpotScore;
                    }
                    else
                    {
                        totalPoints = hotSpotScore;
                    }
                }
            }
            #endregion

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = totalPoints
            };

            return response;
        }
        #endregion

        #region H5PControlJson        
        private static SmartPaperInputControlResponse H5PControlJson(JObject lessonJson, string boxId)
        {
            #region Set control values
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Score)
            };
            #endregion

            return response;
        }
        #endregion

        #region InkListControlJson        
        private static SmartPaperInputControlResponse InkListControlJson(JObject lessonJson, string boxId)
        {
            #region Set control values
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            #region Score
            int totalPoints = 0;
            JArray data = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Data);
            foreach (var item in data)
            {
                if (item.ToString() != "[]")
                {
                    int exerciseValue = (int)item[1]!;
                    if (totalPoints < exerciseValue)
                    {
                        totalPoints = exerciseValue;
                    }
                }
            }
            #endregion

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = totalPoints
            };
            #endregion

            return response;
        }
        #endregion

        #region DropdownControlJson        
        private static SmartPaperInputControlResponse DropdownControlJson(JObject lessonJson, string boxId)
        {
            #region Set control values
            JObject parentPluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            int totalScore = 0;
            foreach (var children in boxesByIdChildren)
            {
                int exerciseWeight = 0;
                string pageName = children.ToString();
                LessonpodHelperMethodRequest methodRequest = new()
                {
                    Jobject = lessonJson,
                    FirstAttribute = LessonpodConstant.Present,
                    SecondAttribute = LessonpodConstant.BoxesById,
                    ThirdAttribute = boxId,
                    FourthAttribute = LessonpodConstant.SortableContainers,
                    FifthAttribute = pageName,
                    SixthAttribute = LessonpodConstant.Children,
                    Index = 0
                };
                string pageId = DistributionCommonHelper.JObjectToString(methodRequest);
                JArray boxesByIdPage = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Children);
                foreach (var page in boxesByIdPage)
                {
                    string boxPageName = page.ToString();
                    LessonpodHelperMethodRequest boxMethodRequest = new()
                    {
                        Jobject = lessonJson,
                        FirstAttribute = LessonpodConstant.Present,
                        SecondAttribute = LessonpodConstant.BoxesById,
                        ThirdAttribute = pageId,
                        FourthAttribute = LessonpodConstant.SortableContainers,
                        FifthAttribute = boxPageName,
                        SixthAttribute = LessonpodConstant.Children,
                        Index = 0
                    };
                    string boxPageId = DistributionCommonHelper.JObjectToString(boxMethodRequest);
                    JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxPageId);
                    string pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                    if (pluginId == ServiceConstant.ComSCAGTextList)
                    {
                        JArray optionData = (JArray)pluginToolbarsById[LessonpodConstant.State]![LessonpodConstant.Data]!;
                        foreach (var data in optionData)
                        {
                            int exerciseValue = (int)data[1]!;
                            if (exerciseWeight < exerciseValue)
                            {
                                exerciseWeight = exerciseValue;
                            }
                        }
                        totalScore += exerciseWeight;
                    }
                }
            }

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = totalScore
            };
            #endregion

            return response;
        }
        #endregion

        #region InputAnswerControlJson        
        private static SmartPaperInputControlResponse InputAnswerControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Set control values
            JObject parentPluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            int totalScore = 0;
            foreach (var children in boxesByIdChildren)
            {
                string pageName = children.ToString();
                LessonpodHelperMethodRequest methodRequest = new()
                {
                    Jobject = lessonJson,
                    FirstAttribute = LessonpodConstant.Present,
                    SecondAttribute = LessonpodConstant.BoxesById,
                    ThirdAttribute = boxId,
                    FourthAttribute = LessonpodConstant.SortableContainers,
                    FifthAttribute = pageName,
                    SixthAttribute = LessonpodConstant.Children,
                    Index = 0
                };
                string pageId = DistributionCommonHelper.JObjectToString(methodRequest);
                JArray boxesByIdPage = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pageId, fourthAttribute: LessonpodConstant.Children);
                foreach (var page in boxesByIdPage)
                {
                    string boxPageName = page.ToString();
                    LessonpodHelperMethodRequest boxMethodRequest = new()
                    {
                        Jobject = lessonJson,
                        FirstAttribute = LessonpodConstant.Present,
                        SecondAttribute = LessonpodConstant.BoxesById,
                        ThirdAttribute = pageId,
                        FourthAttribute = LessonpodConstant.SortableContainers,
                        FifthAttribute = boxPageName,
                        SixthAttribute = LessonpodConstant.Children,
                        Index = 0
                    };
                    string boxPageId = DistributionCommonHelper.JObjectToString(boxMethodRequest);
                    JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxPageId);
                    string pluginId = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);
                    if (pluginId == ServiceConstant.ComSBasicText)
                    {
                        JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxPageId);
                        int score = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.Weight);
                        totalScore += score;
                    }
                }
            }

            SmartPaperInputControlResponse response = new()
            {
                PluginName = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.PluginId),
                ControlTag = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag),
                ControlName = DistributionCommonHelper.JObjectToString(parentPluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName),
                TotalPoints = totalScore
            };
            #endregion

            return response;
        }
        #endregion

        #endregion
    }
}
