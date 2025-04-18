using BackPack.Library.Constants;
using BackPack.Library.Responses.LessonPod.Distribution;
using BackPack.Library.Responses.LessonPod.Distribution.SmartLabel;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class SmartLabelHelper
    {
        #region SmartLabelJson        
        public static LessonPodSlideResponse SmartLabelJson(JObject lessonJson, string slideId)
        {
            #region Declaration            
            LessonPodSlideResponse lessonUnitSlideResponse = new();
            SmartLabelResponse smartLabelSlideResponse = new();
            List<SmartLabelBoxResponse> listSmartLabelBoxResponses = [];
            int ControlID = 0;
            int totalWeight = 0;
            string smartLabelJson = "";
            #endregion

            #region Label box properties            
            JArray slideBoxes = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Boxes);

            foreach (var box in slideBoxes)
            {
                ControlID++;
                SmartLabelBoxResponse smartLabelBoxResponse = new();               

                #region Box Properties
                string boxId = (string)box!;
                smartLabelBoxResponse.ControlID = ControlID;
                JObject boxesById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.BoxesById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Position);
                smartLabelBoxResponse.XAxis = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.X);
                smartLabelBoxResponse.YAxis = DistributionCommonHelper.JObjectToString(boxesById, LessonpodConstant.Y);
                smartLabelBoxResponse.Priority = DistributionCommonHelper.JObjectToInteger(boxesById, LessonpodConstant.Priority);

                JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
                smartLabelBoxResponse.CorrectAnswer = DistributionCommonHelper.JObjectToString(exercises, LessonpodConstant.CorrectAnswer);
                int Weight = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.Weight);
                smartLabelBoxResponse.Weight = Weight;
                totalWeight += Weight;

                JObject pluginToolbarsByIdState = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.State);
                smartLabelBoxResponse.FontSize = DistributionCommonHelper.JObjectToInteger(pluginToolbarsByIdState, LessonpodConstant.FontSize);
                JObject pluginToolbarsByIdStructure = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.Structure);
                smartLabelBoxResponse.Width = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdStructure, LessonpodConstant.Width);
                smartLabelBoxResponse.Rotation = DistributionCommonHelper.JObjectToInteger(pluginToolbarsByIdStructure, LessonpodConstant.Rotation);
                smartLabelBoxResponse.Height = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdStructure, LessonpodConstant.Height);
                smartLabelBoxResponse.CaseSensitive = DistributionCommonHelper.JObjectToBoolean(pluginToolbarsByIdState, LessonpodConstant.Characters);
                smartLabelBoxResponse.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.SequenceMessage);
                smartLabelBoxResponse.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.CorrectAnswerExplanation);
                smartLabelBoxResponse.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.IncorrectAnswerExplanation);
                #endregion

                smartLabelBoxResponse.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.SpeechAudioData);
                smartLabelBoxResponse.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.OptionsAudioData);
                smartLabelBoxResponse.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.FeedbackAudioData);
                smartLabelBoxResponse.BoxOverlay = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.BoxOverlay);

                listSmartLabelBoxResponses.Add(smartLabelBoxResponse);
            }
            #endregion

            #region Label properties
            smartLabelSlideResponse.SmartLabelBoxes = new List<SmartLabelBoxResponse>(listSmartLabelBoxResponses.OrderBy(SmartLabelBox => SmartLabelBox.Priority).ToList());
            JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);
            smartLabelSlideResponse.TotalWeight = totalWeight;
            smartLabelSlideResponse.EditorImageWidth = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.EditorWidth);
            smartLabelSlideResponse.EditorImageHeight = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.EditorHeight);
            smartLabelSlideResponse.BackgroundImage = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.ImageInUrl);
            smartLabelSlideResponse.Opacity = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.Opacity);
            #endregion

            #region Text to speech
            smartLabelSlideResponse.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.TextToSpeech));
            smartLabelSlideResponse.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.Seconds));
            smartLabelSlideResponse.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.SpeechAudioData);
            smartLabelSlideResponse.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.OptionsAudioData);
            smartLabelSlideResponse.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            #region Label slide response
            lessonUnitSlideResponse.TotalPoints = totalWeight;
            lessonUnitSlideResponse.SearchTag = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.SearchTag);
            lessonUnitSlideResponse.FileSize = DistributionCommonHelper.JObjectToInteger(navItemsById, LessonpodConstant.FileSize);
            smartLabelJson = JsonSerializer.Serialize(smartLabelSlideResponse);
            lessonUnitSlideResponse.SlideJson = smartLabelJson;
            #endregion

            return lessonUnitSlideResponse;
        }
        #endregion

    }
}
