using BackPack.Library.Responses.LessonPod.Distribution.SmartTile;
using BackPack.Library.Responses.LessonPod.Distribution;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using BackPack.Library.Constants;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class SmartTileHelper
    {
        #region SmartTileJson
        public static LessonPodSlideResponse SmartTileJson(JObject lessonJson, string slideId)
        {
            #region Declaration
            LessonPodSlideResponse response = new();
            SmartTileResponse smartTileResponse = new();
            smartTileResponse.SmartTileBoxes = new List<List<SmartTileBoxResponse>>();
            smartTileResponse.DesktopSmartTileBoxes = new List<List<SmartTileBoxResponse>>();
            List<SmartTileBoxResponse> listSmartTileBox = [];
            List<SmartTileBoxResponse> listDesktopSmartTileBox = [];
            #endregion

            JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);

            #region Desktop Tile count for each row 
            List<int> desktopList = new()
            {
                (int)navItemsById["row1"]!,
                (int)navItemsById["row2"]!,
                (int)navItemsById["row3"]!
            };
            int[] desktopTileColumn = desktopList.ToArray();
            #endregion

            #region App Tile count for each row 
            List<int> rowList = new()
            {
                (int)navItemsById["row1Tab"]!,
                (int)navItemsById["row2Tab"]!,
                (int)navItemsById["row3Tab"]!
            };
            int[] tileColumn = rowList.ToArray();
            #endregion

            #region Top & Bottom tile type and tile ratio
            smartTileResponse.TopTile = DistributionCommonHelper.JObjectToString(navItemsById, "toptile");
            smartTileResponse.BottomTile = DistributionCommonHelper.JObjectToString(navItemsById, "bottomtile");
            smartTileResponse.TileRatio = DistributionCommonHelper.JObjectToString(navItemsById, "tileratio");
            #endregion

            #region Tile box properties
            int totalTiles = 0;
            int ControlID = 0;
            int FileSize = 0;
            int rowIndex = 0;
            int totalScore = 0;
            int columnIndex = 1;

            JArray slideBoxes = DistributionCommonHelper.JObjectToJArray(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Boxes);
            foreach (var box in slideBoxes)
            {
                totalTiles++;
                ControlID++;
                int columnCount = tileColumn[rowIndex];

                SmartTileBoxResponse smartTileBox = new();
                smartTileBox.ControlID = ControlID;
                string boxId = (string)box!;

                #region Top & Bottom tile content
                JObject pluginToolbarsByIdState = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.State);
                smartTileBox.TopSrc = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, "topsrc");
                smartTileBox.BottomSrc = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, "bottomsrc");
                smartTileBox.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.CorrectAnswerExplanation);
                smartTileBox.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.IncorrectAnswerExplanation);
                smartTileBox.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.SequenceMessage);

                smartTileBox.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.SpeechAudioData);
                smartTileBox.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.OptionsAudioData);
                smartTileBox.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.FeedbackAudioData);
                #endregion

                #region Weight
                JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
                int weight = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.Weight);
                smartTileBox.Weight = weight;
                totalScore += weight;
                #endregion

                #region Slide Tile Row and Column
                listSmartTileBox.Add(smartTileBox);

                if (columnIndex == columnCount)
                {
                    smartTileResponse.SmartTileBoxes!.Add(new List<SmartTileBoxResponse>(listSmartTileBox));
                    listSmartTileBox.Clear();
                    rowIndex++;
                    columnIndex = 1;
                }
                else
                {
                    columnIndex++;
                }
                #endregion
            }
            #endregion

            #region Desktop Tile Box
            int desktopRowIndex = 0;
            int desktopColumnIndex = 1;
            ControlID = 0;
            foreach (var box in slideBoxes)
            {
                ControlID++;
                int desktopColumnCount = desktopTileColumn[desktopRowIndex];

                SmartTileBoxResponse desktopSmartTileBox = new();
                desktopSmartTileBox.ControlID = ControlID;
                string boxId = (string)box!;

                #region Top & Bottom tile content                
                JObject pluginToolbarsByIdState = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.PluginToolbarsById, thirdAttribute: boxId, fourthAttribute: LessonpodConstant.State);
                desktopSmartTileBox.TopSrc = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, "topsrc");
                desktopSmartTileBox.BottomSrc = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, "bottomsrc");
                desktopSmartTileBox.SequenceHint = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.SequenceMessage);
                desktopSmartTileBox.CorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.CorrectAnswerExplanation);
                desktopSmartTileBox.IncorrectAnswerExplanation = DistributionCommonHelper.JObjectToString(pluginToolbarsByIdState, LessonpodConstant.IncorrectAnswerExplanation);

                desktopSmartTileBox.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.SpeechAudioData);
                desktopSmartTileBox.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.OptionsAudioData);
                desktopSmartTileBox.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: pluginToolbarsByIdState, firstAttribute: LessonpodConstant.FeedbackAudioData);

                FileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsByIdState, "topfileSize");
                FileSize += DistributionCommonHelper.JObjectToInteger(pluginToolbarsByIdState, "bottomfileSize");
                #endregion

                #region Weight
                JObject exercises = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.Exercises, thirdAttribute: slideId, fourthAttribute: LessonpodConstant.Exercises, fifthAttribute: boxId);
                desktopSmartTileBox.Weight = DistributionCommonHelper.JObjectToInteger(exercises, LessonpodConstant.Weight);
                #endregion

                #region Slide Tile Row and Column
                listDesktopSmartTileBox.Add(desktopSmartTileBox);

                if (desktopColumnIndex == desktopColumnCount)
                {
                    smartTileResponse.DesktopSmartTileBoxes!.Add(new List<SmartTileBoxResponse>(listDesktopSmartTileBox));
                    listDesktopSmartTileBox.Clear();
                    desktopRowIndex++;
                    desktopColumnIndex = 1;
                }
                else
                {
                    desktopColumnIndex++;
                }
                #endregion
            }
            #endregion

            #region Tile properties
            JObject viewToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: "viewToolbarsById", thirdAttribute: slideId);
            smartTileResponse.SlideName = DistributionCommonHelper.JObjectToString(viewToolbarsById, LessonpodConstant.ViewName);
            smartTileResponse.DocumentTitle = DistributionCommonHelper.JObjectToString(viewToolbarsById, "documentSubtitleContent");
            smartTileResponse.BackgroundImage = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.ImageInUrl);
            smartTileResponse.Opacity = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.Opacity);
            smartTileResponse.FontName = DistributionCommonHelper.JObjectToString(viewToolbarsById, "fontName");
            smartTileResponse.FontColor = DistributionCommonHelper.JObjectToString(viewToolbarsById, "fontColor");
            smartTileResponse.FillColor = DistributionCommonHelper.JObjectToString(viewToolbarsById, "fillColor");
            smartTileResponse.FontSize = DistributionCommonHelper.JObjectToString(viewToolbarsById, LessonpodConstant.FontSize);
            smartTileResponse.Bold = DistributionCommonHelper.JObjectToBoolean(viewToolbarsById, LessonpodConstant.Bold);
            smartTileResponse.Italic = DistributionCommonHelper.JObjectToBoolean(viewToolbarsById, LessonpodConstant.Italic);
            smartTileResponse.TotalTiles = slideBoxes.Count;
            smartTileResponse.TotalScore = totalScore;
            #endregion

            #region Text to speech
            smartTileResponse.TextToSpeech = DistributionCommonHelper.JArrayToListStringList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.TextToSpeech));
            smartTileResponse.Seconds = DistributionCommonHelper.JArrayToIntList(DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.Seconds));

            smartTileResponse.SpeechAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.SpeechAudioData);
            smartTileResponse.OptionsAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.OptionsAudioData);
            smartTileResponse.FeedbackAudioData = DistributionCommonHelper.JObjectToObject(jObject: navItemsById, firstAttribute: LessonpodConstant.FeedbackAudioData);
            #endregion

            #region Response
            response.TotalPoints = totalScore;
            response.SearchTag = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.SearchTag);
            response.FileSize = FileSize;
            string SmartTileJson = JsonSerializer.Serialize(smartTileResponse);
            response.SlideJson = SmartTileJson;
            #endregion

            return response;
        }
        #endregion

    }
}
