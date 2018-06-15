using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public class CRUD
    {
        private HelpDeskDataContext _context;

        public CRUD()
        {
            _context = new HelpDeskDataContext();
        }

        public void CreateWorkOrder(string requestorEmailAddress, DateTime dateTimeReceived, string subject, string workOrderDescription)
        {
            var appUserId = GetAppUserIdByEmail(requestorEmailAddress);
            var workOrder = new WorkOrder()
            {
                CreateDateTime = dateTimeReceived,
                ModifiedDateTime = dateTimeReceived,
                StatusDateTime = dateTimeReceived,
                RequestorId = appUserId,
                LocationId = appUserId == null ? null : GetLocationIdByAppUserId(appUserId),
                UnitId = appUserId == null ? null : GetUnitIdByAppUserId(appUserId),
                StatusId = 1,
                Subject = subject
            };

            _context.WorkOrders.InsertOnSubmit(workOrder);
            _context.SubmitChanges();

            var workOrderComment = new WorkOrderComment()
            {
                CommentorId = appUserId,
                CreateDateTime = dateTimeReceived,
                WorkOrderId = workOrder.Id,
                Comment = dateTimeReceived + "\n\r" + requestorEmailAddress + "\n\r" + workOrderDescription
            };

            _context.WorkOrderComments.InsertOnSubmit(workOrderComment);
            _context.SubmitChanges();

            SendEmail(workOrder, requestorEmailAddress, workOrderDescription);

        }

        public void SendEmail(WorkOrder workOrder, string sendToEmailAddress, string workOrderDescription)
        {
            var email = new EMail();
            email.CreateDateTime = DateTime.Now;
            email.ToEmailAddress = sendToEmailAddress;
            email.Subject = $"{workOrder.Subject} - Work Order # {workOrder.Id} Created";
            email.Body = $"Work Order #{workOrder.Id} has been created per your request." + $"\n\r\n\rStatus: NEW";
            email.Body = email.Body + "\n\r\n\r\n\rAssigned To: Not Currently Assigned";

            email.Body = email.Body + "\n\rLocation: " + "No Location Provided";
            email.Body = email.Body + $"\n\r\n\r\n\r\n\rCategory: Uncategorized\n\r\n\r\n\rSubject: {workOrder.Subject}";
            email.Body = email.Body + $"\n\r\n\r\n\r{workOrderDescription}";
            email.Sent = false;

            _context.EMails.InsertOnSubmit(email);
            _context.SubmitChanges();
        }

        public string GetAppUserIdByEmail(string emailAddress)
        {
            var appUser = _context.AppUsers.SingleOrDefault(a => a.EmailAddress == emailAddress);
            return appUser == null ? null : appUser.Id;
        }

        public int? GetLocationIdByAppUserId(string appUserId)
        {
            return _context.AppUsers.Single(a => a.Id == appUserId).LocationId;
        }

        public int? GetUnitIdByAppUserId(string appUserId)
        {
            return _context.AppUsers.Single(a => a.Id == appUserId).UnitId;
        }
    }
}