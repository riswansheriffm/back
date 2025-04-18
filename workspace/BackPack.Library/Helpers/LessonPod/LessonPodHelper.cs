using BackPack.Library.Constants;
using BackPack.Library.Responses.LessonPod;
using Newtonsoft.Json.Linq;

namespace BackPack.Library.Helpers.LessonPod
{
    public static class LessonPodHelper
    {
        #region LessonPodSlideList
        public static List<LessonPodSlideListResponse> LessonPodSlideList(string LessonJson)
        {
            List<LessonPodSlideListResponse> response = [];
            int displayOrder = 0;

            if (!string.IsNullOrEmpty(LessonJson))
            {
                JObject lessonJson = JObject.Parse(LessonJson);
                JArray lessonPodSlides = DistributionCommonHelper.JObjectToJArray(secondAttribute: LessonpodConstant.NavItemsIds, jObject: lessonJson, firstAttribute: LessonpodConstant.Present);

                string slideId;
                string parentSlideId;
                for (int slideIndex = 0; slideIndex < lessonPodSlides.Count; slideIndex++)
                {
                    bool isParant = false;
                    slideId = lessonPodSlides[slideIndex].ToString();
                    JObject parentNavItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);

                    parentSlideId = DistributionCommonHelper.JObjectToString(parentNavItemsById, LessonpodConstant.ParentRefId);
                    if (parentSlideId == "") isParant = true;
                    while (!isParant)
                    {
                        for (int childIndex = 0; childIndex < lessonPodSlides.Count; childIndex++)
                        {
                            string clideSlideId = lessonPodSlides[childIndex].ToString();
                            JObject childNavItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: clideSlideId);
                            string parentSlide = "";
                            parentSlideId = DistributionCommonHelper.JObjectToString(childNavItemsById, LessonpodConstant.ParentRefId);
                            if (parentSlideId == "" && childIndex > slideIndex)
                            {
                                parentSlide = lessonPodSlides[childIndex].ToString();
                                lessonPodSlides[childIndex] = lessonPodSlides[slideIndex];
                                lessonPodSlides[slideIndex] = parentSlide;
                                break;
                            }
                        }
                        break;
                    }
                }

                for (int slideIndex = 0; slideIndex < lessonPodSlides.Count; slideIndex++)
                {
                    slideId = lessonPodSlides[slideIndex].ToString();
                    JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: slideId);
                    bool isAvailable = IsSlideExist(response, slideId);
                    parentSlideId = DistributionCommonHelper.JObjectToString(navItemsById, LessonpodConstant.ParentRefId);
                    if (parentSlideId != "") isAvailable = true;

                    if (!isAvailable)
                    {
                        displayOrder++;
                        LessonPodSlideListResponse podSlide = SlideResponse(lessonJson, slideId, displayOrder, string.Empty, false);
                        response.Add(podSlide);
                    }

                    JArray childrens = DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.SvChildren);
                    foreach (var child in childrens)
                    {
                        string childSlideId = child.ToString();
                        if (!IsSlideExist(response, childSlideId))
                        {
                            displayOrder++;
                            LessonPodSlideListResponse childSlide = SlideResponse(lessonJson, childSlideId, displayOrder, slideId, false);
                            response.Add(childSlide);
                        }

                        JObject secondNavItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: LessonpodConstant.NavItemsById, thirdAttribute: childSlideId);
                        JArray secondPagess = DistributionCommonHelper.JObjectToJArray(jObject: secondNavItemsById, firstAttribute: LessonpodConstant.ChildPagesFromParent);
                        foreach (var secondChild in secondPagess)
                        {
                            string secondPageSlideId = secondChild.ToString();
                            if (!IsSlideExist(response, secondPageSlideId))
                            {
                                displayOrder++;
                                LessonPodSlideListResponse childSlide = SlideResponse(lessonJson, secondPageSlideId, displayOrder, childSlideId, true);
                                response.Add(childSlide);
                            }
                        }
                    }

                    JArray childPages = DistributionCommonHelper.JObjectToJArray(jObject: navItemsById, firstAttribute: LessonpodConstant.ChildPagesFromParent);
                    foreach (var childPage in childPages)
                    {
                        string childPageSlideId = childPage.ToString();
                        if (!IsSlideExist(response, childPageSlideId))
                        {
                            displayOrder++;
                            LessonPodSlideListResponse childPageSlide = SlideResponse(lessonJson, childPageSlideId, displayOrder, slideId, true);
                            response.Add(childPageSlide);
                        }
                    }
                }
            }

            return response;
        }
        #endregion

        #region Check Existing Slide 
        private static bool IsSlideExist(List<LessonPodSlideListResponse> slides, string slideId)
        {
            bool isAvailable = false;
            for (int slideIndex = 0; slideIndex < slides.Count; slideIndex++)
            {
                if (slides[slideIndex].SlideID == slideId)
                {
                    isAvailable = true;
                    break;
                }
            }

            return isAvailable;
        }
        #endregion

        #region SlideResponse
        private static LessonPodSlideListResponse SlideResponse(JObject lessonJson, string slideId, int displayOrder, string parentSlideId, bool isChildPage)
        {
            LessonPodSlideListResponse response = new();
            JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: "navItemsById", thirdAttribute: slideId);
            JObject viewToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: lessonJson, firstAttribute: LessonpodConstant.Present, secondAttribute: "viewToolbarsById", thirdAttribute: slideId);
            bool isContainedView = DistributionCommonHelper.JObjectToBoolean(navItemsById, "containedview");
            bool isCanvas = DistributionCommonHelper.JObjectToBoolean(navItemsById, "iscanvas");

            response.SlideID = slideId;
            response.SlideType = DistributionCommonHelper.JObjectToString(navItemsById, "tempname");
            response.SlideName = DistributionCommonHelper.JObjectToString(viewToolbarsById, "viewName");
            response.DocumentTitle = DistributionCommonHelper.JObjectToString(viewToolbarsById, "documentSubtitleContent");
            response.IsContentView = isContainedView;
            response.DisplayOrder = displayOrder;
            response.IsCanvas = isCanvas;
            response.ParentSlideID = parentSlideId;
            response.IsChildPage = isChildPage;

            return response;
        }
        #endregion

        #region LessonPodActivityList
        public static List<AllActivitiesByLessonPodData> LessonPodActivityList(string LessonJson)
        {
            List<AllActivitiesByLessonPodData> response = [];

            if (!string.IsNullOrEmpty(LessonJson))
            {
                JObject jobject = JObject.Parse(LessonJson);
                JArray lessonPodSlides = DistributionCommonHelper.JObjectToJArray(jObject: jobject, firstAttribute: LessonpodConstant.Present, secondAttribute: "navItemsIds");
                foreach (var slide in lessonPodSlides)
                {
                    string slideId = slide.ToString();
                    AllActivitiesByLessonPodData podSlide = new();
                    JObject navItemsById = DistributionCommonHelper.JObjectToJObject(jObject: jobject, firstAttribute: LessonpodConstant.Present, secondAttribute: "navItemsById", thirdAttribute: slideId);
                    JObject viewToolbarsById = DistributionCommonHelper.JObjectToJObject(jObject: jobject, firstAttribute: LessonpodConstant.Present, secondAttribute: "viewToolbarsById", thirdAttribute: slideId);
                    podSlide.SlideID = slideId;
                    podSlide.SlideType = DistributionCommonHelper.JObjectToString(navItemsById, "tempname");
                    podSlide.SlideName = DistributionCommonHelper.JObjectToString(viewToolbarsById, "viewName");
                    response.Add(podSlide);
                }
            }
            return response;
        }
        #endregion

    }
}
