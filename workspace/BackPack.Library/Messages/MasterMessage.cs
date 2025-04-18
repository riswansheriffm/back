namespace BackPack.Library.Messages
{
    public static class MasterMessage
    {
        #region Group
        public const string CreateGroupSummary = "Create group";
        public const string CreateGroupDescription = "Create group";
        public const string UpdateGroupSummary = "Update group";
        public const string UpdateGroupDescription = "Update group";
        public const string DeleteGroupSummary = "Delete group";
        public const string DeleteGroupDescription = "Delete group";
        public const string EditGroupSummary = "Edit group";
        public const string EditGroupDescription = "Edit group";
        public const string ListGroupSummary = "List of group details";
        public const string ListGroupDescription = "List of group details based on the Course";

        public const string GroupInvalidCourse = "Course does not exists.";
        public const string GroupInvalidStudent = "Student does not exists.";
        #endregion

        #region Subject
        public const string SubjectSummary = "Get Subject Details";
        public const string SubjectDescription = "Get Subject Details";

        public const string CreateGroupSuccess = "Group added successfully.";
        public const string UpdateGroupSuccess = "Group updated successfully.";
        public const string DeleteGroupSuccess = "Group deleted successfully.";
        public const string GroupDuplicate = "Group name already exists.";
        public const string AllSubjectSummary = "Get All Subject Details";
        public const string AllSubjectDescription = "Get All Subject Details";

        #region Putservice

        public const string CreateSubjectsSummary = "Create Subject";
        public const string CreateSubjectsDescription = "Create Subject";
        public const string CreateSubject = "Subject created successfully";
        public const string SubjectCreationFail = "Subject not created successfully";
        public const string SubjectCreationDuplicate = "Duplicate Subject name";
        public const string CreateSubjects = "Created Lesson";

        public const string UpdateSubjectsSummary = "Update Subject";
        public const string UpdateSubjectsDescription = "Update Subject";
        public const string UpdateSubject = "Update saved successfully";
        public const string UpdateCreationFail = "Update not saved successfully";
        public const string UpdateCreationDuplicate = "Duplicate Subject name";
        public const string UpdateSubjects = "Update Lesson";

        public const string DeleteSubjectsSummary = "Delete Subject";
        public const string DeleteSubjectsDescription = "Delete Subject";
        public const string DeleteSubject = "Delete successfully";
        public const string DeleteCreationFail = "Subject not deleted";
        public const string DeleteCreationDuplicate = "Duplicate Subject name";
        public const string DeleteSubjects = "Delete Lesson";

        #endregion
        #endregion  

        #region Student
        public const string StudentSummary = "Get Student Details";
        public const string StudentDescription = "Get Student Details";

        public const string AllStudentSummary = "Get All Student Details";
        public const string AllStudentDescription = "Get All Student Details";
        #endregion

        #region Course
        public const string StudentListForACourseSummary = "List of students";
        public const string StudentListForACourseDescription = "List of students based on the Course";

        public const string AllCoursesForASubjectSummary = "List of course";
        public const string AllCoursesForASubjectDescription = "List of courses based on the subject";

        public const string GetCourseSummary = "List of course details";
        public const string GetCourseDescription = "List of courses details based on the CourseID";

        public const string GetAllCourseSummary = "List of all course details";
        public const string GetAllCourseDescription = "List of all courses details based on the SchoolID";

        public const string GetAllCourseCapsulesSummary = "List of all Course Capsules details";
        public const string GetAllCourseCapsulesDescription = "List of all Course Capsules details based on the ID ";

        public const string GetAllCoursesByDomainSummary = "Get All Courses By Domain details";
        public const string GetAllCoursesByDomainDescription = "Get All Courses By Domain details ";

        public const string GetAllCourseCapsulesForASubjectSummary = "Get All Course Capsules For A Subject";
        public const string GetAllCourseCapsulesForASubjectDescription = "Get All Course Capsules For A Subject";

        public const string GetAllCourseCapsuleFoldersForACourseCapsuleSummary = "Get All Course Capsule Folders For A Course Capsule";
        public const string GetAllCourseCapsuleFoldersForACourseCapsuleDescription = "Get All Course Capsule Folders For A Course Capsule";

        public const string GetAllPublicCourseCapsuleByDomainAndSubjectSummary = "Get All Public Course Capsule By Domain And Subject";
        public const string GetAllPublicCourseCapsuleByDomainAndSubjectDescription = "Get All Public Course Capsule By Domain And Subject";

        public const string GetAllLPStudentLicensesByCourseCapsuleSummary = "Get All LP Student Licenses By Course Capsule";
        public const string GetAllLPStudentLicensesByCourseCapsuleDescription = "Get All LP Student Licenses By Course Capsule";

        public const string GetAllLPCourseLicensesSummary = "Get All LP Course Licenses";
        public const string GetAllLPCourseLicensesDescription = "Get All LP Course Licenses";

        #region Putservice
        public const string CreateCoursesSummary = "Create Course";
        public const string CreateCoursesDescription = "Create Course";
        public const string CreateCourse = "Course created successfully";
        public const string CourseCreationFail = "Course not created successfully";
        public const string CourseCreationDuplicate = "Duplicate Course name";
        public const string CreateCourses = "Created Lesson";

        public const string UpdateCoursesSummary = "Update Course";
        public const string UpdateCoursesDescription = "Update Course";
        public const string UpdateCourse = "Update saved successfully"; 
        public const string UpdateCreationCourseFail = "Update not saved successfully";
        public const string UpdateCreationCourseDuplicate = "Duplicate Course name";
        public const string UpdateCourses = "Update Lesson"; 

        public const string DeleteCoursesSummary = "Delete Course";
        public const string DeleteCoursesDescription = "Delete Course";
        public const string DeleteCourse = "Delete successfully";
        public const string DeleteCreationCourseFail = "Course not deleted"; 
        public const string DeleteCreationCourseDuplicate = "Duplicate Course name";  
        public const string DeleteCourses = "Delete Lesson";
          
        public const string CreateBulkCoursesSummary = "Create Bulk Course Upload";
        public const string CreateBulkCoursesDescription = "Create Bulk Course Upload";
        public const string CreateBulkCourse = "Create Bulk Course Upload successfully";
        public const string BulkCourseCreationDuplicate = "Duplicate Create Bulk Course Upload";


        public const string MapBulkCoursesSummary = "Create Map Teacher to Bulk Course Upload";
        public const string MapBulkCoursesDescription = "Create Map Teacher to Bulk Course Upload";
        public const string MapBulkCourse = "Create Map Teacher to Bulk Course Upload successfully";
        public const string MapBulkCourseCreationDuplicate = "Duplicate Create Map Teacher to Bulk Course Upload";

        #endregion
        #endregion
         
        #region Lesson
        public const string AllLessonsForASubjectSummary = "List of students";
        public const string AllLessonsForASubjectDescription = "List of students based on the Course";
        public const string GetCourseCapsuleForReorderSummary = "Get Course Capsule For Reorder";
        public const string GetCourseCapsuleForReorderDescription = "Get Course Capsule For Reorder";
        #endregion

        #region Document
        public const string CreateDocumentSummary = "Create Document";
        public const string CreateDocumentDescription = "Create Document";

        public const string GetDocumentSummary = "Get Document";  
        public const string GetDocumentDescription = "Get Document";

        public const string CreateDocument = "Document created successfully";
        public const string CourseDocumentDuplicate = "Duplicate Document name";
        #endregion
    }
}   
  