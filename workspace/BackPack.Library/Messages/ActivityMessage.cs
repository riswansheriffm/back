using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPack.Library.Messages
{
    public static class ActivityMessage
    {
        #region Swagger
        public const string SaveActivitySummary = "Save student activity";
        public const string SaveActivityDescription = "Save student activity";

        public const string SaveTeacherFeedbackSummary = "Save student feedback";
        public const string SaveTeacherFeedbackDescription = "Save student feedback by teacher";
        #endregion

        public const string ExerciseSaved = "Exercise saved successfully";
        public const string ExerciseUpdated = "Exercise updated successfully";
        public const string ExerciseSubmitted = "Exercise submitted successfully";
        public const string ExerciseAlreadySubmitted = "This exercise has been already submitted and it cannot be submitted again";
        public const string TeacherFeedbackSaved = "Feedback saved successfully";
        public const string TeacherFeedbackRework = "Rework initiated successfully";
        public const string TeacherFeedbackNotSubmited = "Exercise not submitted by student";
        public const string OfflineExerciseSaveErrorOnline = "Offline lesson pod exercise cannot be submitted in online";
        public const string OnlineExerciseSaveErrorOffline = "Online lesson pod exercise cannot be submitted in offline";
        public const string OfflineExerciseDownloadVersionError = "Offline lesson pod exercise cannot be submitted with the old downloaded version";
        public const string DateFormatError = "is not a valid date format";

        #region GetBackpackActivityForStudent
        public const string BackpackActivityForStudentSummary = "Get student activity";
        public const string BackpackActivityForStudentDescription = "Get student activity based on student";
        #endregion

    }
}
