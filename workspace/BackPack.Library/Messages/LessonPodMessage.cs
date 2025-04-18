namespace BackPack.Library.Messages
{
    public static class LessonPodMessage
    {
        public const string LessonPodNotExist = "Lesson pod does not exit.";

        #region Distribution
        public const string DistributionSummary = "Lesson pod distribution";
        public const string DistributionDescription = "Lesson pod distribution for students";

        public const string NoStudents = "No student available for distribution";
        public const string LessonPodDistributionSuccess = "Lesson Pod distributed successfully";
        public const string LessonPodDistributionFail = "Your request could not be processed. Please try again or contact your administrator.";
        public const string LessonPodDistributionAuditLog = "Lesson pod distribution ";
        #endregion

        #region Lesson pod redistribution
        public const string LessonPodReDistributionSummary = "Lesson pod redistribution";
        public const string LessonPodReDistributionDescription = "Lesson pd redistribution for students";

        public const string LessonPodReDistributionSuccess = "You have successfully distributed the lesson pods";
        public const string LessonPodReDistributionFail = "Your request could not be processed. Please try again or contact your administrator.";
        public const string LessonPodReDistributionAuditLog = "Lesson pod redistribution ";
        #endregion

        #region UpdateLessonPodProperties
        public const string UpdateLessonPodPodPropertiesSummary = "Update lesson pod properties";
        public const string UpdateLessonPodPodPropertiesDescription = "Update lesson pod properties";

        public const string UpdateLessonPodPropertiesSuccess = "Lesson pod properties changed successfully.";
        public const string UpdateLessonPodPropertiesFail = "Changing the properties of this Lesson pod is not allowed.";
        #endregion

        #region Create lesson pod
        public const string CreateLessonPodSummary = "Create lesson pod";
        public const string CreateLessonPodDescription = "Create lesson pod";

        public const string CreateLessonPodSuccess = "Lesson Pod saved successfully.";
        public const string CreateLessonPodFail = "Lesson Failed to save. Please try again.";
        public const string CreateLessonPodUpdationFail = "Editing this lesson unit is not allowed for this user.";
        public const string CreateLessonPodVersionError = "Property Version Number failed validation.Error was: 'Version Number' must not be empty.";
        public const string UpdateLessonPodSuccess = "Lesson Pod updated successfully.";
        public const string UpdateLessonPodProperty = "Updated Lesson Pod properties ";
        #endregion

        #region Copy lesson pod
        public const string CopyLessonPodSummary = "Copy lesson pod";
        public const string CopyLessonPodDescription = "Copy lesson pod";

        public const string CopyLessonPodSuccess = "Lesson pod copied successfully.";
        public const string CopyLessonPodFail = "Lesson pod could not be copied. Please try again.";
        #endregion

        #region Recall lesson pod distribution
        public const string RecallLessonPodSummary = "Recall lesson pod";
        public const string RecallLessonPodDescription = "Recall lesson pod distribution";

        public const string RecallLessonPodSuccess = "You have successfully recalled the lesson pods.";
        public const string RecallLessonPodFail = "Lesson pod could not be recalled. Please try again.";
        public const string LessonPodRecallDistributionAuditLog = "Recall Lesson pod distribution ";
        #endregion

        #region Delete lesson pod
        public const string DeleteLessonPodSummary = "Delete lesson pod";
        public const string DeleteLessonPodDescription = "Delete lesson pod";

        public const string DeleteLessonPodSuccess = "Lesson pod deleted successfully.";
        public const string DeleteLessonPodFail = "Lesson pod could not be deleted. Please try again.";
        #endregion

        #region CreatePreviewLessonPodActivities
        public const string CreatePreviewLessonPodActivitySummary = "Create lesson pod preview activity";
        public const string CreatePreviewLessonPodActivityDescription = "Create lesson pod preview activity";

        public const string CreatePreviewLessonPodActivitySuccess = "Success.";
        public const string CreatePreviewLessonPodActivityFail = "Failure.";
        #endregion

        #region CreateStudioSlideTemplate
        public const string CreateStudioSlideTemplateSummary = "Create lesson pod slide template";
        public const string CreateStudioSlideTemplateDescription = "Create lesson pod slide template";

        public const string CreateStudioSlideTemplateSuccess = "Success.";
        public const string CreateStudioSlideTemplateFail = "Failure.";
        #endregion

        #region DeleteStudioSlideTemplate
        public const string DeleteStudioSlideTemplateSummary = "Delete lesson pod slide template";
        public const string DeleteStudioSlideTemplateDescription = "Delete lesson pod slide template";

        public const string DeleteStudioSlideTemplateSuccess = "Success.";
        public const string DeleteStudioSlideTemplateFail = "Failure.";
        public const string DeleteStudioSlideTemplate = "Deleted slide template";
        #endregion

        #region Update lesson in lesson pod
        public const string UpdateLessonInLessonPodSummary = "Update lesson in lesson pod";
        public const string UpdateLessonInLessonPodDescription = "Update lesson in lesson pod";

        public const string UpdateLessonInLessonPodSuccess = "Lesson pod moved successfully.";
        public const string UpdateLessonInLessonPodFail = "Moving this lesson pod is not allowed.";

        public const string UpdateLessonInLessonPod = "Moved Lesson pod.";
        #endregion

        #region Unlock offline lesson pod
        public const string UnlockOfflineLessonUnitByCourseIDSummary = "Unlock offline lesson unit";
        public const string UnlockOfflineLessonUnitByCourseIDDescription = "Unlock offline lesson unit based on the course";

        public const string UnlockOfflineSuccess = "Offline lesson pod exercise unlocked successfully";
        public const string UnlockOfflineNoRecord = "There is no Offline lesson pod exercise to unlock";
        #endregion

        #region Lesson folder
        public const string GetLessonFoldersBySubjectSummary = "Get Lesson folder details";
        public const string GetLessonFoldersBySubjectDescription = "Get all lesson olders based on the input";
        #endregion

        #region Lesson folder
        public const string PendingLessonPodsForAStudentSummary = "Pending lesson pod";
        public const string PendingLessonPodsForAStudentDescription = "Pending lesson pod based on the student";
        #endregion

        #region GetBPCompletedLessonUnitsByLesson
        public const string CompletedLessonPodsByLessonSummary = "Completed lesson pods";
        public const string CompletedLessonPodsByLessonDescription = "Completed lesson pods based on the student";
        #endregion

        #region GetLessonUnitsForAStudent
        public const string LessonUnitsForAStudentSummary = "Completed and pending lesson pods";
        public const string LessonUnitsForAStudentDescription = "Completed and pending lesson pods based on the student";
        #endregion

        #region GetLessonUnitSummaryForAStudent
        public const string LessonPodSummaryForAStudentSummary = "Lesson pod summary";
        public const string LessonPodSummaryForAStudentDescription = "List of activities based on Lesson pod";
        #endregion

        #region GetLessonUnitDetailsForAStudent
        public const string LessonPodDetailsForAStudentSummary = "Lesson pod details";
        public const string LessonPodDetailsForAStudentDescription = "List of Lesson pod activities based on Lesson pod";
        #endregion

        #region GetBPSyncCourseDownload
        public const string SyncCourseDownloadSummary = "Sync course download";
        public const string SyncCourseDownloadDescription = "List of course download for sync";
        #endregion

        #region GetBPSyncLessonUnitDownload
        public const string SyncLessonPodDownloadSummary = "Sync lesson pod download";
        public const string SyncLessonPodDownloadDescription = "List of lesson pod activity download for sync";
        #endregion

        #region GetBPSyncCourseLessonUnitDownload
        public const string SyncCourseLessonPodDownloadSummary = "Sync lesson pod ID and file size download";
        public const string SyncCourseLessonPodDownloadDescription = "List of lesson pod ID and file size download for sync based on course IDs";
        #endregion

        #region GetDistributeLessonUnit
        public const string DistributeLessonPodSummary = "Lesson pod slide";
        public const string DistributeLessonPodDescription = "List of lesson pod slides";
        #endregion

        #region GetDistributedLessonUnit
        public const string DistributedLessonPodSummary = "Distributed Lesson pod slide";
        public const string DistributedLessonPodDescription = "Distributed List of lesson pod slides";
        #endregion

        #region GetAllDistributedLessonodsByTeacherByLessonPod
        public const string AllDistributedLessonodsByTeacherByLessonPodSummary = "Distributed Lesson pods";
        public const string AllDistributedLessonodsByTeacherByLessonPodDescription = "Distributed List of lesson pods";
        #endregion

        #region GetCRLessonUnitDetails
        public const string CRLessonUnitDetailsSummary = "Lesson pod activities";
        public const string CRLessonUnitDetailsDescription = "List of lesson pod activities";
        #endregion

        #region GetLessonUnit
        public const string GetLessonPodSummary = "Lesson pod details";
        public const string GetLessonPodDescription = "Lesson pod details based on lesson pod";
        #endregion

        #region GetAllMyLessonUnitByLesson
        public const string AllMyLessonPodsSummary = "Lesson pod list";
        public const string AllMyLessonPodsDescription = "List of Lesson pods based on lesson";
        #endregion

        #region GetAllMyLessonUnitByLesson
        public const string AllActivitiesByLessonPodSummary = "Lesson pod activity list";
        public const string AllActivitiesByLessonPodDescription = "Lesson pod activity based on lesson pod";
        #endregion

        #region GetAllStudioSlideTemplates
        public const string AllSlideTemplateSummary = "Lesson pod slide template list";
        public const string AllSlideTemplateDescription = "list of Lesson pod slide template";
        #endregion

        #region Create Lesson
        public const string CreateLesson = "Created Lesson";
        public const string CreateLessonSummary = "Create Lesson";
        public const string CreateLessonDescription = "Create Lesson";
        public const string CreateLessonDuplicate = "Duplicate Lesson name";
        public const string LessonCreationFail = "Lesson not saved successfully";
        public const string CreateLessonCapsule = "Lesson saved successfully";
        #endregion 

        #region Create Lesson
        public const string DeleteLesson = "Deleted Lesson";   
        public const string DeleteLessonSummary = "Delete Lesson";  
        public const string DeleteLessonDescription = "Deleted Lesson";
        public const string DeleteCreationFail = "Lesson not Deleted successfully";
        public const string DeleteLessonCapsule = "Lesson Deleted successfully";
        #endregion 

        #region Update Lesson
        public const string UpdateLesson = "Update Lesson";
        public const string UpdateLessonSummary = "Update Lesson"; 
        public const string UpdateLessonDescription = "Update Lesson";
        public const string InvalidChapter = "Invalid Chapter";
        public const string UpdateLessonDuplicate = "Duplicate Name";
        public const string UpdateCreationFail = "Lesson not Update successfully";
        public const string UpdateLessonCapsule = "Lesson Update successfully";
        #endregion

        #region GetLesson
        public const string GetLessonSummary = "Lesson  details";
        public const string GetLessonDescription = "Lesson  details based on lesson";
        #endregion
    }
} 
