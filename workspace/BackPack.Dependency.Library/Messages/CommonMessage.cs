namespace BackPack.Dependency.Library.Messages
{
    public static class CommonMessage
    {
        public const string ExceptionTypeValidation = "Validation";
        public const string ExceptionTypeFail = "Fail";
        public const string ExceptionTypeException = "Exception";
        public const string ExceptionTypeSqlException = "SqlException";
        public const string ExceptionTypeNormal = "Normal";
        public const string ServiceStatusSuccess = "Success";
        public const string ServiceStatusFail = "Fail";

        public const string UnauthorizedMessage = "Unauthorized user.";
        public const string BadRequestMessage = "Authentication failed. Missing Mandatory Data.";
        public const string CreatedMessage = "Response message";
        public const string InternalServerErrorMessage = "Authentication failed. Please try again.";
        public const string ExceptionMessage = "Authentication failed. Please try again.";
        public const string ConflictMessage = "Conflict name.";
        public const string ReadMessage = "Data read successully.";
        public const string InvalidDomainMessage = "Authentication failed. Unauthorized user.";
        public const string GatewayFailMessage = "Authentication failed. Please try again.";

        public const string ValidationMessageLeft = "Property ";
        public const string ValidationMessageMiddle = " failed validation. Error was: ";

        public const string InvalidToken = "The token is invalid.";
        public const string ExpiredToken = "The token is expired.";

        #region Message ID
        public const int SuccessID = 0;
        public const int InvalidParameterID = 1;
        public const int FailID = 2;
        public const int ExceptionID = 3;
        public const int NotFoundID = 4;
        public const int DuplicateID = 5;
        public const int ErrorID = 6;
        public const int LockedID = 7;
        #endregion
    }
}
