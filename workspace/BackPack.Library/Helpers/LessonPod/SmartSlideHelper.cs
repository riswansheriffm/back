using BackPack.Library.Constants;
using BackPack.Library.Requests.LessonPod;
using BackPack.Library.Responses.LessonPod.Distribution;
using BackPack.Library.Responses.LessonPod.Distribution.SmartSlide;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class SmartSlideHelper
    {
        private static float SlideTotalPoints;
        private static int TotalFileSize;

        #region SmartSlideJson
        public static LessonPodSlideResponse SmartSlideJson(JObject lessonJson, string slideId)
        {
            #region Declaration
            LessonPodSlideResponse lessonUnitSlideResponse = new();
            SmartSlideResponse smartSlideResponse = new();
            List<object> listObject = [];
            List<SmartSlideInputControlResponse> listSmartSlideInputControl = [];

            int ControlID = 0;
            SlideTotalPoints = 0;
            TotalFileSize = 0;
            #endregion

            #region Slides
            JArray slideBoxes = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: "boxes");
            JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);
            foreach (var box in slideBoxes)
            {
                ControlID++;
                string boxId = box.ToString();
                JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
                string PluginName = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, attribute: LessonpodConstant.PluginId);

                #region HotspotImages
                if (PluginName == ServiceConstant.ComHotspotImages)
                {
                    BaseControlResponse baseControlResponse = HotspotImagesJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion

                #region EnrichedAudio
                if (PluginName == ServiceConstant.ComEnrichedAudio)
                {
                    BaseControlResponse baseControlResponse = EnrichedAudioJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion

                #region EnrichedPlayer
                if (PluginName == ServiceConstant.ComEnrichedPlayer)
                {
                    BaseControlResponse baseControlResponse = EnrichedPlayerJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion

                #region EnrichedPDF
                if (PluginName == ServiceConstant.ComEnrichedPDF)
                {
                    BaseControlResponse baseControlResponse = EnrichedPDFJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion

                #region Webpage
                if (PluginName == ServiceConstant.ComWebpage)
                {
                    BaseControlResponse baseControlResponse = WebpageJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion

                #region SSArrange
                if (PluginName == ServiceConstant.ComSSArrange)
                {
                    SmartSlideArrangeResponse baseControlResponse = ArrangeJson(lessonJson, boxId, slideId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = ArrangeControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSMatchParent
                if (PluginName == ServiceConstant.ComSSMatchParent)
                {
                    SmartSlideMatchParentResponse baseControlResponse = MatchParentJson(lessonJson, boxId, slideId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = MatchParentControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSCategory
                if (PluginName == ServiceConstant.ComSSCategory)
                {
                    SmartSlideCategoryResponse baseControlResponse = CategoryJson(lessonJson, boxId, slideId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = CategoryControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region DynamicCategory
                if (PluginName == ServiceConstant.ComDynamicCategory)
                {
                    SmartSlideDynamicCategoryResponse baseControlResponse = DynamicCategoryJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = DynamicCategoryControlJson(lessonJson, boxId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region Hotspot
                if (PluginName == ServiceConstant.ComHotspot)
                {
                    SmartSlideHotspotResponse baseControlResponse = HotspotJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = HotspotControlJson(lessonJson, boxId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region H5P
                if (PluginName == ServiceConstant.ComH5P)
                {
                    BaseControlResponse baseControlResponse = H5PJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginName = PluginName;
                    listObject.Add(baseControlResponse);

                    SmartSlideInputControlResponse smartSlideInputControl = H5PControlJson(lessonJson, boxId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSMultipleChoice
                if (PluginName == ServiceConstant.ComSSMultipleChoice)
                {
                    SmartSlideMultipleChoiceResponse smartSlideMultipleChoice = MultipleChoiceJson(lessonJson, boxId, slideId);
                    smartSlideMultipleChoice.ControlID = ControlID;
                    smartSlideMultipleChoice.PluginName = PluginName;
                    listObject.Add(smartSlideMultipleChoice);

                    SmartSlideInputControlResponse smartSlideInputControl = MultipleChoiceControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSMultipleAnswer
                if (PluginName == ServiceConstant.ComSSMultipleAnswer)
                {
                    SmartSlideMultipleAnswerResponse smartSlideMultipleAnswer = MultipleAnswerJson(lessonJson, boxId, slideId);
                    smartSlideMultipleAnswer.ControlID = ControlID;
                    smartSlideMultipleAnswer.PluginName = PluginName;
                    listObject.Add(smartSlideMultipleAnswer);

                    SmartSlideInputControlResponse smartSlideInputControl = MultipleAnswerControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSTrueFalse
                if (PluginName == ServiceConstant.ComSSTrueFalse)
                {
                    SmartSlideTrueFalseResponse smartSlideTrueFalse = TrueFalseJson(lessonJson, boxId, slideId);
                    smartSlideTrueFalse.ControlID = ControlID;
                    smartSlideTrueFalse.PluginName = PluginName;
                    listObject.Add(smartSlideTrueFalse);

                    SmartSlideInputControlResponse smartSlideInputControl = TrueFalseControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSRotatingDial
                if (PluginName == ServiceConstant.ComSSRotatingDial)
                {
                    SmartSlideRotatingDialResponse smartSlideRotatingDial = RotatingDialJson(lessonJson, boxId, slideId);
                    smartSlideRotatingDial.ControlID = ControlID;
                    smartSlideRotatingDial.PluginName = PluginName;
                    listObject.Add(smartSlideRotatingDial);

                    SmartSlideInputControlResponse smartSlideInputControl = RotatingDialControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region SSFillinTheBlanks
                if (PluginName == ServiceConstant.ComSSFillinTheBlanks)
                {
                    SmartSlideFillinTheBlanksResponse smartSlideFillinTheBlanks = FillinTheBlanksJson(lessonJson, boxId, slideId);
                    smartSlideFillinTheBlanks.ControlID = ControlID;
                    smartSlideFillinTheBlanks.PluginName = PluginName;
                    listObject.Add(smartSlideFillinTheBlanks);

                    SmartSlideInputControlResponse smartSlideInputControl = FillinTheBlanksControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region MatchTable
                if (PluginName == ServiceConstant.ComMatchTable)
                {
                    SmartSlideMatchTableResponse smartSlideMatchTable = MatchTableJson(lessonJson, boxId, slideId);
                    smartSlideMatchTable.ControlID = ControlID;
                    smartSlideMatchTable.PluginName = PluginName;
                    listObject.Add(smartSlideMatchTable);

                    SmartSlideInputControlResponse smartSlideInputControl = MatchTableControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region Fraction
                if (PluginName == ServiceConstant.ComFraction)
                {
                    SmartSlideFractionResponse smartSlideFraction = FractionJson(lessonJson, boxId, slideId);
                    smartSlideFraction.ControlID = ControlID;
                    smartSlideFraction.PluginName = PluginName;
                    listObject.Add(smartSlideFraction);

                    SmartSlideInputControlResponse smartSlideInputControl = FractionControlJson(lessonJson, boxId, slideId);
                    smartSlideInputControl.ControlID = ControlID;
                    listSmartSlideInputControl.Add(smartSlideInputControl);
                }
                #endregion

                #region Visor3D
                if (PluginName == ServiceConstant.ComVisor3D)
                {
                    BaseControlResponse baseControlResponse = Visor3DJson(lessonJson, boxId);
                    baseControlResponse.ControlID = ControlID;
                    baseControlResponse.PluginId = PluginName;
                    listObject.Add(baseControlResponse);
                }
                #endregion
            }
            #endregion

            #region Slide response
            smartSlideResponse.InputControls = listObject;
            smartSlideResponse.TotalScore = SlideTotalPoints;

            lessonUnitSlideResponse.SearchTag = DistributionCommonHelper.JObjectToString(jObject: navItemsById, attribute: "searchtag");
            lessonUnitSlideResponse.ImageURL = DistributionCommonHelper.JObjectToString(jObject: navItemsById, attribute: "activityImage");
            lessonUnitSlideResponse.SmartSlideInputControls = listSmartSlideInputControl;
            lessonUnitSlideResponse.FileSize = TotalFileSize;
            lessonUnitSlideResponse.SearchName = DistributionCommonHelper.JObjectToString(jObject: navItemsById, attribute: "searchname");
            lessonUnitSlideResponse.TotalPoints = SlideTotalPoints;
            lessonUnitSlideResponse.FileSize = TotalFileSize;
            string SmartSlideJson = JsonSerializer.Serialize(smartSlideResponse);
            lessonUnitSlideResponse.SlideJson = SmartSlideJson;
            #endregion

            return lessonUnitSlideResponse;
        }
        #endregion

        #region Slide Json

        #region HotspotImagesJson
        private static BaseControlResponse HotspotImagesJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
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
            #endregion

            #region Set attributes value
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);

            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
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
            #endregion

            #region Set attributes value
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.VideoSkip = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, "videoSkip");

            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            #endregion

            return response;
        }
        #endregion

        #region EnrichedPDFJson
        private static BaseControlResponse EnrichedPDFJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            BaseControlResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);

            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
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
            #endregion

            #region Set attributes value
            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);

            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            #endregion

            return response;
        }
        #endregion

        #region ArrangeJson
        private static SmartSlideArrangeResponse ArrangeJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideArrangeResponse response = new();
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
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
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

            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

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
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);

            response.Answers = listOption;
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region MatchParentJson
        private static SmartSlideMatchParentResponse MatchParentJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideMatchParentResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            List<SmartSlideMatchingExerciseOptionsResponse> listOption = [];
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) break;
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    SmartSlideMatchingExerciseOptionResponse optionLeft = new();
                    SmartSlideMatchingExerciseOptionResponse optionRight = new();
                    SmartSlideMatchingExerciseOptionsResponse options = new();
                    optionLeft.OptionValue = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, "topsrc");
                    optionRight.OptionValue = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, "bottomsrc");
                    options.OptionLeft = optionLeft;
                    options.OptionRight = optionRight;
                    listOption.Add(options);
                }
            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

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
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region CategoryJson
        private static SmartSlideCategoryResponse CategoryJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideCategoryResponse response = new();
            List<SmartSlideCategoryOptionResponse> listOption = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject categoryExercise = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options            
            float scores = 0;
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer.Contains(LessonpodConstant.ScQuestion))
                {
                    SmartSlideCategoryQuestionResponse categoryQuestion = new();
                    List<SmartSlideCategoryAnswerResponse> listCategoryAnswer = new();                    
                    JArray questionBoxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.Children);
                    foreach (var questionItem in questionBoxesByIdChildren)
                    {
                        string TagCategory = (string)questionItem!;
                        LessonpodHelperMethodRequest methodRequest = new()
                        {
                            Jobject = lessonJson,
                            FirstAttribute = LessonpodConstant.Present,
                            SecondAttribute = LessonpodConstant.BoxesById,
                            ThirdAttribute = pluginToolbarsChildrenID,
                            FourthAttribute = LessonpodConstant.SortableContainers,
                            FifthAttribute = TagCategory,
                            SixthAttribute = LessonpodConstant.Children,
                            Index = 0
                        };
                        string pluginToolbarsCategoryChildrenID = DistributionCommonHelper.JObjectToString(methodRequest);
                        if (TagCategory == LessonpodConstant.ScQuestion)
                        {
                            categoryQuestion.Value = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                            float questionScore = DistributionCommonHelper.JObjectToFloat(categoryExercise, pluginToolbarsChildrenID, LessonpodConstant.Weight);
                            categoryQuestion.Score = questionScore;
                            scores += questionScore;
                        }
                        if (TagCategory.Contains("sc-Answer"))
                        {
                            SmartSlideCategoryAnswerResponse categoryAnswer = new();
                            categoryAnswer.Key = pluginToolbarsCategoryChildrenID;
                            categoryAnswer.Value = (answerType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                            categoryAnswer.IsDecoy = DistributionCommonHelper.JObjectToBoolean(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, "decoy");
                            listCategoryAnswer.Add(categoryAnswer);
                        }
                    }

                    SmartSlideCategoryOptionResponse categoryOption = new()
                    {
                        Question = categoryQuestion,
                        Answers = listCategoryAnswer
                    };

                    listOption.Add(categoryOption);
                }

                if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);

            }

            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;
            #endregion

            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);
            response.NoOfColumns = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, "NoOfColumns");

            response.Score = scores;

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
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += scores;
            #endregion

            return response;
        }
        #endregion

        #region DynamicCategoryJson
        private static SmartSlideDynamicCategoryResponse DynamicCategoryJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            float Score = 0;
            SmartSlideDynamicCategoryResponse response = new();
            List<SmartSlideDynamicCategorizeArrayResponse> listDynamicCategorizeArray = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject marksById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById);
            #endregion
            
            #region HotSpot
            foreach (var markItem in marksById)
            {
                string marker = markItem.ToString();
                int index = marker.IndexOf(',');
                string markId = marker.Substring(1, index - 1).ToString();
                JObject mark = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById, thirdAttribute: markId);
                string origin = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Origin);
                if (origin == boxId)
                {
                    SmartSlideDynamicCategorizeArrayResponse dynamicCategorizeArray = new();
                    string positions = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Value);
                    if (positions != "")
                    {
                        string[] position = positions.Split(',');
                        dynamicCategorizeArray.X = position[0].ToString();
                        dynamicCategorizeArray.Y = position[1].ToString();
                    }
                    
                    List<SmartSlideDynamicCategorizeAnswerResponse> listDynamicCategorizeAnswer = [];
                    JArray objAnswers = DistributionCommonHelper.JObjectToJArray(jObject: mark, firstAttribute: "answerlist");
                    JArray objCheckbox = DistributionCommonHelper.JObjectToJArray(jObject: mark, firstAttribute: "checkboxChecked");
                    for (int answerIndex = 0; answerIndex < objAnswers.Count(); answerIndex++)
                    {
                        SmartSlideDynamicCategorizeAnswerResponse dynamicCategorizeAnswer = new()
                        {
                            Answer = objAnswers[answerIndex].ToString(),
                            IsDecoy = (bool)objCheckbox[answerIndex]
                        };
                        
                        listDynamicCategorizeAnswer.Add(dynamicCategorizeAnswer);
                    }

                    dynamicCategorizeArray.Answers = listDynamicCategorizeAnswer;
                    dynamicCategorizeArray.ID = DistributionCommonHelper.JObjectToString(mark, "id");
                    dynamicCategorizeArray.AnswerLabel = DistributionCommonHelper.JObjectToString(mark, "answer");
                    dynamicCategorizeArray.Color = DistributionCommonHelper.JObjectToString(mark, "color");
                    float CategoryScore = DistributionCommonHelper.JObjectToFloat(mark, LessonpodConstant.Score);
                    dynamicCategorizeArray.Score = CategoryScore;
                    Score += CategoryScore;
                    dynamicCategorizeArray.Width = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Width);
                    dynamicCategorizeArray.Height = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Height);
                    dynamicCategorizeArray.ColumnCount = DistributionCommonHelper.JObjectToInteger(mark, "displayCount");
                    listDynamicCategorizeArray.Add(dynamicCategorizeArray);
                }
            }
            response.HotSpot = listDynamicCategorizeArray;
            #endregion

            #region Set attributes value
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            response.Score = Score;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.EditorImageWidth = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Width);
            response.EditorImageHeight = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Height);
            response.QuestionText = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "categoryTitle");
            response.AnswerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Score;
            #endregion

            return response;
        }
        #endregion

        #region HotspotJson
        private static SmartSlideHotspotResponse HotspotJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            float Score = 0;
            SmartSlideHotspotResponse response = new();
            List<HotSpotArrayResponse> listHotSpotArray = [];
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject marksById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.MarksById);
            #endregion

            #region HotSpot
            bool IsMultipleQuestion = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, "isMultiplequestion");
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

                    hotSpotArray.ID = DistributionCommonHelper.JObjectToString(mark, "id");
                    hotSpotArray.Question = DistributionCommonHelper.JObjectToString(mark, "question");
                    hotSpotArray.AnswerLabel = DistributionCommonHelper.JObjectToString(mark, "answer");
                    hotSpotArray.Color = DistributionCommonHelper.JObjectToString(mark, "color");
                    float hotSpotScore = DistributionCommonHelper.JObjectToFloat(mark, LessonpodConstant.Score);
                    hotSpotArray.Score = hotSpotScore;
                    hotSpotArray.Width = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Width);
                    hotSpotArray.Height = DistributionCommonHelper.JObjectToString(mark, LessonpodConstant.Height);
                    if (IsMultipleQuestion)
                    {
                        Score += hotSpotScore;
                    }
                    else
                    {
                        Score = hotSpotScore;
                    }
                    listHotSpotArray.Add(hotSpotArray);
                }
            }

            response.HotSpot = listHotSpotArray;
            #endregion

            #region Set attributes value
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            response.Score = Score;

            response.IsMultipleQuestion = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, "isMultiplequestion");
            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.EditorImageWidth = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Width);
            response.EditorImageHeight = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Height);
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "display");
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.BackgroundOverlay = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "backgroundOverlay");

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Score;

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
            #endregion

            #region Set attributes value
            float Score = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Score);
            response.Score = Score;

            response.Url = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.LibraryName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "libraryName");

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Score;
            #endregion

            return response;
        }
        #endregion

        #region MultipleChoiceJson
        private static SmartSlideMultipleChoiceResponse MultipleChoiceJson(JObject lessonJson, string boxId, string slideId)
        {
            SmartSlideMultipleChoiceResponse response = new();

            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            string questionType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.AnswerType);

            #region Answers
            List<float> listAnswer = [(float)exercises[LessonpodConstant.CorrectAnswer]!];
            response.Answers = listAnswer;
            #endregion

            #region Question and Options

            List<OptionControlResponse> listOption = [];
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                    OptionControlResponse optionControl = new()
                    {
                        Option = (answerType == "text") ? DistributionCommonHelper.JObjectToString(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.State, fifthAttribute: LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.State, fifthAttribute: LessonpodConstant.Url)
                    };                    
                    
                    listOption.Add(optionControl);
                }
            }
            #endregion

            #region Set control properties
            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;

            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);
            response.NumberOfOptions = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, "noOfOption");

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "QuestionHeight");

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.CategoryTitle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "CategoryTitle");
            response.AnswerUI = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "answerUI");


            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region MultipleAnswerJson
        private static SmartSlideMultipleAnswerResponse MultipleAnswerJson(JObject lessonJson, string boxId, string slideId)
        {
            SmartSlideMultipleAnswerResponse response = new();

            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            string questionType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.AnswerType);

            #region Question and Options

            List<OptionControlResponse> listOption = [];
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else
                {
                    pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                    OptionControlResponse optionControl = new()
                    {
                        Option = (answerType == "text") ? DistributionCommonHelper.JObjectToString(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.State, fifthAttribute: LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.State, fifthAttribute: LessonpodConstant.Url)
                    };

                    listOption.Add(optionControl);
                }
            }
            #endregion

            #region Set control properties
            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;

            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "QuestionHeight");

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.Answers = DistributionCommonHelper.JArrayToFloatList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region TrueFalseJson
        private static SmartSlideTrueFalseResponse TrueFalseJson(JObject lessonJson, string boxId, string slideId)
        {
            SmartSlideTrueFalseResponse response = new();

            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            string questionType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.AnswerType);

            #region Question and Options

            List<string> listOption = [];
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else listOption.Add(DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text));
            }
            #endregion

            #region Set control properties
            response.Options = listOption;
            response.QuestionType = questionType;
            response.AnswerType = answerType;

            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.QuestionHeight = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "QuestionHeight");

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.Answers = DistributionCommonHelper.JArrayToBooleanList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region RotatingDialJson
        private static SmartSlideRotatingDialResponse RotatingDialJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideRotatingDialResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            string questionType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "type");
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            response.Type = answerType;
            #endregion
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;
            response.MinRange = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "minValue");
            response.MaxRange = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "maxValue");
            response.MinValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "initialValue");
            response.MaxValue = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "finalValue");
            response.StepCount = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, "stepCount");
            String displayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);
            response.Answers = (displayType != "TypeAnswer") ? DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value) : 0;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
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
            response.TypeAnswer = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);
            response.DisplayType = displayType;

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            return response;
        }
        #endregion

        #region FillinTheBlanksJson
        private static SmartSlideFillinTheBlanksResponse FillinTheBlanksJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideFillinTheBlanksResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value           

            #region Question and options
            string questionType = DistributionCommonHelper.JObjectToString(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.QuestionType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);

            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            #endregion

            #region Blank array
            JArray blankArray = DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "blankArray");
            List<FillinTheBlanksBlankArrayResponse> listFillinTheBlanksBlankArray = [];
            foreach (var blankItem in blankArray)
            {
                FillinTheBlanksBlankArrayResponse fillinTheBlanksBlankArray = new();
                JObject blank = (JObject)blankItem;

                int Blank = DistributionCommonHelper.JObjectToInteger(jObject: blank, attribute: "blank");
                fillinTheBlanksBlankArray.Blank = Blank;
                fillinTheBlanksBlankArray.Option = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: blank, firstAttribute: "option"));
                listFillinTheBlanksBlankArray.Add(fillinTheBlanksBlankArray);
            }

            response.BlankArray = listFillinTheBlanksBlankArray;
            #endregion
            response.NoOfLinesA = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesA);
            response.NoOfLinesQ = DistributionCommonHelper.JObjectToInteger(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.NoOfLinesQ);

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
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
            response.DisplayType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.DisplayType);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.Answers = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "valueArray"));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));

            SlideTotalPoints += Weight;

            #endregion

            return response;
        }
        #endregion

        #region MatchTableJson
        private static SmartSlideMatchTableResponse MatchTableJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration            
            SmartSlideMatchTableResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value

            #region Question and Options
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            #endregion

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.BorderColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "borderColor");
            response.QuestionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.Type = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "type");
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);

            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));
            response.ColQuestion = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "colQuestion"));
            response.RowAnswer = DistributionCommonHelper.JArrayToStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "rowAnswer"));
            response.Answers = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: "answer"));
            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            
            SlideTotalPoints += Weight;
            #endregion

            return response;
        }
        #endregion

        #region FractionJson
        private static SmartSlideFractionResponse FractionJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideFractionResponse response = new();
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Set attributes value

            #region Question and Options
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.QuestionText = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Description = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }

            response.QuestionType = questionType;
            #endregion

            float Weight = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            response.Score = Weight;

            response.Bold = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Bold);
            response.Italic = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Italic);

            response.Background = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Background);
            response.Feedback = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ShowFeedback);
            response.AnswerFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFontSize);
            response.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SequenceMessage);
            response.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.CorrectAnswerExplanation);
            response.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.IncorrectAnswerExplanation);
            response.Opacity = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Opacity);
            response.FontStyle = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.FontStyle);
            response.BorderColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "borderColor");
            response.LayoutType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "type");
            response.SelectionColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.SelectionColor);
            response.AnswerColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerColor);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);
            response.Answers = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Value);
            response.QuestionColorFill = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionColor);
            response.QuestionFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionFillColor);
            response.QuestionFontSize = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestFontSize);
            response.AnswerFillColor = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerFillColor);

            response.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.TextToSpeech));
            response.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.Seconds));

            SlideTotalPoints += Weight;

            response.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.SpeechAudioData);
            response.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.OptionsAudioData);
            response.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsById, firstAttribute: LessonpodConstant.State, secondAttribute: LessonpodConstant.FeedbackAudioData);
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
            response.X = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.X);
            response.Y = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Position, LessonpodConstant.Y);
            response.ObjectUrl = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Url);
            response.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Height);
            response.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.Structure, LessonpodConstant.Width);
            response.TextureUrl = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, "imgurl");
            #endregion

            return response;
        }
        #endregion

        #endregion

        #region Controls Json

        #region ArrangeControlJson
        private static SmartSlideInputControlResponse ArrangeControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            #region Question and Options
            int answerIndex = 0;
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.Question = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) break;
                else
                {
                    SmartSlideInputControlDetailResponse smartSlideInputControlDetail = new();
                    smartSlideInputControlDetail.OptionData = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                    smartSlideInputControlDetail.OptionIndex = answerIndex;
                    answerIndex++;
                    listSmartSlideInputControlDetail.Add(smartSlideInputControlDetail);
                }
            }
            #endregion

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region MatchParentControlJson
        private static SmartSlideInputControlResponse MatchParentControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            #region Question and Options
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScFeedback) response.Question = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
            }
            #endregion

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region CategoryControlJson
        private static SmartSlideInputControlResponse CategoryControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject categoryExercise = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);

            #region Question and Options
            float scores = 0;
            string questionType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.QuestionType);
            string answerType = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.AnswerType);
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer.Contains(LessonpodConstant.ScQuestion))
                {
                    JArray questionBoxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: pluginToolbarsChildrenID, fourthAttribute: LessonpodConstant.Children);
                    foreach (var questionItem in questionBoxesByIdChildren)
                    {
                        string TagCategory = (string)questionItem!;
                        LessonpodHelperMethodRequest methodRequest = new()
                        {
                            Jobject = lessonJson,
                            FirstAttribute = LessonpodConstant.Present,
                            SecondAttribute = LessonpodConstant.BoxesById,
                            ThirdAttribute = pluginToolbarsChildrenID,
                            FourthAttribute = LessonpodConstant.SortableContainers,
                            FifthAttribute = TagCategory,
                            SixthAttribute = LessonpodConstant.Children,
                            Index = 0
                        };
                        string pluginToolbarsCategoryChildrenID = DistributionCommonHelper.JObjectToString(methodRequest);
                        if (TagCategory == LessonpodConstant.ScQuestion)
                        {
                            response.Question = (questionType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Url);
                            float questionScore = DistributionCommonHelper.JObjectToFloat(categoryExercise, pluginToolbarsChildrenID, LessonpodConstant.Weight);
                            scores += questionScore;
                        }
                        if (TagCategory.Contains("sc-Answer"))
                        {
                            SmartSlideInputControlDetailResponse categoryAnswer = new()
                            {
                                OptionData = (answerType == "text") ? DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Text) : DistributionCommonHelper.JObjectToString(question, pluginToolbarsCategoryChildrenID, LessonpodConstant.State, LessonpodConstant.Url)
                            };
                            
                            listSmartSlideInputControlDetail.Add(categoryAnswer);
                        }
                    }
                }
            }
            response.TotalPoints = scores;
            #endregion

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region DynamicCategoryControlJson
        private static SmartSlideInputControlResponse DynamicCategoryControlJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            #endregion

            #region Mark
            float totalPoints = 0;
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
                    float categoryScore = DistributionCommonHelper.JObjectToFloat(mark, LessonpodConstant.Score);
                    totalPoints += categoryScore;
                }
            }
            response.TotalPoints = totalPoints;
            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region HotspotControlJson
        private static SmartSlideInputControlResponse HotspotControlJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            #endregion

            #region Mark
            float totalPoints = 0;
            bool isMultipleQuestion = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsById, LessonpodConstant.State, "isMultiplequestion");
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
                    float hotSpotScore = DistributionCommonHelper.JObjectToFloat(mark, LessonpodConstant.Score);
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
            response.TotalPoints = totalPoints;
            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region H5PControlJson
        private static SmartSlideInputControlResponse H5PControlJson(JObject lessonJson, string boxId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.Score);

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region MultipleChoiceControlJson
        private static SmartSlideInputControlResponse MultipleChoiceControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);
            int correctAnswerIndex = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.CorrectAnswer);

            int answerIndex = 0;
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.Question = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback)
                {
                    SmartSlideInputControlDetailResponse smartSlideInputControlDetailResponse = new()
                    {
                        OptionData = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text),
                        OptionValue = (correctAnswerIndex == answerIndex) ? 1 : 0,
                        OptionIndex = answerIndex
                    };
                    
                    listSmartSlideInputControlDetail.Add(smartSlideInputControlDetailResponse);
                    answerIndex++;
                }
            }
            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region MultipleAnswerControlJson
        private static SmartSlideInputControlResponse MultipleAnswerControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            int answerIndex = 0;
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.Question = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) break;
                else
                {
                    bool isCorrectAnswer = false;
                    SmartSlideInputControlDetailResponse smartSlideInputControlDetailResponse = new();
                    
                    var listCorrectAnswer = DistributionCommonHelper.JArrayToFloatList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));
                    foreach (var correctAnswer in listCorrectAnswer)
                    {
                        if (correctAnswer == answerIndex)
                        {
                            isCorrectAnswer = true;
                            break;
                        }
                    }
                    smartSlideInputControlDetailResponse.OptionData = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                    smartSlideInputControlDetailResponse.OptionValue = isCorrectAnswer ? 1 : 0;
                    smartSlideInputControlDetailResponse.OptionIndex = answerIndex;
                    listSmartSlideInputControlDetail.Add(smartSlideInputControlDetailResponse);
                    answerIndex++;
                }
            }
            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region TrueFalseControlJson
        private static SmartSlideInputControlResponse TrueFalseControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
            JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            int answerIndex = 0;
            JArray boxesByIdChildren = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Children);
            JObject question = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById);
            foreach (var item in boxesByIdChildren)
            {
                string tagMultipleAnswer = (string)item!;
                string pluginToolbarsChildrenID = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.SortableContainers, tagMultipleAnswer, LessonpodConstant.Children, 0);
                if (tagMultipleAnswer == LessonpodConstant.ScQuestion) response.Question = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                else if (tagMultipleAnswer == LessonpodConstant.ScFeedback) break;
                else
                {
                    bool isCorrectAnswer = false;
                    SmartSlideInputControlDetailResponse smartSlideInputControlDetailResponse = new();
                    
                    var listCorrectAnswer = DistributionCommonHelper.JArrayToFloatList(DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId, sixthAttribute: LessonpodConstant.CorrectAnswer));
                    foreach (var correctAnswer in listCorrectAnswer)
                    {
                        if (correctAnswer == answerIndex)
                        {
                            isCorrectAnswer = true;
                            break;
                        }
                    }
                    smartSlideInputControlDetailResponse.OptionData = DistributionCommonHelper.JObjectToString(question, pluginToolbarsChildrenID, LessonpodConstant.State, LessonpodConstant.Text);
                    smartSlideInputControlDetailResponse.OptionValue = isCorrectAnswer ? 1 : 0;
                    smartSlideInputControlDetailResponse.OptionIndex = answerIndex;
                    listSmartSlideInputControlDetail.Add(smartSlideInputControlDetailResponse);
                    answerIndex++;
                }
            }
            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region RotatingDialControlJson
        private static SmartSlideInputControlResponse RotatingDialControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = [];
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region FillinTheBlanksControlJson
        private static SmartSlideInputControlResponse FillinTheBlanksControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region MatchTableControlJson
        private static SmartSlideInputControlResponse MatchTableControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #region FractionControlJson
        private static SmartSlideInputControlResponse FractionControlJson(JObject lessonJson, string boxId, string slideId)
        {
            #region Declaration
            SmartSlideInputControlResponse response = new();
            List<SmartSlideInputControlDetailResponse> listSmartSlideInputControlDetail = new();
            #endregion

            #region Control properties
            JObject pluginToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId);
            JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);

            response.PluginName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.PluginId);
            response.ControlTag = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlTag);
            response.ControlName = DistributionCommonHelper.JObjectToString(pluginToolbarsById, LessonpodConstant.State, LessonpodConstant.ControlName);
            response.TotalPoints = DistributionCommonHelper.JObjectToFloat(exercises, LessonpodConstant.Weight);

            response.InputControlDetails = listSmartSlideInputControlDetail;
            #endregion

            return response;
        }
        #endregion

        #endregion
    }
}
