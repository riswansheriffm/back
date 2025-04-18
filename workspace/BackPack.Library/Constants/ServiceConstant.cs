namespace BackPack.Library.Constants
{
    public static class ServiceConstant
    {
        public const string TokenType = "Bearer";

        #region User type
        public const string Teacher = "Teacher";
        public const string Student = "Student";
        #endregion

        #region Service name
        public const string TeacherLoginService = "TeacherLogin";
        public const string StudentLoginService = "StudentLogin";
        public const string RefreshTokenService = "RefreshToken";
        public const string ActivateUserAccountService = "ActivateUserAccount";
        public const string ResetPasswordService = "ResetPassword";
        public const string CreatePasswordService = "CreatePassword";
        public const string ServiceStatusService = "ServiceStatus";
        public const string ServiceCreateDocument = "CreateDocument";
        public const string ServiceGetAllDocument = "GetAllDocument";
        #endregion

        #region Slide activity
        public const string SmartLabel = "Smart Label";
        public const string SmartTile = "Smart Tile";
        public const string SmartPaper = "Smart Paper";
        public const string SmartSlide = "Smart Slide";
        #endregion

        #region Slide app name
        public const string AppSmartLabel = "SmartLabel";
        public const string AppSmartTile = "SmartTiles";
        public const string AppSmartPaper = "SmartPaper";
        public const string AppSmartSlide = "SmartSlide";
        #endregion

        #region Plugin name
        public const string ComReplayRectangle = "ReplayRectangle";
        public const string ComTextArea = "TextArea";
        public const string ComTextInk = "TextInk";
        public const string ComRichText = "RichText";
        public const string ComPopupSlide = "PopupSlide";
        public const string ComHotspotImages = "HotspotImages";
        public const string ComEnrichedAudio = "EnrichedAudio";
        public const string ComEnrichedPlayer = "EnrichedPlayer";
        public const string ComEnrichedPDF = "EnrichedPDF";
        public const string ComVisor3D = "Visor3D";
        public const string ComAGTextList = "AGTextList";
        public const string ComAGInput = "AGInput";
        public const string ComSSMultipleChoice = "SSMultipleChoice";
        public const string ComSSMultipleAnswer = "SSMultipleAnswer";
        public const string ComSSTrueFalse = "SSTrueFalse";
        public const string ComSSRotatingDial = "SSRotatingDial";
        public const string ComSSFillinTheBlanks = "SSFillinTheBlanks";
        public const string ComSPHotspot = "SPHotspot";
        public const string ComFraction = "Fraction";
        public const string ComMatchTable = "MatchTable";
        public const string ComH5P = "H5P";
        public const string ComWebpage = "Webpage";
        public const string ComAGInkList = "AGInkList";
        public const string ComSSArrange = "SSArrange";
        public const string ComSSMatchParent = "SSMatchParent";
        public const string ComSSCategory = "SSCategory";
        public const string ComDynamicCategory = "DynamicCategory";
        public const string ComHotspot = "Hotspot";

        public const string ComSCTextArea = "SCTextArea";
        public const string ComSCReplayRectangle = "SCReplayRectangle";
        public const string ComSCRichText = "SCRichText";
        public const string ComSCHotspotImages = "SCHotspotImages";
        public const string ComSCAGTextList = "SCAGTextList";
        public const string ComSCMultipleChoice = "SCMultipleChoice";
        public const string ComSCMultipleAnswer = "SCMultipleAnswer";
        public const string ComSCTrueFalse = "SCTrueFalse";
        public const string ComAssesmentBlock = "AssesmentBlock";
        public const string ComDropdown = "Dropdown";
        public const string ComInputAnswer = "InputAnswer";
        public const string ComSCArrangeOrder = "SCArrangeOrder";
        public const string ComSCAnswerPad = "SCAnswerPad";
        public const string ComSCTitle = "SCTitle";
        public const string ComSCBasicText = "SCBasicText";
        public const string ComSBasicText = "SBasicText";
        #endregion

        #region Schema name
        public const string SchemaMasters = "masters.";
        public const string SchemaUsers = "users.";
        public const string SchemaStudents = "students.";
        public const string SchemaLessonpods = "lessonpods.";
        public const string SchemaLogs = "logs.";
        #endregion

        #region Email
        public const string EmailSettings = "EmailSettings";
        public const string DisplayName = "DisplayName";
        public const string From = "From";
        public const string Host = "Host";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string Port = "Port";
        #endregion        
    }
}
