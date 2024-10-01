using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Common.Constants
{
    public class Constants
    {
        public const string Token = "authorization";
        public const string AuthenticationSchema = "Bearer";
    }

    public static class CommonConstant
    {
        public static DateTime DeafultDate = Convert.ToDateTime("1900/01/01");
        public static int SessionExpired = 30;
        public static string DateFormate = "yyyy/MM/dd";
    }

    public static class MessageConstant
    {
        //public const string RevertSuccessfully = "Revert successfully.";
        public const string SavedSuccessfully = "Saved successfully.";
        //public const string UpdatedSuccessfully = "Updated successfully.";
        //public const string SetAsDefaultSuccessfully = "The default has been set successfully.";
        //public const string RemoveDefaultSuccessfully = "The default has been removed successfully.";
        //public const string FilterAddedSuccesfully = "Filter added successfully.";
        public const string RegisterSuccessfully = "Register successfully.";
        public const string SaveFailed = "Failed to save information.";
        //public const string DeleteFailed = "Failed to delete.";
        public const string DeleteSuccess = "Delete successfully.";
        //public const string SaveUserssuccess = "User save successfully.";
        //public const string SaveUsersFail = "User save successfully.";
        //public const string DuplicateUserIDOrEmail = "Duplicate user name or email.";
        public const string DuplicateUserName = "Duplicate user name";
        public const string EmailRequired = "Email must be required";
        public const string UsernameRequired = "Username must be required";
        public const string PasswordRequired = "Password must be required";
        public const string EmailAlreadyExist = "Email already exist";
        //public const string PhoneNumberAlreadyExist = "Phone number already exist";
        public const string ConfirmPasswordNotMatch = "Confirm password not matched";
        //public const string DepartmentName = "Department name is required.";
        //public const string DepartmentNameExist = "This department name is already exist.";
        //public const string DepartmentStatusTitle = "Department status title is required.";
        //public const string DepartmentTaskID = "Please select department task.";
        //public const string TaskDescription = "Task description is required.";
        //public const string KnowledgeDetail = "Knowledge detail is required.";
        //public const string KnowledgeBase = "Please select knowledge.";
        //public const string PermissionName = "Permission name is required.";
        //public const string RoleName = "Role name is required.";
        //public const string OrganizationName = "Organization name is required.";
        //public const string TagTitle = "Tag title is required.";
        //public const string TableName = "Table name is required.";
        //public const string ColumnName = "Column name is required.";
        //public const string ActionName = "Action name is required.";
        //public const string PermissionChcek = "Permission selection required";
        //public const string DepartmentTaskDescription = "Department task description is required.";
        //public const string DepartmentTypeName = "Department type  name is required.";
        //public const string Email = "Email name is required.";
        //public const string EmployeeName = "Employee name is required.";
        public const string LoginSuccess = "Login successfully";
        //public const string Unauthorizerequest = "Unauthorize request.";
        //public const string InternalServerError = "Internal server error.";
        public const string LogOutSuccessfully = "Log out successfully.";
        public const string Invaliddatafound = "Invalid data found.";
        public const string Confirmpasswordnotmatch = "Confirm password not match.";
        //public const string PatientNote = "Patient Note is Required.";
        //public const string CaregiverNote = "Care giver Note is required.";
        public const string Token = "Token is required.";
        //public const string CustomFilterObject = "Filter object is required.";
        //public const string TagColor = "Tag color is required.";
        //public const string SelectUser = "Please select users.";
        //public const string AnyuserNotMapping = "Any user not mapping.";
        //public const string Message = "Message can not be null or empty.";
        //public const string ScheduleMessage = "Schedule created successfully";
        //public const string ScheduleMessagesenderInvalid = "Invalid sender contact";
        //public const string MessageSend = "All message send successfully.";
        //public const string ExceededMessage = "You have exceeded your message limit, Please check";
        //public const string SelectTwilioContact = "Please select a twilio contact to send the message";
        //public const string TwolioAccountAccess = "Twolio account access not found for this sender";
        //public const string Invalidcontact = "Invalid contact number :";
        //public const string FailedMessage = "Failed to send message";
        //public const string InvalidMessage = "Invalid message entered";
        //public const string RequestObject = "Request object not empty";
        //public const string Pinned = "Pinned Successfully";
        //public const string UnPinned = "Successfully unpin the contact";
        //public const string FailToSaveContact = "Could not found any existing twilio contact";
        //public const string DuplicateHHAXColumnSetting = "Duplicate column setting";
        //public const string UndoWarning = "Previous history not available";
        //public const string Attachment = "File content requried.";
        //public const string Caregiverfile = "File name requried.";
        //public const string UseTag = "This tag can not delete because this tag alrady used.";
        //public const string PatientID = "Patient Not is found.";
        //public const string CaregiverID = "Caregiver Not is found.";
        //public const string DuplicateTask = "Alrady has a task with the name.";
        //public const string DuplacteColumnName = "The Column name already has in default table.";
        //public const string CustomColumnAlreadyExist = "The Column name already has under this organization.";
        //public const string UsernameAndPasswordRequired = "Invalid username or password";
        //public const string TonumberAndFromNumber = "From number and To number must be required";
        //public const string MessageSenderKey = "Message sender key must be required";
        //public const string ColumnNotMatchBothTable = " Column not match both default and custom table.";
        //public const string FileProcesssingStart = "File processsing start.";
        //public const string DuplicateSharedFilter = "Filter has been already shared with this user.";
        //public const string ExportFileMessage = "Your report will be generated.";
        //public const string ExportFileDownload = "File download successfully.";
        //public const string FileCheck = "File can not be  null.";
        //public const string PatientNotFound = "No Patient found with the imported AdmissionIDs";
        //public const string CaregiverNotFound = "No Caregiver found with the imported CaregiverCodes";
        //public const string TagNotFound = "No tag found";
        //public const string DulicateMapping = "Already mapped this department.";
        //public const string DuplicateCustomFilterFolder = "Folder name already exist.";
        //public const string MailSendingStarted = "Mail sending started.";
        //public const string MailSendingFinished = "Mail sent successfully.";
    }
}
